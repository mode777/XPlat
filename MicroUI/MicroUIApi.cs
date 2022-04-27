// namespace MicroUI;
// public static class MicroUIApi
// {
//     /*
// ** Copyright (c) 2020 rxi
// **
// ** This library is free software; you can redistribute it and/or modify it
// ** under the terms of the MIT license. See `microui.c` for details.
// */
//     const string MU_VERSION = "2.01";
//     const int MU_COMMANDLIST_SIZE = (256 * 1024);
//     const int MU_ROOTLIST_SIZE = 32;
//     const int MU_CONTAINERSTACK_SIZE = 32;
//     const int MU_CLIPSTACK_SIZE = 32;
//     const int intSTACK_SIZE = 32;
//     const int MU_LAYOUTSTACK_SIZE = 16;
//     const int MU_CONTAINERPOOL_SIZE = 48;
//     const int MU_TREENODEPOOL_SIZE = 48;
//     const int MU_MAX_WIDTHS = 16;
//     // #define MU_REAL                 float
//     // #define MU_REAL_FMT             "%.3g"
//     // #define MU_SLIDER_FMT           "%.2f"
//     const int MU_MAX_FMT = 127;

//     //#define mu_stack(T, n)          struct { int idx; T items[n]; }
//     //#define Math.Min(a, b)            ((a) < (b) ? (a) : (b))
//     //#define mu_max(a, b)            ((a) > (b) ? (a) : (b))
//     //#define mu_clamp(x, a, b)       Math.Min(b, mu_max(a, x))

//     enum MU_CLIP
//     {
//         MU_CLIP_PART = 1,
//         MU_CLIP_ALL
//     };

//     enum MU_COMMAND
//     {
//         MU_COMMAND_JUMP = 1,
//         MU_COMMAND_CLIP,
//         MU_COMMAND_RECT,
//         MU_COMMAND_TEXT,
//         MU_COMMAND_ICON,
//         MU_COMMAND_MAX
//     };

//     public enum MU_COLOR
//     {
//         MU_COLOR_TEXT,
//         MU_COLOR_BORDER,
//         MU_COLOR_WINDOWBG,
//         MU_COLOR_TITLEBG,
//         MU_COLOR_TITLETEXT,
//         MU_COLOR_PANELBG,
//         MU_COLOR_BUTTON,
//         MU_COLOR_BUTTONHOVER,
//         MU_COLOR_BUTTONFOCUS,
//         MU_COLOR_BASE,
//         MU_COLOR_BASEHOVER,
//         MU_COLOR_BASEFOCUS,
//         MU_COLOR_SCROLLBASE,
//         MU_COLOR_SCROLLTHUMB,
//         MU_COLOR_MAX
//     };

//     enum MU_ICON
//     {
//         MU_ICON_CLOSE = 1,
//         MU_ICON_CHECK,
//         MU_ICON_COLLAPSED,
//         MU_ICON_EXPANDED,
//         MU_ICON_MAX
//     };

//     enum MU_RES
//     {
//         MU_RES_ACTIVE = (1 << 0),
//         MU_RES_SUBMIT = (1 << 1),
//         MU_RES_CHANGE = (1 << 2)
//     };

//     enum MU_OPT
//     {
//         MU_OPT_ALIGNCENTER = (1 << 0),
//         MU_OPT_ALIGNRIGHT = (1 << 1),
//         MU_OPT_NOINTERACT = (1 << 2),
//         MU_OPT_NOFRAME = (1 << 3),
//         MU_OPT_NORESIZE = (1 << 4),
//         MU_OPT_NOSCROLL = (1 << 5),
//         MU_OPT_NOCLOSE = (1 << 6),
//         MU_OPT_NOTITLE = (1 << 7),
//         MU_OPT_HOLDFOCUS = (1 << 8),
//         MU_OPT_AUTOSIZE = (1 << 9),
//         MU_OPT_POPUP = (1 << 10),
//         MU_OPT_CLOSED = (1 << 11),
//         MU_OPT_EXPANDED = (1 << 12)
//     };

//     enum MU_MOUSE
//     {
//         MU_MOUSE_LEFT = (1 << 0),
//         MU_MOUSE_RIGHT = (1 << 1),
//         MU_MOUSE_MIDDLE = (1 << 2)
//     };

//     enum MU_KEY
//     {
//         MU_KEY_SHIFT = (1 << 0),
//         MU_KEY_CTRL = (1 << 1),
//         MU_KEY_ALT = (1 << 2),
//         MU_KEY_BACKSPACE = (1 << 3),
//         MU_KEY_RETURN = (1 << 4)
//     };


//     //typedef unsigned int;
//     //typedef MU_REAL float;
//     //typedef void* mu_Font;

//     public struct mu_Vec2 { public int x, y; }
//     public struct mu_Rect { public int x, y, w, h; }
//     public struct mu_Color { public byte r, g, b, a; }
//     public struct mu_PoolItem { public int id; public int last_update; }

//     public class mu_Command
//     {
//         public int type;
//         public int size;
//         public int dst;
//         public mu_Rect rect;
//         public mu_Color color;
//         public object font;
//         public mu_Vec2 pos;
//         public string str;
//         public int id;
//     }

//     public class mu_Layout
//     {
//         public mu_Rect body;
//         public mu_Rect next;
//         public mu_Vec2 position;
//         public mu_Vec2 size;
//         public mu_Vec2 max;
//         public int[] widths = new int[MU_MAX_WIDTHS];
//         public int items;
//         public int item_index;
//         public int next_row;
//         public int next_type;
//         public int indent;
//     }

//     public class mu_Container
//     {
//         //public mu_Command head, tail;
//         public int head, tail;
//         public mu_Rect rect;
//         public mu_Rect body;
//         public mu_Vec2 content_size;
//         public mu_Vec2 scroll;
//         public int zindex;
//         public int open;
//     }

//     public class mu_Style
//     {
//         public object font;
//         public mu_Vec2 size;
//         public int padding;
//         public int spacing;
//         public int indent;
//         public int title_height;
//         public int scrollbar_size;
//         public int thumb_size;
//         public mu_Color[] colors = new mu_Color[(int)MU_COLOR.MU_COLOR_MAX];
//     }

//     public class mu_Context
//     {
//         /* callbacks */
//         public Func<object, string, int, int> text_width;
//         public Func<object, int> text_height;
//         public Action<mu_Context, mu_Rect, MU_COLOR> draw_frame;
//         /* core state */
//         public mu_Style _style;
//         public mu_Style style;
//         public int hover;
//         public int focus;
//         public int last_id;
//         public mu_Rect last_rect;
//         public int last_zindex;
//         public bool updated_focus;
//         public int frame;
//         public mu_Container hover_root;
//         public mu_Container next_hover_root;
//         public mu_Container scroll_target;
//         public byte[] number_edit_buf = new byte[MU_MAX_FMT];
//         public int number_edit;
//         /* stacks */
//         public List<mu_Command> command_list = new(MU_COMMANDLIST_SIZE);
//         public List<mu_Container> root_list = new(MU_ROOTLIST_SIZE);
//         public Stack<mu_Container> container_stack = new(MU_CONTAINERSTACK_SIZE);
//         public Stack<mu_Rect> clip_stack = new(MU_CLIPSTACK_SIZE);
//         public Stack<int> id_stack = new(intSTACK_SIZE);
//         public Stack<mu_Layout> layout_stack = new(MU_LAYOUTSTACK_SIZE);
//         /* retained state pools */
//         public mu_PoolItem[] container_pool = new mu_PoolItem[MU_CONTAINERPOOL_SIZE];
//         public mu_Container[] containers = new mu_Container[MU_CONTAINERPOOL_SIZE];
//         public mu_PoolItem[] treenode_pool = new mu_PoolItem[MU_TREENODEPOOL_SIZE];
//         /* input state */
//         public mu_Vec2 mouse_pos;
//         public mu_Vec2 last_mouse_pos;
//         public mu_Vec2 mouse_delta;
//         public mu_Vec2 scroll_delta;
//         public bool mouse_down;
//         public bool mouse_pressed;
//         public int key_down;
//         public int key_pressed;
//         public string input_text;
//     };


