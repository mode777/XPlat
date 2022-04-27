//using System;
//using System.Runtime.InteropServices;
//using OpenToolkit.Graphics.OpenGL;

//namespace Gwen.Net.OpenTk.Renderers
//{
//    public class OpenTKGL10Renderer : OpenTKRendererBase
//    {
//        private const int MaxVerts = 1024;

//        [StructLayout(LayoutKind.Sequential, Pack = 1)]
//        public struct Vertex
//        {
//            public short x, y;
//            public float u, v;
//            public byte r, g, b, a;
//        }

//        private int vertNum;
//        private readonly Vertex[] vertices;
//        private readonly int vertexSize;

//        private bool wasBlendEnabled, wasTexture2DEnabled, wasDepthTestEnabled;
//        private int prevBlendSrc, prevBlendDst, prevAlphaFunc;
//        private float prevAlphaRef;
//        private bool restoreRenderState;

//        public OpenTKGL10Renderer(bool restoreRenderState = true)
//            : base()
//        {
//            vertices = new Vertex[MaxVerts];
//            vertexSize = Marshal.SizeOf(vertices[0]);
//            this.restoreRenderState = restoreRenderState;
//        }

//        public override void Begin()
//        {
//            if (restoreRenderState)
//            {
//                // Get previous parameter values before changing them.
//                GL.GetInteger(GetPName.BlendSrc, out prevBlendSrc);
//                GL.GetInteger(GetPName.BlendDst, out prevBlendDst);
//                GL.GetInteger(GetPName.AlphaTestFunc, out prevAlphaFunc);
//                GL.GetFloat(GetPName.AlphaTestRef, out prevAlphaRef);

//                wasBlendEnabled = GL.IsEnabled(EnableCap.Blend);
//                wasTexture2DEnabled = GL.IsEnabled(EnableCap.Texture2D);
//                wasDepthTestEnabled = GL.IsEnabled(EnableCap.DepthTest);
//            }

//            // Set default values and enable/disable caps.
//            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
//            GL.AlphaFunc(AlphaFunction.Greater, 1.0f);
//            GL.Enable(EnableCap.Blend);
//            GL.Disable(EnableCap.DepthTest);
//            GL.Disable(EnableCap.Texture2D);

//            vertNum = 0;
//            drawCallCount = 0;
//            clipEnabled = false;
//            textureEnabled = false;
//            lastTextureID = -1;

//            GL.EnableClientState(ArrayCap.VertexArray);
//            GL.EnableClientState(ArrayCap.ColorArray);
//            GL.EnableClientState(ArrayCap.TextureCoordArray);
//        }

//        public override void End()
//        {
//            Flush();

//            if (restoreRenderState)
//            {
//                GL.BindTexture(TextureTarget.Texture2D, 0);
//                lastTextureID = 0;

//                // Restore the previous parameter values.
//                GL.BlendFunc((BlendingFactor)prevBlendSrc, (BlendingFactor)prevBlendDst);
//                GL.AlphaFunc((AlphaFunction)prevAlphaFunc, prevAlphaRef);

//                if (!wasBlendEnabled)
//                    GL.Disable(EnableCap.Blend);

//                if (wasTexture2DEnabled && !textureEnabled)
//                    GL.Enable(EnableCap.Texture2D);

//                if (wasDepthTestEnabled)
//                    GL.Enable(EnableCap.DepthTest);
//            }

//            GL.DisableClientState(ArrayCap.VertexArray);
//            GL.DisableClientState(ArrayCap.ColorArray);
//            GL.DisableClientState(ArrayCap.TextureCoordArray);
//        }

//        public override int VertexCount { get { return vertNum; } }

//        protected override unsafe void Flush()
//        {
//            if (vertNum == 0) return;

//            fixed (short* ptr1 = &vertices[0].x)
//            fixed (byte* ptr2 = &vertices[0].r)
//            fixed (float* ptr3 = &vertices[0].u)
//            {
//                GL.VertexPointer(2, VertexPointerType.Short, vertexSize, (IntPtr)ptr1);
//                GL.ColorPointer(4, ColorPointerType.UnsignedByte, vertexSize, (IntPtr)ptr2);
//                GL.TexCoordPointer(2, TexCoordPointerType.Float, vertexSize, (IntPtr)ptr3);

