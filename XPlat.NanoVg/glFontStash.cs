
//
// Copyright (c) 2009-2013 Mikko Mononen memon@inside.org
//
// This software is provided 'as-is', without any express or implied
// warranty.  In no event will the authors be held liable for any damages
// arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
//

/*
 * Por to C#
 * Copyright (c) 2016 Miguel A. Guirado L. https://sites.google.com/site/bitiopia/
 * 
 * 	FontStash.net is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  any later version.
 *
 *  FontStash.net is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with FontStash.net.  If not, see <http://www.gnu.org/licenses/>. See
 *  the file lgpl-3.0.txt for more details.
*/

using System;
using GLES2;

namespace XPlat.FontStash
{
	public static class GlFontStash
	{
		public static FONScontext glfonsCreate(int width, int height, FONSflags flags)
		{
			FONSparams fparams;
			GLFONScontext gl = new GLFONScontext();

			fparams.width = width;
			fparams.height = height;
			fparams.flags = flags;
			fparams.renderCreate = glfons__renderCreate;
			fparams.renderResize = glfons__renderResize;
			fparams.renderUpdate = glfons__renderUpdate;
			fparams.renderDraw = glfons__renderDraw; 
			fparams.renderDelete = glfons__renderDelete;
			fparams.userPtr = gl;

			return FontStashApi.fonsCreateInternal(ref fparams);
		}

		public static void glfonsDelete(FONScontext ctx)
		{
			FontStashApi.fonsDeleteInternal(ctx);
		}

		public static int glfons__renderCreate(object userPtr, int width, int height)
		{
			GLFONScontext gl = userPtr as GLFONScontext;

			// Create may be called multiple times, delete existing texture.
			if (gl.tex != 0)
			{
				unsafe { fixed(uint* p = &gl.tex) { GL.DeleteTextures(1, p); } }
				gl.tex = 0;
			}
			unsafe { fixed(uint* p = &gl.tex) {GL.GenTextures(1, p); } }
			if (!(gl.tex != 0))
				return 0;
			gl.width = width;
			gl.height = height;
			GL.BindTexture(GL.TEXTURE_2D, gl.tex);
			unsafe {
				GL.TexImage2D(GL.TEXTURE_2D, 0, (int)GL.ALPHA,
					(uint)gl.width, (uint)gl.height, 0, GL.ALPHA, GL.UNSIGNED_BYTE, null);
			}
			GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 
				(int)GL.LINEAR);
			return 1;		
		}

		public static int glfons__renderResize(object uptr, int width, int height)
		{
			// Reuse create to resize too.
			return glfons__renderCreate(uptr, width, height);
		}

		public static void glfons__renderUpdate(object uptr, ref int[] rect, byte[] data)
		{
			throw new NotImplementedException();
			// GLFONScontext gl = (GLFONScontext)uptr;
			// int w = rect[2] - rect[0];
			// int h = rect[3] - rect[1];

			// if (gl.tex == 0)
			// 	return;
			// TODO: What is the impact of commenting out?
			//GL.PushClientAttrib(ClientAttribMask.ClientPixelStoreBit);
			//https://www.khronos.org/registry/OpenGL-Refpages/gl4/html/glPixelStore.xhtml
			// GL.BindTexture(GL.TEXTURE_2D, gl.tex);
			// GL.PixelStorei(GL.UNPACK_ALIGNMENT, 1);
			// GL.PixelStorei(GL.UNPACK_ROW_LENGTH, gl.width);
			// GL.PixelStorei(GL.UNPACK_SKIP_PIXELS, rect[0]);
			// GL.PixelStorei(GL.UNPACK_SKIP_ROWS, rect[1]);
			// unsafe { fixed(void* p = data) { GL.TexSubImage2D(GL.TEXTURE_2D, 0, rect[0], rect[1], w, h,
			// 	GL.ALPHA, GL.UNSIGNED_BYTE, p); }}
			//GL.PopClientAttrib();
		}

		public static void glfons__renderDraw(object userPtr, float[] verts, float[] tcoords, 
		                                      uint[] colors, int nverts)
		{
			throw new NotImplementedException();
			// GLFONScontext gl = (GLFONScontext)userPtr;
			// if (gl.tex == 0)
			// 	return;
			// GL.BindTexture(GL.TEXTURE_2D, gl.tex);
			// GL.Enable(GL.TEXTURE_2D);
			// GL.EnableClientState(GL.VERTEX_ARRAY);
			// GL.EnableClientState(GL.TEXTURE_COORD_ARRAY);
			// GL.EnableClientState(GL.COLOR_ARRAY);

			// //GL.VertexPointer(2, VertexPointerType.Float, sizeof(float)*2, verts);
			// //GL.TexCoordPointer(2, TexCoordPointerType.Float, sizeof(float)*2, tcoords);
			// //GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(uint), colors);

			// GL.VertexPointer(2, VertexPointerType.Float, 0, verts);
			// GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, tcoords);
			// GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, colors);

			// GL.DrawArrays(GL.TRIANGLES, 0, nverts);

			// GL.Disable(GL.TEXTURE_2D);
			// GL.DisableClientState(GL.VERTEX_ARRAY);
			// GL.DisableClientState(GL.TEXTURE_COORD_ARRAY);
			// GL.DisableClientState(GL.COLOR_ARRAY);
		}

		public static void glfons__renderDelete(object userPtr)
		{
			GLFONScontext gl = (GLFONScontext)userPtr;
			if (gl.tex != 0)
				unsafe { fixed(uint* p = &gl.tex) { GL.DeleteTextures(1, p); } }
			gl.tex = 0;
			//free(gl);
		}

		public static uint glfonsRGBA(byte r, byte g, byte b, byte a)
		{
			return (uint)((r) | (g << 8) | (b << 16) | (a << 24));
		}
	}

	public class GLFONScontext
	{
		public uint tex;
		public int width, height;
	}
}