//     // mu_Vec2 mu_vec2(int x, int y);
//     // mu_Rect mu_rect(int x, int y, int w, int h);
//     // mu_Color mu_color(int r, int g, int b, int a);

//     // void mu_init(mu_Context ctx);
//     // void mu_begin(mu_Context ctx);
//     // void mu_end(mu_Context ctx);
//     // void mu_set_focus(mu_Context ctx, int id);
//     // int mu_get_id(mu_Context ctx, const void *data, int size);
//     // void mu_push_id(mu_Context ctx, const void *data, int size);
//     // void mu_pop_id(mu_Context ctx);
//     // void mu_push_clip_rect(mu_Context ctx, mu_Rect rect);
//     // void mu_pop_clip_rect(mu_Context ctx);
//     // mu_Rect mu_get_clip_rect(mu_Context ctx);
//     // int mu_check_clip(mu_Context ctx, mu_Rect r);
//     // mu_Container mu_get_current_container(mu_Context ctx);
//     // mu_Container mu_get_container(mu_Context ctx, string name);
//     // void mu_bring_to_front(mu_Context ctx, mu_Container *cnt);

//     // int mu_pool_init(mu_Context ctx, mu_PoolItem *items, int len, int id);
//     // int mu_pool_get(mu_Context ctx, mu_PoolItem *items, int len, int id);
//     // void mu_pool_update(mu_Context ctx, mu_PoolItem *items, int idx);

//     // void mu_input_mousemove(mu_Context ctx, int x, int y);
//     // void mu_input_mousedown(mu_Context ctx, int x, int y, int btn);
//     // void mu_input_mouseup(mu_Context ctx, int x, int y, int btn);
//     // void mu_input_scroll(mu_Context ctx, int x, int y);
//     // void mu_input_keydown(mu_Context ctx, int key);
//     // void mu_input_keyup(mu_Context ctx, int key);
//     // void mu_input_text(mu_Context ctx, string text);

//     // mu_Command* mu_push_command(mu_Context ctx, int type, int size);
//     // int mu_next_command(mu_Context ctx, mu_Command **cmd);
//     // void mu_set_clip(mu_Context ctx, mu_Rect rect);
//     // void mu_draw_rect(mu_Context ctx, mu_Rect rect, mu_Color color);
//     // void mu_draw_box(mu_Context ctx, mu_Rect rect, mu_Color color);
//     // void mu_draw_text(mu_Context ctx, mu_Font font, string str, int len, mu_Vec2 pos, mu_Color color);
//     // void mu_draw_icon(mu_Context ctx, int id, mu_Rect rect, mu_Color color);

//     // void mu_layout_row(mu_Context ctx, int items, const int *widths, int height);
//     // void mu_layout_width(mu_Context ctx, int width);
//     // void mu_layout_height(mu_Context ctx, int height);
//     // void mu_layout_begin_column(mu_Context ctx);
//     // void mu_layout_end_column(mu_Context ctx);
//     // void mu_layout_set_next(mu_Context ctx, mu_Rect r, int relative);
//     // mu_Rect mu_layout_next(mu_Context ctx);

//     // void mu_draw_control_frame(mu_Context ctx, int id, mu_Rect rect, int colorid, int opt);
//     // void mu_draw_control_text(mu_Context ctx, string str, mu_Rect rect, int colorid, int opt);
//     // int mu_mouse_over(mu_Context ctx, mu_Rect rect);
//     // void mu_update_control(mu_Context ctx, int id, mu_Rect rect, int opt);

//     // #define mu_button(ctx, label)             mu_button_ex(ctx, label, 0, MU_OPT_ALIGNCENTER)
//     // #define mu_textbox(ctx, buf, bufsz)       mu_textbox_ex(ctx, buf, bufsz, 0)
//     // #define mu_slider(ctx, value, lo, hi)     mu_slider_ex(ctx, value, lo, hi, 0, MU_SLIDER_FMT, MU_OPT_ALIGNCENTER)
//     // #define mu_number(ctx, value, step)       mu_number_ex(ctx, value, step, MU_SLIDER_FMT, MU_OPT_ALIGNCENTER)
//     // #define mu_header(ctx, label)             mu_header_ex(ctx, label, 0)
//     // #define mu_begin_treenode(ctx, label)     mu_begin_treenode_ex(ctx, label, 0)
//     // #define mu_begin_window(ctx, title, rect) mu_begin_window_ex(ctx, title, rect, 0)
//     // #define mu_begin_panel(ctx, name)         mu_begin_panel_ex(ctx, name, 0)

//     // void mu_text(mu_Context ctx, string text);
//     // void mu_label(mu_Context ctx, string text);
//     // int mu_button_ex(mu_Context ctx, string label, int icon, int opt);
//     // int mu_checkbox(mu_Context ctx, string label, int *state);
//     // int mu_textbox_raw(mu_Context ctx, char *buf, int bufsz, int id, mu_Rect r, int opt);
//     // int mu_textbox_ex(mu_Context ctx, char *buf, int bufsz, int opt);
//     // int mu_slider_ex(mu_Context ctx, float *value, float low, float high, float step, string fmt, int opt);
//     // int mu_number_ex(mu_Context ctx, float *value, float step, string fmt, int opt);
//     // int mu_header_ex(mu_Context ctx, string label, int opt);
//     // int mu_begin_treenode_ex(mu_Context ctx, string label, int opt);
//     // void mu_end_treenode(mu_Context ctx);
//     // int mu_begin_window_ex(mu_Context ctx, string title, mu_Rect rect, int opt);
//     // void mu_end_window(mu_Context ctx);
//     // void mu_open_popup(mu_Context ctx, string name);
//     // int mu_begin_popup(mu_Context ctx, string name);
//     // void mu_end_popup(mu_Context ctx);
//     // void mu_begin_panel_ex(mu_Context ctx, string name, int opt);
//     // void mu_end_panel(mu_Context ctx);

//     // IMPLEMENTATION

//     /*
// ** Copyright (c) 2020 rxi
// **
// ** Permission is hereby granted, free of charge, to any person obtaining a copy
// ** of this software and associated documentation files (the "Software"), to
// ** deal in the Software without restriction, including without limitation the
// ** rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// ** sell copies of the Software, and to permit persons to whom the Software is
// ** furnished to do so, subject to the following conditions:
// **
// ** The above copyright notice and this permission notice shall be included in
// ** all copies or substantial portions of the Software.
// **
// ** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// ** IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// ** FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// ** AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// ** LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// ** FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// ** IN THE SOFTWARE.
// */


//     //#define unused(x) ((void) (x))

//     // #define expect(x) do {                                               \
//     //     if (!(x)) {                                                      \
//     //       fprintf(stderr, "Fatal error: %s:%d: assertion '%s' failed\n", \
//     //         __FILE__, __LINE__, #x);                                     \
//     //       abort();                                                       \
//     //     }                                                                \
//     //   } while (0)

//     // #define push(stk, val) do {                                                 \
//     //     expect((stk).idx < (int) (sizeof((stk).items) / sizeof(*(stk).items))); \
//     //     (stk).items[(stk).idx] = (val);                                         \
//     //     (stk).idx++; /* incremented after incase `val` uses this value */       \
//     //   } while (0)

//     // #define pop(stk) do {      \
//     //     expect((stk).idx > 0); \
//     //     (stk).idx--;           \
//     //   } while (0)


//     static readonly mu_Rect unclipped_rect = new mu_Rect { x = 0, y = 0, w = 0x1000000, h = 0x1000000 };

