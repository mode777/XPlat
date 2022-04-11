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
        public Vertex A;
        public Vertex B;
        public Vertex C;
        public Vertex D;
    }

    public struct Color
    {
        public Color(byte r, byte g, byte b, byte a = 255){
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
    }

    public class SpriteBatch : IDisposable
    {
        const int MAX_SPRITES = ushort.MaxValue >> 2;

        private static Lazy<VertexIndices> __spriteIndices = new Lazy<VertexIndices>(() => {
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

        public int Capacity { get; private set; }

        private Vector2 screenSize;

        public int Count { get; private set; }

        private int currentCount = 0;
        private int currentStart = 0;
        private Texture currentTexture = null;
        private Rectangle currentSource = Rectangle.Empty;


        private Quad[] data;
        private readonly List<DrawCall> drawCalls;
        private readonly GlBufferHandle glBuffer;
        private readonly Primitive primitive;

        public void SetTexture(Texture material)
        {
            if (material == currentTexture) return;

            AddCall();
            currentTexture = material;
            currentSource = new Rectangle(0, 0, (int)material.Size.X, (int)material.Size.Y);
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
            if(Count >= Capacity) Resize(Math.Min(Capacity * 2, MAX_SPRITES));
            data[Count] = new Quad
            {
                A = new Vertex
                {
                    X = (short)x,
                    Y = (short)(y + currentSource.Height),
                    U = (ushort)currentSource.X,
                    V = (ushort)(currentSource.Y + currentSource.Height),
                    Color = color
                },
                B = new Vertex
                {
                    X = (short)x,
                    Y = (short)y,
                    U = (ushort)currentSource.X,
                    V = (ushort)currentSource.Y,
                    Color = color
                },
                C = new Vertex
                {
                    X = (short)(x + currentSource.Width),
                    Y = (short)y,
                    U = (ushort)(currentSource.X + currentSource.Width),
                    V = (ushort)currentSource.Y,
                    Color = color
                },
                D = new Vertex
                {
                    X = (short)(x + currentSource.Width),
                    Y = (short)(y + currentSource.Height),
                    U = (ushort)(currentSource.X + currentSource.Width),
                    V = (ushort)(currentSource.Y + currentSource.Height),
                    Color = color
                }
            };
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

            data[Count] = new Quad
            {
                A = new Vertex
                {
                    X = (short)a.X,
                    Y = (short)a.Y,
                    U = (ushort)currentSource.X,
                    V = (ushort)(currentSource.Y + currentSource.Height),
                    Color = color
                },
                B = new Vertex
                {
                    X = (short)b.X,
                    Y = (short)b.Y,
                    U = (ushort)currentSource.X,
                    V = (ushort)currentSource.Y,
                    Color = color
                },
                C = new Vertex
                {
                    X = (short)c.X,
                    Y = (short)c.Y,
                    U = (ushort)(currentSource.X + currentSource.Width),
                    V = (ushort)currentSource.Y,
                    Color = color
                },
                D = new Vertex
                {
                    X = (short)d.X,
                    Y = (short)d.Y,
                    U = (ushort)(currentSource.X + currentSource.Width),
                    V = (ushort)(currentSource.Y + currentSource.Height),
                    Color = color
                }
            };
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

            data[Count] = new Quad
            {
                A = new Vertex
                {
                    X = (short)a.X,
                    Y = (short)a.Y,
                    U = (ushort)currentSource.X,
                    V = (ushort)(currentSource.Y + currentSource.Height),
                    Color = color
                },
                B = new Vertex
                {
                    X = (short)b.X,
                    Y = (short)b.Y,
                    U = (ushort)currentSource.X,
                    V = (ushort)currentSource.Y,
                    Color = color
                },
                C = new Vertex
                {
                    X = (short)c.X,
                    Y = (short)c.Y,
                    U = (ushort)(currentSource.X + currentSource.Width),
                    V = (ushort)currentSource.Y,
                    Color = color
                },
                D = new Vertex
                {
                    X = (short)d.X,
                    Y = (short)d.Y,
                    U = (ushort)(currentSource.X + currentSource.Width),
                    V = (ushort)(currentSource.Y + currentSource.Height),
                    Color = color
                }
            };
            Count++;
            currentCount++;
        }

        public void Begin(int width, int height)
        {
            drawCalls.Clear();
            screenSize = new Vector2(width, height);
            Count = 0;
            currentCount = 0;
            currentStart = 0;
            currentTexture = null;
            color = new Color(255,255,255);
        }

        private void AddCall()
        {
            if (currentTexture == null) return;

            drawCalls.Add(new DrawCall
            {
                Size = currentCount,
                Start = currentStart,
                Texture = currentTexture
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
            shader.SetUniform(Uniform.ViewportSize, screenSize);

            foreach (var call in drawCalls)
            {
                shader.SetUniform(Uniform.TextureSize, call.Texture.Size);
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

