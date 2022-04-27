//using System;
//using System.Runtime.InteropServices;
//using Gwen.Net.OpenTk.Shaders;
//using OpenToolkit.Graphics.OpenGL;

//namespace Gwen.Net.OpenTk.Renderers
//{
//    public class OpenTKGL20Renderer : OpenTKRendererBase
//    {
//        private const int MaxVerts = 4096;

//        private readonly Vertex[] vertices;
//        private readonly int vertexSize;
//        private readonly bool restoreRenderState;
//        private readonly IShader shader;

//        private int vertNum;
//        private int totalVertNum;
//        private bool wasBlendEnabled;
//        private bool wasDepthTestEnabled;
//        private int prevBlendSrc;
//        private int prevBlendDst;
//        private int prevAlphaFunc;
//        private float prevAlphaRef;
//        private int vbo;

//        public override int VertexCount => totalVertNum;

//        public OpenTKGL20Renderer(bool restoreRenderState = true)
//            : base()
//        {
//            vertices = new Vertex[MaxVerts];
//            vertexSize = Marshal.SizeOf(vertices[0]);
//            this.restoreRenderState = restoreRenderState;

//            CreateBuffers();
//            shader = new GL20ShaderLoader().Load("gui.gl20");
//        }

//        private void CreateBuffers()
//        {
//            GL.GenBuffers(1, out vbo);
//            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
//            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexSize * MaxVerts), IntPtr.Zero, BufferUsageHint.StreamDraw); // Allocate

//            // Vertex positions
//            GL.EnableVertexAttribArray(0);
//            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, vertexSize, 0);

//            // Tex coords
//            GL.EnableVertexAttribArray(1);
//            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, vertexSize, 2 * sizeof(float));

//            // Colors
//            GL.EnableVertexAttribArray(2);
//            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, vertexSize, 2 * (sizeof(float) + sizeof(float)));

//            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
//        }

//        public override void Begin()
//        {
//            GL.ActiveTexture(TextureUnit.Texture0);
//            GL.UseProgram(shader.Program);

//            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

//            if (restoreRenderState)
//            {
//                // Get previous parameter values before changing them.
//                GL.GetInteger(GetPName.BlendSrc, out prevBlendSrc);
//                GL.GetInteger(GetPName.BlendDst, out prevBlendDst);
//                GL.GetInteger(GetPName.AlphaTestFunc, out prevAlphaFunc);
//                GL.GetFloat(GetPName.AlphaTestRef, out prevAlphaRef);

//                wasBlendEnabled = GL.IsEnabled(EnableCap.Blend);
//                wasDepthTestEnabled = GL.IsEnabled(EnableCap.DepthTest);
//            }

//            // Set default values and enable/disable caps.
//            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
//            GL.Enable(EnableCap.Blend);
//            GL.Disable(EnableCap.DepthTest);

//            vertNum = 0;
//            totalVertNum = 0;
//            drawCallCount = 0;
//            clipEnabled = false;
//            textureEnabled = false;
//            lastTextureID = -1;
//        }

//        public override void End()
//        {
//            Flush();

//            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

//            if (restoreRenderState)
//            {
//                GL.BindTexture(TextureTarget.Texture2D, 0);
//                lastTextureID = 0;

//                // Restore the previous parameter values.
//                GL.BlendFunc((BlendingFactor)prevBlendSrc, (BlendingFactor)prevBlendDst);

//                if (!wasBlendEnabled)
//                    GL.Disable(EnableCap.Blend);

//                if (wasDepthTestEnabled)
//                    GL.Enable(EnableCap.DepthTest);
//            }
//        }

//        protected override unsafe void Flush()
//        {
//            if (vertNum == 0) return;

//            //GL.InvalidateBufferData(m_Vbo);
//            GL.BufferSubData<Vertex>(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(vertNum * vertexSize), vertices);

//            GL.Uniform1(shader.Uniforms["uUseTexture"], textureEnabled ? 1.0f : 0.0f);

//            GL.DrawArrays(PrimitiveType.Triangles, 0, vertNum);

//            drawCallCount++;
//            totalVertNum += vertNum;
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

//                Rectangle clipRect = ClipRegion;

//                if (rect.Y < clipRect.Y)
//                {
//                    int oldHeight = rect.Height;
//                    int delta = clipRect.Y - rect.Y;
//                    rect.Y = clipRect.Y;
//                    rect.Height -= delta;

//                    if (rect.Height <= 0)
//                    {
//                        return;
//                    }

//                    float dv = (float)delta / (float)oldHeight;