//     static readonly mu_Style default_style = new mu_Style
//     {
//         /* font | size | padding | spacing | indent */
//         font = null,
//         size = new mu_Vec2 { x = 68, y = 10 },
//         padding = 5,
//         spacing = 4,
//         indent = 24,
//         /* title_height | scrollbar_size | thumb_size */
//         title_height = 24,
//         scrollbar_size = 12,
//         thumb_size = 8,
//         colors = new mu_Color[]
//       {
//     new mu_Color { r = 230, g = 230, b = 230 }, /* MU_COLOR.MU_COLOR_TEXT */
//     new mu_Color { r = 25, g = 25, b = 25 }, /* MU_COLOR.MU_COLOR_BORDER */
//     new mu_Color { r = 50, g = 50, b = 50 }, /* MU_COLOR.MU_COLOR_WINDOWBG */
//     new mu_Color { r = 25, g = 25, b = 25 }, /* MU_COLOR.MU_COLOR_TITLEBG */
//     new mu_Color { r = 240, g = 240, b = 240 }, /* MU_COLOR.MU_COLOR_TITLETEXT */
//     new mu_Color { r = 0, g = 0, b = 0 }, /* MU_COLOR.MU_COLOR_PANELBG */
//     new mu_Color { r = 75, g = 75, b = 75 }, /* MU_COLOR.MU_COLOR_BUTTON */
//     new mu_Color { r = 95, g = 95, b = 95 }, /* MU_COLOR.MU_COLOR_BUTTONHOVER */
//     new mu_Color { r = 115, g = 115, b = 115 }, /* MU_COLOR.MU_COLOR_BUTTONFOCUS */
//     new mu_Color { r = 30, g = 30, b = 30 }, /* MU_COLOR.MU_COLOR_BASE */
//     new mu_Color { r = 35, g = 35, b = 35 }, /* MU_COLOR.MU_COLOR_BASEHOVER */
//     new mu_Color { r = 40, g = 40, b = 40 }, /* MU_COLOR.MU_COLOR_BASEFOCUS */
//     new mu_Color { r = 43, g = 43, b = 43 }, /* MU_COLOR.MU_COLOR_SCROLLBASE */
//     new mu_Color { r = 30, g = 30, b = 30 }  /* MU_COLOR.MU_COLOR_SCROLLTHUMB */
//       }
//     };


//     static mu_Vec2 mu_vec2(int x, int y)
//     {
//         mu_Vec2 res;
//         res.x = x; res.y = y;
//         return res;
//     }


//     static mu_Rect mu_rect(int x, int y, int w, int h)
//     {
//         mu_Rect res;
//         res.x = x; res.y = y; res.w = w; res.h = h;
//         return res;
//     }


//     static mu_Color mu_color(byte r, byte g, byte b, byte a)
//     {
//         mu_Color res;
//         res.r = r; res.g = g; res.b = b; res.a = a;
//         return res;
//     }


//     static mu_Rect expand_rect(mu_Rect rect, int n)
//     {
//         return mu_rect(rect.x - n, rect.y - n, rect.w + n * 2, rect.h + n * 2);
//     }


//     static mu_Rect intersect_rects(mu_Rect r1, mu_Rect r2)
//     {
//         int x1 = Math.Max(r1.x, r2.x);
//         int y1 = Math.Max(r1.y, r2.y);
//         int x2 = Math.Min(r1.x + r1.w, r2.x + r2.w);
//         int y2 = Math.Min(r1.y + r1.h, r2.y + r2.h);
//         if (x2 < x1) { x2 = x1; }
//         if (y2 < y1) { y2 = y1; }
//         return mu_rect(x1, y1, x2 - x1, y2 - y1);
//     }


//     static bool rect_overlaps_vec2(mu_Rect r, mu_Vec2 p)
//     {
//         return p.x >= r.x && p.x < r.x + r.w && p.y >= r.y && p.y < r.y + r.h;
//     }


//     static void draw_frame(mu_Context ctx, mu_Rect rect, MU_COLOR colorid)
//     {
//         mu_draw_rect(ctx, rect, ctx.style.colors[(int)colorid]);
//         if (colorid == MU_COLOR.MU_COLOR_SCROLLBASE ||
//             colorid == MU_COLOR.MU_COLOR_SCROLLTHUMB ||
//             colorid == MU_COLOR.MU_COLOR_TITLEBG) { return; }
//         /* draw border */
//         if (ctx.style.colors[(int)MU_COLOR.MU_COLOR_BORDER].a > 0)
//         {
//             mu_draw_box(ctx, expand_rect(rect, 1), ctx.style.colors[(int)MU_COLOR.MU_COLOR_BORDER]);
//         }
//     }


//     static void mu_init(mu_Context ctx)
//     {
//         ctx.draw_frame = draw_frame;
//         ctx._style = default_style;
//         ctx.style = ctx._style;
//     }

//     static void expect(bool c, string msg) { if (!c) throw new InvalidOperationException(msg); }

//     static void mu_begin(mu_Context ctx)
//     {
//         expect(ctx.text_width != null && ctx.text_height != null, "No text measurement functions found");
//         //ctx.command_list.idx = 0;
//         //ctx.root_list.idx = 0;
//         ctx.scroll_target = null;
//         ctx.hover_root = ctx.next_hover_root;
//         ctx.next_hover_root = null;
//         ctx.mouse_delta.x = ctx.mouse_pos.x - ctx.last_mouse_pos.x;
//         ctx.mouse_delta.y = ctx.mouse_pos.y - ctx.last_mouse_pos.y;
//         ctx.frame++;
//     }


//     // static int compare_zindex(const void *a, const void *b) {
//     //   return (*(mu_Container*) a).zindex - (*(mu_Container*) b).zindex;
//     // }


//     static void mu_end(mu_Context ctx)
//     {
//         int i, n;
//         /* check stacks */
//         expect(ctx.container_stack.Count == 0, "Container stack not empty");
//         expect(ctx.clip_stack.Count == 0, "Clip stack not empty");
//         expect(ctx.id_stack.Count == 0, "Id stack not empty");
//         expect(ctx.layout_stack.Count == 0, "Layout stack not empty");

//         /* handle scroll input */
//         if (ctx.scroll_target != null)
//         {
//             ctx.scroll_target.scroll.x += ctx.scroll_delta.x;
//             ctx.scroll_target.scroll.y += ctx.scroll_delta.y;
//         }

//         /* unset focus if focus id was not touched this frame */
//         if (!ctx.updated_focus) { ctx.focus = 0; }
//         ctx.updated_focus = false;

//         /* bring hover root to front if mouse was pressed */
//         if (ctx.mouse_pressed && ctx.next_hover_root != null &&
//             ctx.next_hover_root.zindex < ctx.last_zindex &&
//             ctx.next_hover_root.zindex >= 0
//         )
//         {
//             mu_bring_to_front(ctx, ctx.next_hover_root);
//         }

//         /* reset input state */
//         ctx.key_pressed = 0;
//         ctx.input_text = string.Empty;
//         ctx.mouse_pressed = false;
//         ctx.scroll_delta = mu_vec2(0, 0);
//         ctx.last_mouse_pos = ctx.mouse_pos;

//         /* sort root containers by zindex */
//         ctx.root_list.Sort((a, b) => a.zindex.CompareTo(b.zindex));
//         /* set root container jump commands */
//         for (i = 0; i < ctx.root_list.Count; i++)
//         {
//             mu_Container cnt = ctx.root_list[i];
//             /* if this is the first container then make the first command jump to it.
//             ** otherwise set the previous container's tail to jump to this one */
//             if (i == 0)
//             {
//                 mu_Command cmd = ctx.command_list.First();

//                 cmd.dst = cnt.head + 1; // (char*) cnt.head + sizeof(mu_JumpCommand);
//             }
//             else
//             {
//                 mu_Container prev = ctx.root_list[i - 1];
//                 ctx.command_list[prev.tail].dst = cnt.head + 1; // (char*) cnt.head + sizeof(mu_JumpCommand);
//             }
//             /* make the last container's tail jump to the end of command list */
//             if (i == ctx.root_list.Count - 1)
//             {
//                 ctx.command_list[cnt.tail].dst = ctx.command_list.Count;//ctx.command_list.items + ctx.command_list.idx;
//             }
//         }
//     }


//     static void mu_set_focus(mu_Context ctx, int id)
//     {
//         ctx.focus = id;
//         ctx.updated_focus = true;
//     }


//     // /* 32bit fnv-1a hash */
//     // #define HASH_INITIAL 2166136261

//     // static void hash(int *hash, const void *data, int size) {
//     //   const unsigned char *p = data;
//     //   while (size--) {
//     //     *hash = (*hash ^ *p++) * 16777619;
//     //   }
//     // }


