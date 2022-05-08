using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using GLES2;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.Graphics
{


    internal struct Quad
    {
        public Quad(int ax, int ay, int bx, int by, int cx, int cy, int dx, int dy, Rectangle r, Color c)
        {
            A = new Vertex
            {
                X = (short)ax,
                Y = (short)ay,
                U = (ushort)r.X,
                V = (ushort)(r.Y + r.Height),
                Color = c
            };
            B = new Vertex
            {
                X = (short)bx,
                Y = (short)by,
                U = (ushort)r.X,
                V = (ushort)r.Y,
                Color = c
            };
            C = new Vertex
            {
                X = (short)cx,
                Y = (short)cy,
                U = (ushort)(r.X + r.Width),
                V = (ushort)r.Y,
                Color = c
            };
            D = new Vertex
            {
                X = (short)dx,
                Y = (short)dy,
                U = (ushort)(r.X + r.Width),
                V = (ushort)(r.Y + r.Height),
                Color = c
            };
        }

        public Quad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Rectangle r, Color color)
        {
            A = new Vertex
            {
                X = (short)a.X,
                Y = (short)a.Y,
                U = (ushort)r.X,
                V = (ushort)(r.Y + r.Height),
                Color = color
            };
            B = new Vertex
            {
                X = (short)b.X,
                Y = (short)b.Y,
                U = (ushort)r.X,
                V = (ushort)r.Y,
                Color = color
            };
            C = new Vertex
            {
                X = (short)c.X,
                Y = (short)c.Y,
                U = (ushort)(r.X + r.Width),
                V = (ushort)r.Y,
                Color = color
            };
            D = new Vertex
            {
                X = (short)d.X,
                Y = (short)d.Y,
                U = (ushort)(r.X + r.Width),
                V = (ushort)(r.Y + r.Height),
                Color = color
            };
        }

        public Vertex A;
        public Vertex B;
        public Vertex C;
        public Vertex D;
    }

    public struct Color
    {
        public static readonly Color White = new Color(255,255,255);
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }

    internal struct Vertex
    {
        public short X;
        public short Y;
        public ushort U;
        public ushort V;
        public Color Color;
    }

    internal struct DrawCall
    {
        public int Start;
        public int Size;
        public Texture Texture;
        public Matrix4x4 Transform;
    }

    public class SpriteBatch : IDisposable
    {
        public const int MAX_SPRITES = ushort.MaxValue >> 2;

        private static Lazy<VertexIndices> __spriteIndices = new Lazy<VertexIndices>(() =>
        {
            ushort[] indices = new ushort[MAX_SPRITES * 6];
            for (int i = 0; i < MAX_SPRITES; i++)
            {
                var index = i * 4;
                var offset = i * 6;
                indices[offset] = (ushort)(index + 3);
                indices[offset + 1] = (ushort)(index + 2);
                indices[offset + 2] = (ushort)(index + 1);
                indices[offset + 3] = (ushort)(index + 3);
                indices[offset + 4] = (ushort)(index + 1);
                indices[offset + 5] = (ushort)(index + 0);
            }
            return new VertexIndices(indices);
        });

        static readonly VertexAttributeDescriptor PositionDescriptor;
        static readonly VertexAttributeDescriptor UvDescriptor;
        static readonly VertexAttributeDescriptor ColorDescriptor;

        static SpriteBatch()
        {
            unsafe
            {
                PositionDescriptor = new VertexAttributeDescriptor(2, GL.SHORT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>("X"));
                UvDescriptor = new VertexAttributeDescriptor(2, GL.UNSIGNED_SHORT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>("U"));
                ColorDescriptor = new VertexAttributeDescriptor(4, GL.UNSIGNED_BYTE, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>("Color"), true);
            }
        }

        public SpriteBatch(int capacity = 64)
        {
            Capacity = capacity;
            data = new Quad[Capacity];
            drawCalls = new List<DrawCall>();
            glBuffer = GlUtil.CreateBuffer(GL.ARRAY_BUFFER, data, GL.STREAM_DRAW);

            primitive = new Primitive(new VertexAttribute[]
            {
                new VertexAttribute(Attribute.Position, glBuffer, PositionDescriptor),
                new VertexAttribute(Attribute.Uv_0, glBuffer, UvDescriptor),
                new VertexAttribute(Attribute.Color, glBuffer, ColorDescriptor),
            }, __spriteIndices.Value);
        }

        public Camera2d Camera { get; set; } = new();

        public int Capacity { get; private set; }

        private Vector2 screenSize;

        public int Count { get; private set; }

        private int currentCount = 0;
        private int currentStart = 0;
        private Texture currentTexture = null;
        private Rectangle currentSource = Rectangle.Empty;
        private Matrix4x4 currentTransform = Matrix4x4.Identity;


        private Quad[] data;
        private readonly List<DrawCall> drawCalls;
        private readonly GlBufferHandle glBuffer;
        private readonly Primitive primitive;

        public void SetTexture(Texture material)
        {
            if (material == currentTexture) return;

            AddCall();
            currentTexture = material;
            currentSource = new Rectangle(0, 0, (int)material.Width, (int)material.Height);
        }

        public void SetTransform(Matrix4x4 mat){
            AddCall();
            currentTransform = mat;
        }

        public void ClearTransform(){
            if(currentTransform.IsIdentity) return;
            AddCall();
            currentTransform = Matrix4x4.Identity;
        }

        public void SetSource(Rectangle source)
        {
            currentSource = source;
        }
        private Color color = new Color { A = 255, R = 255, G = 255, B = 255 };
        private bool disposedValue;

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void SetSprite(SpriteSource spr)
        {
            if (spr == null) throw new ArgumentNullException("sprite");
            SetTexture(spr.Texture);
            SetSource(spr.Rectangle);
        }

        private void Resize(int capacity)
        {
            Array.Resize(ref data, capacity);
            GlUtil.ResizeBuffer(glBuffer, GL.ARRAY_BUFFER, data, GL.STREAM_DRAW);
            Capacity = capacity;
        }

        public void Draw(int x, int y)
        {
            if (Count >= Capacity) Resize(Math.Min(Capacity * 2, MAX_SPRITES));
            data[Count] = new Quad(
                x, y + currentSource.Height,
                x, y,
                x + currentSource.Width, y,
                x + currentSource.Width, y + currentSource.Height,
                currentSource,
                color);
            Count++;
            currentCount++;
        }

        public void Draw(ref Matrix3x2 mat)
        {
            if (Count >= Capacity) Resize(Math.Min(Capacity * 2, MAX_SPRITES));
            Vector2 a = Vector2.Transform(new Vector2(0, currentSource.Height), mat);
            Vector2 b = Vector2.Transform(new Vector2(0, 0), mat);
            Vector2 c = Vector2.Transform(new Vector2(currentSource.Width, 0), mat);
            Vector2 d = Vector2.Transform(new Vector2(currentSource.Width, currentSource.Height), mat);

            data[Count] = new Quad(a, b, c, d, currentSource, color);

            Count++;
            currentCount++;
        }

        public void Draw(ref Matrix4x4 mat)
        {
            if (Count >= Capacity) Resize(Math.Min(Capacity * 2, MAX_SPRITES));
            Vector2 a = Vector2.Transform(new Vector2(0, currentSource.Height), mat);
            Vector2 b = Vector2.Transform(new Vector2(0, 0), mat);
            Vector2 c = Vector2.Transform(new Vector2(currentSource.Width, 0), mat);
            Vector2 d = Vector2.Transform(new Vector2(currentSource.Width, currentSource.Height), mat);

            data[Count] = new Quad(a, b, c, d, currentSource, color);

            Count++;
            currentCount++;
        }

        public void Draw(SpriteBuffer buffer, ref Matrix4x4 transform){
            SetTexture(buffer.Texture);
            SetTransform(transform);
            if(Count+buffer.Size >= Capacity) Resize(Math.Min(Math.Max(Capacity * 2, Capacity + buffer.Size), MAX_SPRITES));

            Array.Copy(buffer.quads, 0, data, Count, buffer.Size);

            Count += buffer.Size;
            currentCount += buffer.Size;
            ClearTransform();
        }

        public void Begin(int width, int height)
        {
            drawCalls.Clear();
            screenSize = new Vector2(width, height);
            Count = 0;
            currentCount = 0;
            currentStart = 0;
            currentTexture = null;
            currentTransform = Matrix4x4.Identity;
            color = new Color(255, 255, 255);
        }

        private void AddCall()
        {
            if (currentTexture == null) return;
            if(currentCount == 0) return;

            drawCalls.Add(new DrawCall
            {
                Size = currentCount,
                Start = currentStart,
                Texture = currentTexture,
                Transform = currentTransform
            });
            currentCount = 0;
            currentStart = Count;
        }

        public void End()
        {
            AddCall();

            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
            GL.Enable(GL.BLEND);

            GlUtil.UpdateBuffer(glBuffer, GL.ARRAY_BUFFER, data, Count);

            var shader = SpriteBatchShader.Singleton;
            Shader.Use(shader);
            GL.UseProgram(shader.GlProgram.Handle);
            GL.ActiveTexture(GL.TEXTURE0);
            shader.SetUniform(Uniform.AlbedoTexture, 0);
            //shader.SetUniform(Uniform.ViewportSize, screenSize);
            Camera.Size = screenSize;
            Camera.ApplyToShader(shader);

            foreach (var call in drawCalls)
            {
                Matrix4x4 mat = call.Transform;
                shader.SetUniform(Uniform.ModelMatrix, ref mat);
                shader.SetUniform(Uniform.TextureSize, new Vector2(call.Texture.Width, call.Texture.Height));
                GL.BindTexture(GL.TEXTURE_2D, call.Texture.GlTexture.Handle);
                primitive.DrawWithShader(shader, call.Size * 6, call.Start * 6 * 2);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    glBuffer.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

