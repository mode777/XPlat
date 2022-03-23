using System;
using System.Collections.Generic;
using System.Numerics;
using XPlat.Core;
using XPlat.NanoVg;

namespace XPlat.Gui
{
    public class Widget
    {
        public Widget? Parent { get; set; }
        public bool MouseFocus => mouseFocus;
        public Vector2 Position;
        public Vector2 AbsolutePosition => Parent != null ? Parent.AbsolutePosition + Position : Position;
        public Vector2 Size;
        public float Width { get => Size.X; set => Size.X = value; }
        public float Height { get => Size.Y; set => Size.Y = value; }
        public Vector2 FixedSize = Vector2.Zero;
        public float FixedWidth { get => FixedSize.X; set => FixedSize.X = value; }
        public float FixedHeight { get => FixedSize.Y; set => FixedSize.Y = value; }
        public bool Visible { get; set; }
        public List<Widget> Children { get; } = new List<Widget>();
        public int ChildCount => Children.Count;       
        public bool Enabled { get; set; }
        public bool Focused { get; set; }
        public string Id { get; set; }
        public Widget(Widget? parent = null)
        {
            Visible = true;
            Enabled = true;
            Focused = false;
            mouseFocus = false;

            parent?.AddChild(this);
        }

        public bool VisibleRecursive()
        {
            bool visible = true;
            var widget = this;
            while (widget != null)
            {
                if (!widget.Visible)
                {
                    visible = false;
                    break;
                }
                widget = widget.Parent;
            }
            return visible;
        }

        public void AddChild(int index, Widget widget)
        {
            Children.Insert(index, widget);
            widget.Parent = this;
        }

        public void AddChild(Widget widget)
        {
            Children.Add(widget);
            widget.Parent = this;
        }

        public void RemoveChildAt(int index)
        {
            Children.RemoveAt(index);
        }

        public void RemoveChild(Widget widget)
        {
            Children.Remove(widget);
        }

        public Widget ChildAt(int index) => Children[index];
        public int ChildIndex(Widget widget) => Children.IndexOf(widget);

        public void RequestFocus()
        {
            //Screen?.UpdateFocus(this);
        }

        public bool Contains(Vector2 p)
        {
            Vector2 d = p - Position;
            return d.X >= 0 && d.Y >= 0 &&
                   d.X < Size.X && d.Y < Size.Y;
        }

        public Widget? FindWidget(Vector2 p)
        {
            foreach (var child in Children)
            {
                if (child.Visible && child.Contains(p - Position))
                    return child.FindWidget(p - Position);
            }
            return Contains(p) ? this : null;
        }

        public virtual bool MouseButtonEvent(Vector2 p, int button, bool down, int modifiers)
        {
            foreach (var child in Children)
            {
                if (child.Visible && child.Contains(p - Position) &&
                    child.MouseButtonEvent(p - Position, button, down, modifiers))
                    return true;
            }
            if (button == (int)MouseButton.Left && down && !Focused)
                RequestFocus();

            return false;
        }

        public virtual bool MouseMotionEvent(Vector2 p, Vector2 rel, int button, int modifiers)
        {
            bool handled = false;

            foreach (var child in Children)
            {
                if (!child.Visible) continue;

                var contained = child.Contains(p - Position);
                var prevContained = child.Contains(p - Position - rel);

                if (contained != prevContained)
                {
                    var hchild = child.MouseEnterEvent(p, contained);
                    handled = handled ? true : hchild;
                }

                if (contained || prevContained)
                {
                    var hchild = child.MouseMotionEvent(p - Position, rel, button, modifiers);
                    handled = handled ? true : hchild;
                }
            }

            return handled;
        }

        public virtual bool MouseDragEvent(Vector2 p, Vector2 rel, int button, int modifiers)
        {
            return false;
        }

        public virtual bool MouseEnterEvent(Vector2 p, bool enter)
        {

            mouseFocus = enter;
            return false;
        }

        public virtual bool ScrollEvent(Vector2 p, Vector2 rel)
        {
            foreach (var child in Children)
            {
                if (!child.Visible)
                {
                    continue;
                }
                if (child.Contains(p - Position) && child.ScrollEvent(p - Position, rel))
                    return true;
            }
            return false;
        }

        public virtual bool FocusEvent(bool focused)
        {
            Focused = focused;
            return false;
        }

        public virtual bool KeyboardEvent(int keycode, int scancode, int action, bool repeat, int modifiers)
        {
            return false;
        }

        public virtual bool KeyboardCharacterEvent(uint codepoint)
        {
            return false;
        }

        public virtual Vector2 PreferredSize(NVGcontext ctx)
        {
            return Size;
        }

        public virtual void PerformLayout(NVGcontext ctx)
        {
            var pref = PreferredSize(ctx);
            var fix = FixedSize;
            Size = new Vector2(
                fix.X != 0 ? fix.X : pref.X,
                fix.Y != 0 ? fix.Y : pref.Y);
            foreach (var c in Children)
            {
                c.PerformLayout(ctx);
            }
        }

        public virtual void Draw(NVGcontext vg)
        {
            vg.StrokeColor("#0f0");
            vg.BeginPath();
            vg.Rect(Position.X, Position.Y, Width, Height);
            vg.StrokeWidth(2);
            vg.Stroke();

            DrawChildren(vg);
        }

        protected virtual void DrawChildren(NVGcontext vg)
        {
            if (Children.Count == 0) return;

            vg.Translate(Position.X, Position.Y);
            foreach (var child in Children)
            {
                if (!child.Visible) continue;

                child.Draw(vg);
            }
            vg.Translate(-Position.X, -Position.Y);
        }

        protected bool mouseFocus;
        private float fontSize;
    }
}