//     static int mu_get_id(mu_Context ctx, object data, int size)
//     {
//         int idx = ctx.id_stack.idx;
//         int res = (idx > 0) ? ctx.id_stack.items[idx - 1] : HASH_INITIAL;
//         hash(&res, data, size);
//         ctx.last_id = res;
//         return res;
//     }


//     static void mu_push_id(mu_Context ctx, object data, int size)
//     {
//         push(ctx.id_stack, mu_get_id(ctx, data, size));
//     }


//     static void mu_pop_id(mu_Context ctx)
//     {
//         pop(ctx.id_stack);
//     }


//     static void mu_push_clip_rect(mu_Context ctx, mu_Rect rect)
//     {
//         mu_Rect last = mu_get_clip_rect(ctx);
//         push(ctx.clip_stack, intersect_rects(rect, last));
//     }


//     static void mu_pop_clip_rect(mu_Context ctx)
//     {
//         pop(ctx.clip_stack);
//     }


//     static mu_Rect mu_get_clip_rect(mu_Context ctx)
//     {
//         expect(ctx.clip_stack.idx > 0);
//         return ctx.clip_stack.items[ctx.clip_stack.idx - 1];
//     }


//     static int mu_check_clip(mu_Context ctx, mu_Rect r)
//     {
//         mu_Rect cr = mu_get_clip_rect(ctx);
//         if (r.x > cr.x + cr.w || r.x + r.w < cr.x ||
//             r.y > cr.y + cr.h || r.y + r.h < cr.y) { return MU_CLIP_ALL; }
//         if (r.x >= cr.x && r.x + r.w <= cr.x + cr.w &&
//             r.y >= cr.y && r.y + r.h <= cr.y + cr.h) { return 0; }
//         return MU_CLIP_PART;
//     }


//     static void push_layout(mu_Context ctx, mu_Rect body, mu_Vec2 scroll)
//     {
//         mu_Layout layout;
//         int width = 0;
//         memset(&layout, 0, sizeof(layout));
//         layout.body = mu_rect(body.x - scroll.x, body.y - scroll.y, body.w, body.h);
//         layout.max = mu_vec2(-0x1000000, -0x1000000);
//         push(ctx.layout_stack, layout);
//         mu_layout_row(ctx, 1, &width, 0);
//     }


//     static mu_Layout get_layout(mu_Context ctx)
//     {
//         return &ctx.layout_stack.items[ctx.layout_stack.idx - 1];
//     }


//     static void pop_container(mu_Context ctx)
//     {
//         mu_Container* cnt = mu_get_current_container(ctx);
//         mu_Layout* layout = get_layout(ctx);
//         cnt.content_size.x = layout.max.x - layout.body.x;
//         cnt.content_size.y = layout.max.y - layout.body.y;
//         /* pop container, layout and id */
//         pop(ctx.container_stack);
//         pop(ctx.layout_stack);
//         mu_pop_id(ctx);
//     }


//     static mu_Container mu_get_current_container(mu_Context ctx)
//     {
//         expect(ctx.container_stack.idx > 0);
//         return ctx.container_stack.items[ctx.container_stack.idx - 1];
//     }


//     static mu_Container get_container(mu_Context ctx, int id, int opt)
//     {
//         mu_Container* cnt;
//         /* try to get existing container from pool */
//         int idx = mu_pool_get(ctx, ctx.container_pool, MU_CONTAINERPOOL_SIZE, id);
//         if (idx >= 0)
//         {
//             if (ctx.containers[idx].open || ~opt & MU_OPT_CLOSED)
//             {
//                 mu_pool_update(ctx, ctx.container_pool, idx);
//             }
//             return &ctx.containers[idx];
//         }
//         if (opt & MU_OPT_CLOSED) { return null; }
//         /* container not found in pool: init new container */
//         idx = mu_pool_init(ctx, ctx.container_pool, MU_CONTAINERPOOL_SIZE, id);
//         cnt = &ctx.containers[idx];
//         //memset(cnt, 0, sizeof(*cnt));
//         cnt.open = 1;
//         mu_bring_to_front(ctx, cnt);
//         return cnt;
//     }


//     static mu_Container mu_get_container(mu_Context ctx, string name)
//     {
//         int id = mu_get_id(ctx, name, name.length);
//         return get_container(ctx, id, 0);
//     }


//     static void mu_bring_to_front(mu_Context ctx, mu_Container cnt)
//     {
//         cnt.zindex = ++ctx.last_zindex;
//     }


//     /*============================================================================
//     ** pool
//     **============================================================================*/

//     static int mu_pool_init(mu_Context ctx, mu_PoolItem[] items, int len, int id)
//     {
//         int i, n = -1, f = ctx.frame;
//         for (i = 0; i < len; i++)
//         {
//             if (items[i].last_update < f)
//             {
//                 f = items[i].last_update;
//                 n = i;
//             }
//         }
//         expect(n > -1);
//         items[n].id = id;
//         mu_pool_update(ctx, items, n);
//         return n;
//     }


//     static int mu_pool_get(mu_Context ctx, mu_PoolItem[] items, int len, int id)
//     {
//         int i;
//         unused(ctx);
//         for (i = 0; i < len; i++)
//         {
//             if (items[i].id == id) { return i; }
//         }
//         return -1;
//     }


//     static void mu_pool_update(mu_Context ctx, mu_PoolItem[] items, int idx)
//     {
//         items[idx].last_update = ctx.frame;
//     }


//     /*============================================================================
//     ** input handlers
//     **============================================================================*/

//     static void mu_input_mousemove(mu_Context ctx, int x, int y)
//     {
//         ctx.mouse_pos = mu_vec2(x, y);
//     }


//     static void mu_input_mousedown(mu_Context ctx, int x, int y, int btn)
//     {
//         mu_input_mousemove(ctx, x, y);
//         ctx.mouse_down |= btn;
//         ctx.mouse_pressed |= btn;
//     }


//     static void mu_input_mouseup(mu_Context ctx, int x, int y, int btn)
//     {
//         mu_input_mousemove(ctx, x, y);
//         ctx.mouse_down &= ~btn;
//     }


//     static void mu_input_scroll(mu_Context ctx, int x, int y)
//     {
//         ctx.scroll_delta.x += x;
//         ctx.scroll_delta.y += y;
//     }


//     static void mu_input_keydown(mu_Context ctx, int key)
//     {
//         ctx.key_pressed |= key;
//         ctx.key_down |= key;
//     }


//     static void mu_input_keyup(mu_Context ctx, int key)
//     {
//         ctx.key_down &= ~key;
//     }


//     static void mu_input_text(mu_Context ctx, string text)
//     {
//         int len = ctx.input_text.length;
//         int size = text.length + 1;
//         expect(len + size <= (int)sizeof(ctx.input_text));
//         memcpy(ctx.input_text + len, text, size);
//     }


//     /*============================================================================
//     ** commandlist
//     **============================================================================*/

//     static mu_Command mu_push_command(mu_Context ctx, int type, int size)
//     {
//         mu_Command* cmd = (mu_Command*)(ctx.command_list.items + ctx.command_list.idx);
//         expect(ctx.command_list.idx + size < MU_COMMANDLIST_SIZE);
//         cmd.type = type;
//         cmd.size = size;
//         ctx.command_list.idx += size;
//         return cmd;
//     }


//     static int mu_next_command(mu_Context ctx, mu_Command[][] cmd)
//     {
//         if (*cmd)
//         {
//             *cmd = (mu_Command*)(((char*)*cmd) + (*cmd).base.size);
//         }
//         else
//         {
//             *cmd = (mu_Command*)ctx.command_list.items;
//         }
//         while ((char*)*cmd != ctx.command_list.items + ctx.command_list.idx)
//         {
//             if ((*cmd).type != MU_COMMAND_JUMP) { return 1; }
//             *cmd = (*cmd).jump.dst;
//         }
//         return 0;
//     }


//     static mu_Command push_jump(mu_Context ctx, mu_Command dst)
//     {
//         mu_Command cmd;
//         cmd = mu_push_command(ctx, MU_COMMAND_JUMP, sizeof(mu_JumpCommand));
//         cmd.dst = dst;
//         return cmd;
//     }