//                    v1 += dv * (v2 - v1);
//                }

//                if ((rect.Y + rect.Height) > (clipRect.Y + clipRect.Height))
//                {
//                    int oldHeight = rect.Height;
//                    int delta = (rect.Y + rect.Height) - (clipRect.Y + clipRect.Height);

//                    rect.Height -= delta;

//                    if (rect.Height <= 0)
//                    {
//                        return;
//                    }

//                    float dv = (float)delta / (float)oldHeight;

//                    v2 -= dv * (v2 - v1);
//                }

//                if (rect.X < clipRect.X)
//                {
//                    int oldWidth = rect.Width;
//                    int delta = clipRect.X - rect.X;
//                    rect.X = clipRect.X;
//                    rect.Width -= delta;

//                    if (rect.Width <= 0)
//                    {
//                        return;
//                    }

//                    float du = (float)delta / (float)oldWidth;

//                    u1 += du * (u2 - u1);
//                }

//                if ((rect.X + rect.Width) > (clipRect.X + clipRect.Width))
//                {
//                    int oldWidth = rect.Width;
//                    int delta = (rect.X + rect.Width) - (clipRect.X + clipRect.Width);

//                    rect.Width -= delta;

//                    if (rect.Width <= 0)
//                    {
//                        return;
//                    }

//                    float du = (float)delta / (float)oldWidth;

//                    u2 -= du * (u2 - u1);
//                }
//            }

//            float cR = color.R / 255f;
//            float cG = color.G / 255f;
//            float cB = color.B / 255f;
//            float cA = color.A / 255f;

//            int vertexIndex = vertNum;
//            vertices[vertexIndex].x = (short)rect.X;
//            vertices[vertexIndex].y = (short)rect.Y;
//            vertices[vertexIndex].u = u1;
//            vertices[vertexIndex].v = v1;
//            vertices[vertexIndex].r = cR;
//            vertices[vertexIndex].g = cG;
//            vertices[vertexIndex].b = cB;
//            vertices[vertexIndex].a = cA;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)(rect.X + rect.Width);
//            vertices[vertexIndex].y = (short)rect.Y;
//            vertices[vertexIndex].u = u2;
//            vertices[vertexIndex].v = v1;
//            vertices[vertexIndex].r = cR;
//            vertices[vertexIndex].g = cG;
//            vertices[vertexIndex].b = cB;
//            vertices[vertexIndex].a = cA;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)(rect.X + rect.Width);
//            vertices[vertexIndex].y = (short)(rect.Y + rect.Height);
//            vertices[vertexIndex].u = u2;
//            vertices[vertexIndex].v = v2;
//            vertices[vertexIndex].r = cR;
//            vertices[vertexIndex].g = cG;
//            vertices[vertexIndex].b = cB;
//            vertices[vertexIndex].a = cA;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)rect.X;
//            vertices[vertexIndex].y = (short)rect.Y;
//            vertices[vertexIndex].u = u1;
//            vertices[vertexIndex].v = v1;
//            vertices[vertexIndex].r = cR;
//            vertices[vertexIndex].g = cG;
//            vertices[vertexIndex].b = cB;
//            vertices[vertexIndex].a = cA;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)(rect.X + rect.Width);
//            vertices[vertexIndex].y = (short)(rect.Y + rect.Height);
//            vertices[vertexIndex].u = u2;
//            vertices[vertexIndex].v = v2;
//            vertices[vertexIndex].r = cR;
//            vertices[vertexIndex].g = cG;
//            vertices[vertexIndex].b = cB;
//            vertices[vertexIndex].a = cA;

//            vertexIndex++;
//            vertices[vertexIndex].x = (short)rect.X;
//            vertices[vertexIndex].y = (short)(rect.Y + rect.Height);
//            vertices[vertexIndex].u = u1;
//            vertices[vertexIndex].v = v2;
//            vertices[vertexIndex].r = cR;
//            vertices[vertexIndex].g = cG;
//            vertices[vertexIndex].b = cB;
//            vertices[vertexIndex].a = cA;

//            vertNum += 6;
//        }

//        public override void Resize(int width, int height)
//        {
//            GL.Viewport(0, 0, width, height);
//            GL.UseProgram(shader.Program);
//            GL.Uniform2(shader.Uniforms["uScreenSize"], (float)width, (float)height);
//        }

//        [StructLayout(LayoutKind.Sequential, Pack = 1)]
//        public struct Vertex
//        {
//            public float x, y;
//            public float u, v;
//            public float r, g, b, a;
//        }
//    }
//}