//                GL.DrawArrays(PrimitiveType.Quads, 0, vertNum);
//            }

//            drawCallCount++;
//            vertNum = 0;
//        }

//        protected override void DrawRect(Rectangle rect, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
//        {
//            if (vertNum + 4 >= MaxVerts)
//            {
//                Flush();
//            }

//            if (clipEnabled)
//            {
//                // cpu scissors test

//                if (rect.Y < ClipRegion.Y)
//                {
//                    int oldHeight = rect.Height;
//                    int delta = ClipRegion.Y - rect.Y;
//                    rect.Y = ClipRegion.Y;
//                    rect.Height -= delta;

//                    if (rect.Height <= 0)
//                    {
//                        return;
//                    }

//                    float dv = (float)delta / (float)oldHeight;

//                    v1 += dv * (v2 - v1);
//                }

//                if ((rect.Y + rect.Height) > (ClipRegion.Y + ClipRegion.Height))
//                {
//                    int oldHeight = rect.Height;
//                    int delta = (rect.Y + rect.Height) - (ClipRegion.Y + ClipRegion.Height);

//                    rect.Height -= delta;

//                    if (rect.Height <= 0)
//                    {
//                        return;
//                    }

//                    float dv = (float)delta / (float)oldHeight;

//                    v2 -= dv * (v2 - v1);
//                }

//                if (rect.X < ClipRegion.X)
//                {
//                    int oldWidth = rect.Width;
//                    int delta = ClipRegion.X - rect.X;
//                    rect.X = ClipRegion.X;
//                    rect.Width -= delta;

//                    if (rect.Width <= 0)
//                    {
//                        return;
//                    }

//                    float du = (float)delta / (float)oldWidth;

//                    u1 += du * (u2 - u1);
//                }

//                if ((rect.X + rect.Width) > (ClipRegion.X + ClipRegion.Width))
//                {
//                    int oldWidth = rect.Width;
//                    int delta = (rect.X + rect.Width) - (ClipRegion.X + ClipRegion.Width);

//                    rect.Width -= delta;

//                    if (rect.Width <= 0)
//                    {
//                        return;
//                    }

//                    float du = (float)delta / (float)oldWidth;

//                    u2 -= du * (u2 - u1);
//                }
//            }

//            int vertexIndex = vertNum;
//            vertices[vertexIndex].x = (short)rect.X;
//            vertices[vertexIndex].y = (short)rect.Y;
//            vertices[vertexIndex].u = u1;
//            vertices[vertexIndex].v = v1;
//            vertices[vertexIndex].r = color.R;
//            vertices[vertexIndex].g = color.G;
//            vertices[vertexIndex].b = color.B;
//            vertices[vertexIndex].a = color.A;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)(rect.X + rect.Width);
//            vertices[vertexIndex].y = (short)rect.Y;
//            vertices[vertexIndex].u = u2;
//            vertices[vertexIndex].v = v1;
//            vertices[vertexIndex].r = color.R;
//            vertices[vertexIndex].g = color.G;
//            vertices[vertexIndex].b = color.B;
//            vertices[vertexIndex].a = color.A;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)(rect.X + rect.Width);
//            vertices[vertexIndex].y = (short)(rect.Y + rect.Height);
//            vertices[vertexIndex].u = u2;
//            vertices[vertexIndex].v = v2;
//            vertices[vertexIndex].r = color.R;
//            vertices[vertexIndex].g = color.G;
//            vertices[vertexIndex].b = color.B;
//            vertices[vertexIndex].a = color.A;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)rect.X;
//            vertices[vertexIndex].y = (short)(rect.Y + rect.Height);
//            vertices[vertexIndex].u = u1;
//            vertices[vertexIndex].v = v2;
//            vertices[vertexIndex].r = color.R;
//            vertices[vertexIndex].g = color.G;
//            vertices[vertexIndex].b = color.B;
//            vertices[vertexIndex].a = color.A;

//            vertNum += 4;
//        }

//        public override void Resize(int width, int height)
//        {
//            GL.Viewport(0, 0, width, height);
//            GL.MatrixMode(MatrixMode.Projection);
//            GL.LoadIdentity();
//            GL.Ortho(0, width, height, 0, -1, 1);
//        }
//    }
//}