//     static void mu_set_clip(mu_Context ctx, mu_Rect rect)
//     {
//         mu_Command cmd;
//         cmd = mu_push_command(ctx, MU_COMMAND_CLIP, sizeof(mu_ClipCommand));
//         cmd.clip.rect = rect;
//     }


//     static void mu_draw_rect(mu_Context ctx, mu_Rect rect, mu_Color color)
//     {
//         mu_Command* cmd;
//         rect = intersect_rects(rect, mu_get_clip_rect(ctx));
//         if (rect.w > 0 && rect.h > 0)
//         {
//             cmd = mu_push_command(ctx, MU_COMMAND_RECT, sizeof(mu_RectCommand));
//             cmd.rect.rect = rect;
//             cmd.rect.color = color;
//         }
//     }


//     static void mu_draw_box(mu_Context ctx, mu_Rect rect, mu_Color color)
//     {
//         mu_draw_rect(ctx, mu_rect(rect.x + 1, rect.y, rect.w - 2, 1), color);
//         mu_draw_rect(ctx, mu_rect(rect.x + 1, rect.y + rect.h - 1, rect.w - 2, 1), color);
//         mu_draw_rect(ctx, mu_rect(rect.x, rect.y, 1, rect.h), color);
//         mu_draw_rect(ctx, mu_rect(rect.x + rect.w - 1, rect.y, 1, rect.h), color);
//     }


//     static void mu_draw_text(mu_Context ctx, object font, string str, int len,
//       mu_Vec2 pos, mu_Color color)
//     {
//         mu_Command* cmd;
//         mu_Rect rect = mu_rect(
//           pos.x, pos.y, ctx.text_width(font, str, len), ctx.text_height(font));
//         int clipped = mu_check_clip(ctx, rect);
//         if (clipped == MU_CLIP_ALL) { return; }
//         if (clipped == MU_CLIP_PART) { mu_set_clip(ctx, mu_get_clip_rect(ctx)); }
//         /* add command */
//         if (len < 0) { len = str.length; }
//         cmd = mu_push_command(ctx, MU_COMMAND_TEXT, sizeof(mu_TextCommand) + len);
//         memcpy(cmd.text.str, str, len);
//         cmd.text.str[len] = '\0';
//         cmd.text.pos = pos;
//         cmd.text.color = color;
//         cmd.text.font = font;
//         /* reset clipping if it was set */
//         if (clipped) { mu_set_clip(ctx, unclipped_rect); }
//     }


//     static void mu_draw_icon(mu_Context ctx, int id, mu_Rect rect, mu_Color color)
//     {
//         mu_Command* cmd;
//         /* do clip command if the rect isn't fully contained within the cliprect */
//         int clipped = mu_check_clip(ctx, rect);
//         if (clipped == MU_CLIP_ALL) { return; }
//         if (clipped == MU_CLIP_PART) { mu_set_clip(ctx, mu_get_clip_rect(ctx)); }
//         /* do icon command */
//         cmd = mu_push_command(ctx, MU_COMMAND_ICON, sizeof(mu_IconCommand));
//         cmd.icon.id = id;
//         cmd.icon.rect = rect;
//         cmd.icon.color = color;
//         /* reset clipping if it was set */
//         if (clipped) { mu_set_clip(ctx, unclipped_rect); }
//     }


//     /*============================================================================
//     ** layout
//     **============================================================================*/

//     enum POS { RELATIVE = 1, ABSOLUTE = 2 };


//     static void mu_layout_begin_column(mu_Context ctx)
//     {
//         push_layout(ctx, mu_layout_next(ctx), mu_vec2(0, 0));
//     }


//     static void mu_layout_end_column(mu_Context ctx)
//     {
//         mu_Layout a, b;
//         b = get_layout(ctx);
//         pop(ctx.layout_stack);
//         /* inherit position/next_row/max from child layout if they are greater */
//         a = get_layout(ctx);
//         a.position.x = Math.Max(a.position.x, b.position.x + b.body.x - a.body.x);
//         a.next_row = Math.Max(a.next_row, b.next_row + b.body.y - a.body.y);
//         a.max.x = Math.Max(a.max.x, b.max.x);
//         a.max.y = Math.Max(a.max.y, b.max.y);
//     }


//     static void mu_layout_row(mu_Context ctx, int items, int[] widths, int height)
//     {
//         mu_Layout* layout = get_layout(ctx);
//         if (widths)
//         {
//             expect(items <= MU_MAX_WIDTHS);
//             memcpy(layout.widths, widths, items * sizeof(widths[0]));
//         }
//         layout.items = items;
//         layout.position = mu_vec2(layout.indent, layout.next_row);
//         layout.size.y = height;
//         layout.item_index = 0;
//     }


//     static void mu_layout_width(mu_Context ctx, int width)
//     {
//         get_layout(ctx).size.x = width;
//     }


//     static void mu_layout_height(mu_Context ctx, int height)
//     {
//         get_layout(ctx).size.y = height;
//     }


//     static void mu_layout_set_next(mu_Context ctx, mu_Rect r, int relative)
//     {
//         mu_Layout* layout = get_layout(ctx);
//         layout.next = r;
//         layout.next_type = relative ? RELATIVE : ABSOLUTE;
//     }


//     static mu_Rect mu_layout_next(mu_Context ctx)
//     {
//         mu_Layout* layout = get_layout(ctx);
//         mu_Style* style = ctx.style;
//         mu_Rect res;

//         if (layout.next_type)
//         {
//             /* handle rect set by `mu_layout_set_next` */
//             int type = layout.next_type;
//             layout.next_type = 0;
//             res = layout.next;
//             if (type == ABSOLUTE) { return (ctx.last_rect = res); }

//         }
//         else
//         {
//             /* handle next row */
//             if (layout.item_index == layout.items)
//             {
//                 mu_layout_row(ctx, layout.items, null, layout.size.y);
//             }

//             /* position */
//             res.x = layout.position.x;
//             res.y = layout.position.y;

//             /* size */
//             res.w = layout.items > 0 ? layout.widths[layout.item_index] : layout.size.x;
//             res.h = layout.size.y;
//             if (res.w == 0) { res.w = style.size.x + style.padding * 2; }
//             if (res.h == 0) { res.h = style.size.y + style.padding * 2; }
//             if (res.w < 0) { res.w += layout.body.w - res.x + 1; }
//             if (res.h < 0) { res.h += layout.body.h - res.y + 1; }

//             layout.item_index++;
//         }

//         /* update position */
//         layout.position.x += res.w + style.spacing;
//         layout.next_row = Math.Max(layout.next_row, res.y + res.h + style.spacing);

//         /* apply body offset */
//         res.x += layout.body.x;
//         res.y += layout.body.y;

//         /* update max position */
//         layout.max.x = Math.Max(layout.max.x, res.x + res.w);
//         layout.max.y = Math.Max(layout.max.y, res.y + res.h);

//         return (ctx.last_rect = res);
//     }


//     /*============================================================================
//     ** controls
//     **============================================================================*/

//     static int in_hover_root(mu_Context ctx)
//     {
//         int i = ctx.container_stack.idx;
//         while (i--)
//         {
//             if (ctx.container_stack.items[i] == ctx.hover_root) { return 1; }
//             /* only root containers have their `head` field set; stop searching if we've
//             ** reached the current root container */
//             if (ctx.container_stack.items[i].head) { break; }
//         }
//         return 0;
//     }


//     static void mu_draw_control_frame(mu_Context ctx, int id, mu_Rect rect,
//       int colorid, int opt)
//     {
//         if (opt & MU_OPT_NOFRAME) { return; }
//         colorid += (ctx.focus == id) ? 2 : (ctx.hover == id) ? 1 : 0;
//         ctx.draw_frame(ctx, rect, colorid);
//     }


//     static void mu_draw_control_text(mu_Context ctx, string str, mu_Rect rect,
//       int colorid, int opt)
//     {
//         mu_Vec2 pos;
//         mu_Font font = ctx.style.font;
//         int tw = ctx.text_width(font, str, -1);
//         mu_push_clip_rect(ctx, rect);
//         pos.y = rect.y + (rect.h - ctx.text_height(font)) / 2;
//         if (opt & MU_OPT_ALIGNCENTER)
//         {
//             pos.x = rect.x + (rect.w - tw) / 2;
//         }
//         else if (opt & MU_OPT_ALIGNRIGHT)
//         {
//             pos.x = rect.x + rect.w - tw - ctx.style.padding;
//         }
//         else
//         {
//             pos.x = rect.x + ctx.style.padding;
//         }
//         mu_draw_text(ctx, font, str, -1, pos, ctx.style.colors[colorid]);
//         mu_pop_clip_rect(ctx);
//     }


//     static int mu_mouse_over(mu_Context ctx, mu_Rect rect)
//     {
//         return rect_overlaps_vec2(rect, ctx.mouse_pos) &&
//           rect_overlaps_vec2(mu_get_clip_rect(ctx), ctx.mouse_pos) &&
//           in_hover_root(ctx);
//     }


//     static void mu_update_control(mu_Context ctx, int id, mu_Rect rect, int opt)
//     {
//         int mouseover = mu_mouse_over(ctx, rect);

//         if (ctx.focus == id) { ctx.updated_focus = 1; }
//         if (opt & MU_OPT_NOINTERACT) { return; }
//         if (mouseover && !ctx.mouse_down) { ctx.hover = id; }

//         if (ctx.focus == id)
//         {
//             if (ctx.mouse_pressed && !mouseover) { mu_set_focus(ctx, 0); }
//             if (!ctx.mouse_down && ~opt & MU_OPT_HOLDFOCUS) { mu_set_focus(ctx, 0); }
//         }

//         if (ctx.hover == id)
//         {
//             if (ctx.mouse_pressed)
//             {
//                 mu_set_focus(ctx, id);
//             }
//             else if (!mouseover)
//             {
//                 ctx.hover = 0;
//             }
//         }
//     }


//     static void mu_text(mu_Context ctx, string text)
//     {
//         string start, end, p = text;
//         int width = -1;
//         mu_Font font = ctx.style.font;
//         mu_Color color = ctx.style.colors[MU_COLOR.MU_COLOR_TEXT];
//         mu_layout_begin_column(ctx);
//         mu_layout_row(ctx, 1, &width, ctx.text_height(font));
//         do
//         {
//             mu_Rect r = mu_layout_next(ctx);
//             int w = 0;
//             start = end = p;
//             do
//             {
//                 const char* word = p;
//                 while (*p && *p != ' ' && *p != '\n') { p++; }
//                 w += ctx.text_width(font, word, p - word);
//                 if (w > r.w && end != start) { break; }
//                 w += ctx.text_width(font, p, 1);
//                 end = p++;
//             } while (*end && *end != '\n');
//             mu_draw_text(ctx, font, start, end - start, mu_vec2(r.x, r.y), color);
//             p = end + 1;
//         } while (*end);
//         mu_layout_end_column(ctx);
//     }


//     static void mu_label(mu_Context ctx, string text)
//     {
//         mu_draw_control_text(ctx, text, mu_layout_next(ctx), MU_COLOR.MU_COLOR_TEXT, 0);
//     }


//     static int mu_button_ex(mu_Context ctx, string label, int icon, int opt)
//     {
//         int res = 0;
//         int id = label ? mu_get_id(ctx, label, label.length)
//                          : mu_get_id(ctx, &icon, sizeof(icon));
//         mu_Rect r = mu_layout_next(ctx);
//         mu_update_control(ctx, id, r, opt);
//         /* handle click */
//         if (ctx.mouse_pressed == MU_MOUSE_LEFT && ctx.focus == id)
//         {
//             res |= MU_RES_SUBMIT;
//         }
//         /* draw */
//         mu_draw_control_frame(ctx, id, r, MU_COLOR.MU_COLOR_BUTTON, opt);
//         if (label) { mu_draw_control_text(ctx, label, r, MU_COLOR.MU_COLOR_TEXT, opt); }
//         if (icon) { mu_draw_icon(ctx, icon, r, ctx.style.colors[MU_COLOR.MU_COLOR_TEXT]); }
//         return res;
//     }


//     static int mu_checkbox(mu_Context ctx, string label, ref int state)
//     {
//         int res = 0;
//         int id = mu_get_id(ctx, &state, sizeof(state));
//         mu_Rect r = mu_layout_next(ctx);
//         mu_Rect box = mu_rect(r.x, r.y, r.h, r.h);
//         mu_update_control(ctx, id, r, 0);
//         /* handle click */
//         if (ctx.mouse_pressed == MU_MOUSE_LEFT && ctx.focus == id)
//         {
//             res |= MU_RES_CHANGE;
//             *state = !*state;
//         }
//         /* draw */
//         mu_draw_control_frame(ctx, id, box, MU_COLOR.MU_COLOR_BASE, 0);
//         if (*state)
//         {
//             mu_draw_icon(ctx, MU_ICON_CHECK, box, ctx.style.colors[MU_COLOR.MU_COLOR_TEXT]);
//         }
//         r = mu_rect(r.x + box.w, r.y, r.w - box.w, r.h);
//         mu_draw_control_text(ctx, label, r, MU_COLOR.MU_COLOR_TEXT, 0);
//         return res;
//     }


//     static int mu_textbox_raw(mu_Context ctx, string buf, int bufsz, int id, mu_Rect r,
//       int opt)
//     {
//         int res = 0;
//         mu_update_control(ctx, id, r, opt | MU_OPT_HOLDFOCUS);

//         if (ctx.focus == id)
//         {
//             /* handle text input */
//             int len = buf.length;
//             int n = Math.Min(bufsz - len - 1, (int)ctx.input_text.length);
//             if (n > 0)
//             {
//                 memcpy(buf + len, ctx.input_text, n);
//                 len += n;
//                 buf[len] = '\0';
//                 res |= MU_RES_CHANGE;
//             }
//             /* handle backspace */
//             if (ctx.key_pressed & MU_KEY_BACKSPACE && len > 0)
//             {
//                 /* skip utf-8 continuation bytes */
//                 while ((buf[--len] & 0xc0) == 0x80 && len > 0) ;
//                 buf[len] = '\0';
//                 res |= MU_RES_CHANGE;
//             }
//             /* handle return */
//             if (ctx.key_pressed & MU_KEY_RETURN)
//             {
//                 mu_set_focus(ctx, 0);
//                 res |= MU_RES_SUBMIT;
//             }
//         }

//         /* draw */
//         mu_draw_control_frame(ctx, id, r, MU_COLOR.MU_COLOR_BASE, opt);
//         if (ctx.focus == id)
//         {
//             mu_Color color = ctx.style.colors[MU_COLOR.MU_COLOR_TEXT];
//             mu_Font font = ctx.style.font;
//             int textw = ctx.text_width(font, buf, -1);
//             int texth = ctx.text_height(font);
//             int ofx = r.w - ctx.style.padding - textw - 1;
//             int textx = r.x + Math.Min(ofx, ctx.style.padding);
//             int texty = r.y + (r.h - texth) / 2;
//             mu_push_clip_rect(ctx, r);
//             mu_draw_text(ctx, font, buf, -1, mu_vec2(textx, texty), color);
//             mu_draw_rect(ctx, mu_rect(textx + textw, texty, 1, texth), color);
//             mu_pop_clip_rect(ctx);
//         }
//         else
//         {
//             mu_draw_control_text(ctx, buf, r, MU_COLOR.MU_COLOR_TEXT, opt);
//         }

//         return res;
//     }


//     static int number_textbox(mu_Context ctx, ref float value, mu_Rect r, int id)
//     {
//         if (ctx.mouse_pressed == MU_MOUSE_LEFT && ctx.key_down & MU_KEY_SHIFT &&
//             ctx.hover == id
//         )
//         {
//             ctx.number_edit = id;
//             sprintf(ctx.number_edit_buf, MU_REAL_FMT, *value);
//         }
//         if (ctx.number_edit == id)
//         {
//             int res = mu_textbox_raw(
//               ctx, ctx.number_edit_buf, sizeof(ctx.number_edit_buf), id, r, 0);
//             if (res & MU_RES_SUBMIT || ctx.focus != id)
//             {
//                 *value = strtod(ctx.number_edit_buf, null);
//                 ctx.number_edit = 0;
//             }
//             else
//             {
//                 return 1;
//             }
//         }
//         return 0;
//     }


//     static int mu_textbox_ex(mu_Context ctx, string buf, int bufsz, int opt)
//     {
//         int id = mu_get_id(ctx, &buf, sizeof(buf));
//         mu_Rect r = mu_layout_next(ctx);
//         return mu_textbox_raw(ctx, buf, bufsz, id, r, opt);
//     }


//     static int mu_slider_ex(mu_Context ctx, ref float value, float low, float high,
//       float step, string fmt, int opt)
//     {
//         char buf[MU_MAX_FMT + 1];
//         mu_Rect thumb;
//         int x, w, res = 0;
//         float last = *value, v = last;
//         int id = mu_get_id(ctx, &value, sizeof(value));
//         mu_Rect _base = mu_layout_next(ctx);

//         /* handle text input mode */
//         if (number_textbox(ctx, &v, _base, id)) { return res; }

//         /* handle normal mode */
//         mu_update_control(ctx, id, _base, opt);

//         /* handle input */
//         if (ctx.focus == id &&
//             (ctx.mouse_down | ctx.mouse_pressed) == MU_MOUSE_LEFT)
//         {
//             v = low + (ctx.mouse_pos.x - _base.x) * (high - low) / _base.w;
//             if (step) { v = (((v + step / 2) / step)) * step; }
//         }
//         /* clamp and store value, update res */
//         *value = v = mu_clamp(v, low, high);
//         if (last != v) { res |= MU_RES_CHANGE; }

//         /* draw base */
//         mu_draw_control_frame(ctx, id, _base, MU_COLOR.MU_COLOR_BASE, opt);
//         /* draw thumb */
//         w = ctx.style.thumb_size;
//         x = (v - low) * (_base.w - w) / (high - low);
//         thumb = mu_rect(_base.x + x, _base.y, w, _base.h);
//         mu_draw_control_frame(ctx, id, thumb, MU_COLOR.MU_COLOR_BUTTON, opt);
//         /* draw text  */
//         sprintf(buf, fmt, v);
//         mu_draw_control_text(ctx, buf, _base, MU_COLOR.MU_COLOR_TEXT, opt);

//         return res;
//     }


//     static int mu_number_ex(mu_Context ctx, ref float value, float step,
//       string fmt, int opt)
//     {
//         char buf[MU_MAX_FMT + 1];
//         int res = 0;
//         int id = mu_get_id(ctx, &value, sizeof(value));
//         mu_Rect _base = mu_layout_next(ctx);
//         float last = *value;

//         /* handle text input mode */
//         if (number_textbox(ctx, value, _base, id)) { return res; }

//         /* handle normal mode */
//         mu_update_control(ctx, id, _base, opt);

//         /* handle input */
//         if (ctx.focus == id && ctx.mouse_down == MU_MOUSE_LEFT)
//         {
//             *value += ctx.mouse_delta.x * step;
//         }
//         /* set flag if value changed */
//         if (*value != last) { res |= MU_RES_CHANGE; }

//         /* draw base */
//         mu_draw_control_frame(ctx, id, _base, MU_COLOR.MU_COLOR_BASE, opt);
//         /* draw text  */
//         sprintf(buf, fmt, *value);
//         mu_draw_control_text(ctx, buf, _base, MU_COLOR.MU_COLOR_TEXT, opt);

//         return res;
//     }


//     static int header(mu_Context ctx, string label, int istreenode, int opt)
//     {
//         mu_Rect r;
//         int active, expanded;
//         int id = mu_get_id(ctx, label, label.length);
//         int idx = mu_pool_get(ctx, ctx.treenode_pool, MU_TREENODEPOOL_SIZE, id);
//         int width = -1;
//         mu_layout_row(ctx, 1, &width, 0);

//         active = (idx >= 0);
//         expanded = (opt & MU_OPT_EXPANDED) ? !active : active;
//         r = mu_layout_next(ctx);
//         mu_update_control(ctx, id, r, 0);

//         /* handle click */
//         active ^= (ctx.mouse_pressed == MU_MOUSE_LEFT && ctx.focus == id);

//         /* update pool ref */
//         if (idx >= 0)
//         {
//             if (active) { mu_pool_update(ctx, ctx.treenode_pool, idx); }
//             else { memset(&ctx.treenode_pool[idx], 0, sizeof(mu_PoolItem)); }
//         }
//         else if (active)
//         {
//             mu_pool_init(ctx, ctx.treenode_pool, MU_TREENODEPOOL_SIZE, id);
//         }

//         /* draw */
//         if (istreenode)
//         {
//             if (ctx.hover == id) { ctx.draw_frame(ctx, r, MU_COLOR.MU_COLOR_BUTTONHOVER); }
//         }
//         else
//         {
//             mu_draw_control_frame(ctx, id, r, MU_COLOR.MU_COLOR_BUTTON, 0);
//         }
//         mu_draw_icon(
//           ctx, expanded ? MU_ICON_EXPANDED : MU_ICON_COLLAPSED,
//           mu_rect(r.x, r.y, r.h, r.h), ctx.style.colors[MU_COLOR.MU_COLOR_TEXT]);
//         r.x += r.h - ctx.style.padding;
//         r.w -= r.h - ctx.style.padding;
//         mu_draw_control_text(ctx, label, r, MU_COLOR.MU_COLOR_TEXT, 0);

//         return expanded ? MU_RES_ACTIVE : 0;
//     }


//     static int mu_header_ex(mu_Context ctx, string label, int opt)
//     {
//         return header(ctx, label, 0, opt);
//     }


//     static int mu_begin_treenode_ex(mu_Context ctx, string label, int opt)
//     {
//         int res = header(ctx, label, 1, opt);
//         if (res & MU_RES_ACTIVE)
//         {
//             get_layout(ctx).indent += ctx.style.indent;
//             push(ctx.id_stack, ctx.last_id);
//         }
//         return res;
//     }


//     static void mu_end_treenode(mu_Context ctx)
//     {
//         get_layout(ctx).indent -= ctx.style.indent;
//         mu_pop_id(ctx);
//     }


//     static void scrollbar(mu_Context ctx, mu_Container cnt, mu_Rect b, int cs, int x, int y, int w, int h)
//     {
//         /* only add scrollbar if content size is larger than body */
//         int maxscroll = cs.y - b.h;

//         if (maxscroll > 0 && b.h > 0)
//         {
//             mu_Rect _base, thumb;
//             int id = mu_get_id(ctx, "!scrollbar" #y, 11);                       
                                                                            
//       /* get sizing / positioning */
//             _base = *b;
//             _base.x = b.x + b.w;
//             _base.w = ctx.style.scrollbar_size;

//             /* handle input */
//             mu_update_control(ctx, id, base, 0);
//             if (ctx.focus == id && ctx.mouse_down == MU_MOUSE_LEFT)
//             {
//                 cnt.scroll.y += ctx.mouse_delta.y * cs.y / base.h;
//             }
//             /* clamp scroll to limits */
//             cnt.scroll.y = mu_clamp(cnt.scroll.y, 0, maxscroll);

//             /* draw base and thumb */
//             ctx.draw_frame(ctx, base, MU_COLOR.MU_COLOR_SCROLLBASE);
//             thumb = base;
//             thumb.h = Math.Max(ctx.style.thumb_size, base.h * b.h / cs.y);
//             thumb.y += cnt.scroll.y * (base.h - thumb.h) / maxscroll;
//             ctx.draw_frame(ctx, thumb, MU_COLOR.MU_COLOR_SCROLLTHUMB);

//             /* set this as the scroll_target (will get scrolled on mousewheel) */
//             /* if the mouse is over it */
//             if (mu_mouse_over(ctx, *b)) { ctx.scroll_target = cnt; }
//         }
//         else
//         {
//             cnt.scroll.y = 0;
//         }
//     }


//     static void scrollbars(mu_Context ctx, mu_Container cnt, ref mu_Rect body)
//     {
//         int sz = ctx.style.scrollbar_size;
//         mu_Vec2 cs = cnt.content_size;
//         cs.x += ctx.style.padding * 2;
//         cs.y += ctx.style.padding * 2;
//         mu_push_clip_rect(ctx, body);
//         /* resize body to make room for scrollbars */
//         if (cs.y > cnt.body.h) { body.w -= sz; }
//         if (cs.x > cnt.body.w) { body.h -= sz; }
//         /* to create a horizontal or vertical scrollbar almost-identical code is
//         ** used; only the references to `x|y` `w|h` need to be switched */
//         scrollbar(ctx, cnt, body, cs, x, y, w, h);
//         scrollbar(ctx, cnt, body, cs, y, x, h, w);
//         mu_pop_clip_rect(ctx);
//     }


//     static void push_container_body(
//       mu_Context ctx, mu_Container cnt, mu_Rect body, int opt
//     )
//     {
//         if ((~opt & (int)MU_OPT.MU_OPT_NOSCROLL) > 0) { scrollbars(ctx, cnt, ref body); }
//         push_layout(ctx, expand_rect(body, -ctx.style.padding), cnt.scroll);
//         cnt.body = body;
//     }


//     static void begin_root_container(mu_Context ctx, mu_Container cnt)
//     {
//         push(ctx.container_stack, cnt);
//         /* push container to roots list and push head command */
//         push(ctx.root_list, cnt);
//         cnt.head = push_jump(ctx, null);
//         /* set as hover root if the mouse is overlapping this container and it has a
//         ** higher zindex than the current hover root */
//         if (rect_overlaps_vec2(cnt.rect, ctx.mouse_pos) &&
//             (!ctx.next_hover_root || cnt.zindex > ctx.next_hover_root.zindex)
//         )
//         {
//             ctx.next_hover_root = cnt;
//         }
//         /* clipping is reset here in case a root-container is made within
//         ** another root-containers's begin/end block; this prevents the inner
//         ** root-container being clipped to the outer */
//         push(ctx.clip_stack, unclipped_rect);
//     }


//     static void end_root_container(mu_Context ctx)
//     {
//         /* push tail 'goto' jump command and set head 'skip' command. the final steps
//         ** on initing these are done in mu_end() */
//         mu_Container* cnt = mu_get_current_container(ctx);
//         cnt.tail = push_jump(ctx, null);
//         cnt.head.jump.dst = ctx.command_list.items + ctx.command_list.idx;
//         /* pop base clip rect and container */
//         mu_pop_clip_rect(ctx);
//         pop_container(ctx);
//     }


//     static int mu_begin_window_ex(mu_Context ctx, string title, mu_Rect rect, int opt)
//     {
//         mu_Rect body;
//         int id = mu_get_id(ctx, title, title.length);
//         mu_Container cnt = get_container(ctx, id, opt);
//         if (!cnt || !cnt.open) { return 0; }
//         push(ctx.id_stack, id);

//         if (cnt.rect.w == 0) { cnt.rect = rect; }
//         begin_root_container(ctx, cnt);
//         rect = body = cnt.rect;

//         /* draw frame */
//         if (~opt & MU_OPT_NOFRAME)
//         {
//             ctx.draw_frame(ctx, rect, MU_COLOR.MU_COLOR_WINDOWBG);
//         }

//         /* do title bar */
//         if (~opt & MU_OPT_NOTITLE)
//         {
//             mu_Rect tr = rect;
//             tr.h = ctx.style.title_height;
//             ctx.draw_frame(ctx, tr, MU_COLOR.MU_COLOR_TITLEBG);

//             /* do title text */
//             if (~opt & MU_OPT_NOTITLE)
//             {
//                 int id = mu_get_id(ctx, "!title", 6);
//                 mu_update_control(ctx, id, tr, opt);
//                 mu_draw_control_text(ctx, title, tr, MU_COLOR.MU_COLOR_TITLETEXT, opt);
//                 if (id == ctx.focus && ctx.mouse_down == MU_MOUSE_LEFT)
//                 {
//                     cnt.rect.x += ctx.mouse_delta.x;
//                     cnt.rect.y += ctx.mouse_delta.y;
//                 }
//                 body.y += tr.h;
//                 body.h -= tr.h;
//             }

//             /* do `close` button */
//             if (~opt & MU_OPT_NOCLOSE)
//             {
//                 int id = mu_get_id(ctx, "!close", 6);
//                 mu_Rect r = mu_rect(tr.x + tr.w - tr.h, tr.y, tr.h, tr.h);
//                 tr.w -= r.w;
//                 mu_draw_icon(ctx, MU_ICON_CLOSE, r, ctx.style.colors[MU_COLOR.MU_COLOR_TITLETEXT]);
//                 mu_update_control(ctx, id, r, opt);
//                 if (ctx.mouse_pressed == MU_MOUSE_LEFT && id == ctx.focus)
//                 {
//                     cnt.open = 0;
//                 }
//             }
//         }

//         push_container_body(ctx, cnt, body, opt);

//         /* do `resize` handle */
//         if (~opt & MU_OPT_NORESIZE)
//         {
//             int sz = ctx.style.title_height;
//             int id = mu_get_id(ctx, "!resize", 7);
//             mu_Rect r = mu_rect(rect.x + rect.w - sz, rect.y + rect.h - sz, sz, sz);
//             mu_update_control(ctx, id, r, opt);
//             if (id == ctx.focus && ctx.mouse_down == MU_MOUSE_LEFT)
//             {
//                 cnt.rect.w = Math.Max(96, cnt.rect.w + ctx.mouse_delta.x);
//                 cnt.rect.h = Math.Max(64, cnt.rect.h + ctx.mouse_delta.y);
//             }
//         }

//         /* resize to content size */
//         if (opt & MU_OPT_AUTOSIZE)
//         {
//             mu_Rect r = get_layout(ctx).body;
//             cnt.rect.w = cnt.content_size.x + (cnt.rect.w - r.w);
//             cnt.rect.h = cnt.content_size.y + (cnt.rect.h - r.h);
//         }

//         /* close if this is a popup window and elsewhere was clicked */
//         if (opt & MU_OPT_POPUP && ctx.mouse_pressed && ctx.hover_root != cnt)
//         {
//             cnt.open = 0;
//         }

//         mu_push_clip_rect(ctx, cnt.body);
//         return MU_RES_ACTIVE;
//     }


//     static void mu_end_window(mu_Context ctx)
//     {
//         mu_pop_clip_rect(ctx);
//         end_root_container(ctx);
//     }


//     static void mu_open_popup(mu_Context ctx, string name)
//     {
//         mu_Container* cnt = mu_get_container(ctx, name);
//         /* set as hover root so popup isn't closed in begin_window_ex()  */
//         ctx.hover_root = ctx.next_hover_root = cnt;
//         /* position at mouse cursor, open and bring-to-front */
//         cnt.rect = mu_rect(ctx.mouse_pos.x, ctx.mouse_pos.y, 1, 1);
//         cnt.open = 1;
//         mu_bring_to_front(ctx, cnt);
//     }


//     static int mu_begin_popup(mu_Context ctx, string name)
//     {
//         int opt = MU_OPT_POPUP | MU_OPT_AUTOSIZE | MU_OPT_NORESIZE |
//                   MU_OPT_NOSCROLL | MU_OPT_NOTITLE | MU_OPT_CLOSED;
//         return mu_begin_window_ex(ctx, name, mu_rect(0, 0, 0, 0), opt);
//     }


//     static void mu_end_popup(mu_Context ctx)
//     {
//         mu_end_window(ctx);
//     }


//     static void mu_begin_panel_ex(mu_Context ctx, string name, int opt)
//     {
//         mu_Container* cnt;
//         mu_push_id(ctx, name, name.length);
//         cnt = get_container(ctx, ctx.last_id, opt);
//         cnt.rect = mu_layout_next(ctx);
//         if (~opt & MU_OPT_NOFRAME)
//         {
//             ctx.draw_frame(ctx, cnt.rect, MU_COLOR.MU_COLOR_PANELBG);
//         }
//         push(ctx.container_stack, cnt);
//         push_container_body(ctx, cnt, cnt.rect, opt);
//         mu_push_clip_rect(ctx, cnt.body);
//     }


//     static void mu_end_panel(mu_Context ctx)
//     {
//         mu_pop_clip_rect(ctx);
//         pop_container(ctx);
//     }

// }
