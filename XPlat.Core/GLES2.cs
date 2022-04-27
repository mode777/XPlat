using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GLES2
{
    public static class GL
    {
        private const string nativeLibName = "libGLESv2";

        #region CONSTANTS

        public const uint DEPTH_BUFFER_BIT = 0x00000100;
        public const uint STENCIL_BUFFER_BIT = 0x00000400;
        public const uint COLOR_BUFFER_BIT = 0x00004000;
        public const uint FALSE = 0;
        public const uint TRUE = 1;
        public const uint POINTS = 0x0000;
        public const uint LINES = 0x0001;
        public const uint LINE_LOOP = 0x0002;
        public const uint LINE_STRIP = 0x0003;
        public const uint TRIANGLES = 0x0004;
        public const uint TRIANGLE_STRIP = 0x0005;
        public const uint TRIANGLE_FAN = 0x0006;
        public const uint ZERO = 0;
        public const uint ONE = 1;
        public const uint SRC_COLOR = 0x0300;
        public const uint ONE_MINUS_SRC_COLOR = 0x0301;
        public const uint SRC_ALPHA = 0x0302;
        public const uint ONE_MINUS_SRC_ALPHA = 0x0303;
        public const uint DST_ALPHA = 0x0304;
        public const uint ONE_MINUS_DST_ALPHA = 0x0305;
        public const uint DST_COLOR = 0x0306;
        public const uint ONE_MINUS_DST_COLOR = 0x0307;
        public const uint SRC_ALPHA_SATURATE = 0x0308;
        public const uint FUNC_ADD = 0x8006;
        public const uint BLEND_EQUATION = 0x8009;
        public const uint BLEND_EQUATION_RGB = 0x8009;
        public const uint BLEND_EQUATION_ALPHA = 0x883D;
        public const uint FUNC_SUBTRACT = 0x800A;
        public const uint FUNC_REVERSE_SUBTRACT = 0x800B;
        public const uint BLEND_DST_RGB = 0x80C8;
        public const uint BLEND_SRC_RGB = 0x80C9;
        public const uint BLEND_DST_ALPHA = 0x80CA;
        public const uint BLEND_SRC_ALPHA = 0x80CB;
        public const uint CONSTANT_COLOR = 0x8001;
        public const uint ONE_MINUS_CONSTANT_COLOR = 0x8002;
        public const uint CONSTANT_ALPHA = 0x8003;
        public const uint ONE_MINUS_CONSTANT_ALPHA = 0x8004;
        public const uint BLEND_COLOR = 0x8005;
        public const uint ARRAY_BUFFER = 0x8892;
        public const uint ELEMENT_ARRAY_BUFFER = 0x8893;
        public const uint ARRAY_BUFFER_BINDING = 0x8894;
        public const uint ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
        public const uint STREAM_DRAW = 0x88E0;
        public const uint STATIC_DRAW = 0x88E4;
        public const uint DYNAMIC_DRAW = 0x88E8;
        public const uint BUFFER_SIZE = 0x8764;
        public const uint BUFFER_USAGE = 0x8765;
        public const uint CURRENT_VERTEX_ATTRIB = 0x8626;
        public const uint FRONT = 0x0404;
        public const uint BACK = 0x0405;
        public const uint FRONT_AND_BACK = 0x0408;
        public const uint TEXTURE_2D = 0x0DE1;
        public const uint CULL_FACE = 0x0B44;
        public const uint BLEND = 0x0BE2;
        public const uint DITHER = 0x0BD0;
        public const uint STENCIL_TEST = 0x0B90;
        public const uint DEPTH_TEST = 0x0B71;
        public const uint SCISSOR_TEST = 0x0C11;
        public const uint POLYGON_OFFSET_FILL = 0x8037;
        public const uint SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
        public const uint SAMPLE_COVERAGE = 0x80A0;
        public const uint NO_ERROR = 0;
        public const uint INVALID_ENUM = 0x0500;
        public const uint INVALID_VALUE = 0x0501;
        public const uint INVALID_OPERATION = 0x0502;
        public const uint OUT_OF_MEMORY = 0x0505;
        public const uint CW = 0x0900;
        public const uint CCW = 0x0901;
        public const uint LINE_WIDTH = 0x0B21;
        public const uint ALIASED_POINT_SIZE_RANGE = 0x846D;
        public const uint ALIASED_LINE_WIDTH_RANGE = 0x846E;
        public const uint CULL_FACE_MODE = 0x0B45;
        public const uint FRONT_FACE = 0x0B46;
        public const uint DEPTH_RANGE = 0x0B70;
        public const uint DEPTH_WRITEMASK = 0x0B72;
        public const uint DEPTH_CLEAR_VALUE = 0x0B73;
        public const uint DEPTH_FUNC = 0x0B74;
        public const uint STENCIL_CLEAR_VALUE = 0x0B91;
        public const uint STENCIL_FUNC = 0x0B92;
        public const uint STENCIL_FAIL = 0x0B94;
        public const uint STENCIL_PASS_DEPTH_FAIL = 0x0B95;
        public const uint STENCIL_PASS_DEPTH_PASS = 0x0B96;
        public const uint STENCIL_REF = 0x0B97;
        public const uint STENCIL_VALUE_MASK = 0x0B93;
        public const uint STENCIL_WRITEMASK = 0x0B98;
        public const uint STENCIL_BACK_FUNC = 0x8800;
        public const uint STENCIL_BACK_FAIL = 0x8801;
        public const uint STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
        public const uint STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
        public const uint STENCIL_BACK_REF = 0x8CA3;
        public const uint STENCIL_BACK_VALUE_MASK = 0x8CA4;
        public const uint STENCIL_BACK_WRITEMASK = 0x8CA5;
        public const uint VIEWPORT = 0x0BA2;
        public const uint SCISSOR_BOX = 0x0C10;
        public const uint COLOR_CLEAR_VALUE = 0x0C22;
        public const uint COLOR_WRITEMASK = 0x0C23;
        public const uint UNPACK_ALIGNMENT = 0x0CF5;
        public const uint PACK_ALIGNMENT = 0x0D05;
        public const uint MAX_TEXTURE_SIZE = 0x0D33;
        public const uint MAX_VIEWPORT_DIMS = 0x0D3A;
        public const uint SUBPIXEL_BITS = 0x0D50;
        public const uint RED_BITS = 0x0D52;
        public const uint GREEN_BITS = 0x0D53;
        public const uint BLUE_BITS = 0x0D54;
        public const uint ALPHA_BITS = 0x0D55;
        public const uint DEPTH_BITS = 0x0D56;
        public const uint STENCIL_BITS = 0x0D57;
        public const uint POLYGON_OFFSET_UNITS = 0x2A00;
        public const uint POLYGON_OFFSET_FACTOR = 0x8038;
        public const uint TEXTURE_BINDING_2D = 0x8069;
        public const uint SAMPLE_BUFFERS = 0x80A8;
        public const uint SAMPLES = 0x80A9;
        public const uint SAMPLE_COVERAGE_VALUE = 0x80AA;
        public const uint SAMPLE_COVERAGE_INVERT = 0x80AB;
        public const uint NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
        public const uint COMPRESSED_TEXTURE_FORMATS = 0x86A3;
        public const uint DONT_CARE = 0x1100;
        public const uint FASTEST = 0x1101;
        public const uint NICEST = 0x1102;
        public const uint GENERATE_MIPMAP_HINT = 0x8192;
        public const uint BYTE = 0x1400;
        public const uint UNSIGNED_BYTE = 0x1401;
        public const uint SHORT = 0x1402;
        public const uint UNSIGNED_SHORT = 0x1403;
        public const uint INT = 0x1404;
        public const uint UNSIGNED_INT = 0x1405;
        public const uint FLOAT = 0x1406;
        public const uint FIXED = 0x140C;
        public const uint DEPTH_COMPONENT = 0x1902;
        public const uint ALPHA = 0x1906;
        public const uint RGB = 0x1907;
        public const uint RGBA = 0x1908;
        public const uint LUMINANCE = 0x1909;
        public const uint LUMINANCE_ALPHA = 0x190A;
        public const uint UNSIGNED_SHORT_4_4_4_4 = 0x8033;
        public const uint UNSIGNED_SHORT_5_5_5_1 = 0x8034;
        public const uint UNSIGNED_SHORT_5_6_5 = 0x8363;
        public const uint FRAGMENT_SHADER = 0x8B30;
        public const uint VERTEX_SHADER = 0x8B31;
        public const uint MAX_VERTEX_ATTRIBS = 0x8869;
        public const uint MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
        public const uint MAX_VARYING_VECTORS = 0x8DFC;
        public const uint MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
        public const uint MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
        public const uint MAX_TEXTURE_IMAGE_UNITS = 0x8872;
        public const uint MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
        public const uint SHADER_TYPE = 0x8B4F;
        public const uint DELETE_STATUS = 0x8B80;
        public const uint LINK_STATUS = 0x8B82;
        public const uint VALIDATE_STATUS = 0x8B83;
        public const uint ATTACHED_SHADERS = 0x8B85;
        public const uint ACTIVE_UNIFORMS = 0x8B86;
        public const uint ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
        public const uint ACTIVE_ATTRIBUTES = 0x8B89;
        public const uint ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
        public const uint SHADING_LANGUAGE_VERSION = 0x8B8C;
        public const uint CURRENT_PROGRAM = 0x8B8D;
        public const uint NEVER = 0x0200;
        public const uint LESS = 0x0201;
        public const uint EQUAL = 0x0202;
        public const uint LEQUAL = 0x0203;
        public const uint GREATER = 0x0204;
        public const uint NOTEQUAL = 0x0205;
        public const uint GEQUAL = 0x0206;
        public const uint ALWAYS = 0x0207;
        public const uint KEEP = 0x1E00;
        public const uint REPLACE = 0x1E01;
        public const uint INCR = 0x1E02;
        public const uint DECR = 0x1E03;
        public const uint INVERT = 0x150A;
        public const uint INCR_WRAP = 0x8507;
        public const uint DECR_WRAP = 0x8508;
        public const uint VENDOR = 0x1F00;
        public const uint RENDERER = 0x1F01;
        public const uint VERSION = 0x1F02;
        public const uint EXTENSIONS = 0x1F03;
        public const uint NEAREST = 0x2600;
        public const uint LINEAR = 0x2601;
        public const uint NEAREST_MIPMAP_NEAREST = 0x2700;
        public const uint LINEAR_MIPMAP_NEAREST = 0x2701;
        public const uint NEAREST_MIPMAP_LINEAR = 0x2702;
        public const uint LINEAR_MIPMAP_LINEAR = 0x2703;
        public const uint TEXTURE_MAG_FILTER = 0x2800;
        public const uint TEXTURE_MIN_FILTER = 0x2801;
        public const uint TEXTURE_WRAP_S = 0x2802;
        public const uint TEXTURE_WRAP_T = 0x2803;
        public const uint TEXTURE = 0x1702;
        public const uint TEXTURE_CUBE_MAP = 0x8513;
        public const uint TEXTURE_BINDING_CUBE_MAP = 0x8514;
        public const uint TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
        public const uint TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
        public const uint TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
        public const uint TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
        public const uint TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
        public const uint TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
        public const uint MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
        public const uint TEXTURE0 = 0x84C0;
        public const uint TEXTURE1 = 0x84C1;
        public const uint TEXTURE2 = 0x84C2;
        public const uint TEXTURE3 = 0x84C3;
        public const uint TEXTURE4 = 0x84C4;
        public const uint TEXTURE5 = 0x84C5;
        public const uint TEXTURE6 = 0x84C6;
        public const uint TEXTURE7 = 0x84C7;
        public const uint TEXTURE8 = 0x84C8;
        public const uint TEXTURE9 = 0x84C9;
        public const uint TEXTURE10 = 0x84CA;
        public const uint TEXTURE11 = 0x84CB;
        public const uint TEXTURE12 = 0x84CC;
        public const uint TEXTURE13 = 0x84CD;
        public const uint TEXTURE14 = 0x84CE;
        public const uint TEXTURE15 = 0x84CF;
        public const uint TEXTURE16 = 0x84D0;
        public const uint TEXTURE17 = 0x84D1;
        public const uint TEXTURE18 = 0x84D2;
        public const uint TEXTURE19 = 0x84D3;
        public const uint TEXTURE20 = 0x84D4;
        public const uint TEXTURE21 = 0x84D5;
        public const uint TEXTURE22 = 0x84D6;
        public const uint TEXTURE23 = 0x84D7;
        public const uint TEXTURE24 = 0x84D8;
        public const uint TEXTURE25 = 0x84D9;
        public const uint TEXTURE26 = 0x84DA;
        public const uint TEXTURE27 = 0x84DB;
        public const uint TEXTURE28 = 0x84DC;
        public const uint TEXTURE29 = 0x84DD;
        public const uint TEXTURE30 = 0x84DE;
        public const uint TEXTURE31 = 0x84DF;
        public const uint ACTIVE_TEXTURE = 0x84E0;
        public const uint REPEAT = 0x2901;
        public const uint CLAMP_TO_EDGE = 0x812F;
        public const uint MIRRORED_REPEAT = 0x8370;
        public const uint FLOAT_VEC2 = 0x8B50;
        public const uint FLOAT_VEC3 = 0x8B51;
        public const uint FLOAT_VEC4 = 0x8B52;
        public const uint INT_VEC2 = 0x8B53;
        public const uint INT_VEC3 = 0x8B54;
        public const uint INT_VEC4 = 0x8B55;
        public const uint BOOL = 0x8B56;
        public const uint BOOL_VEC2 = 0x8B57;
        public const uint BOOL_VEC3 = 0x8B58;
        public const uint BOOL_VEC4 = 0x8B59;
        public const uint FLOAT_MAT2 = 0x8B5A;
        public const uint FLOAT_MAT3 = 0x8B5B;
        public const uint FLOAT_MAT4 = 0x8B5C;
        public const uint SAMPLER_2D = 0x8B5E;
        public const uint SAMPLER_CUBE = 0x8B60;
        public const uint VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
        public const uint VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
        public const uint VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
        public const uint VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
        public const uint VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
        public const uint VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
        public const uint VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
        public const uint IMPLEMENTATION_COLOR_READ_TYPE = 0x8B9A;
        public const uint IMPLEMENTATION_COLOR_READ_FORMAT = 0x8B9B;
        public const uint COMPILE_STATUS = 0x8B81;
        public const uint INFO_LOG_LENGTH = 0x8B84;
        public const uint SHADER_SOURCE_LENGTH = 0x8B88;
        public const uint SHADER_COMPILER = 0x8DFA;
        public const uint SHADER_BINARY_FORMATS = 0x8DF8;
        public const uint NUM_SHADER_BINARY_FORMATS = 0x8DF9;
        public const uint LOW_FLOAT = 0x8DF0;
        public const uint MEDIUM_FLOAT = 0x8DF1;
        public const uint HIGH_FLOAT = 0x8DF2;
        public const uint LOW_INT = 0x8DF3;
        public const uint MEDIUM_INT = 0x8DF4;
        public const uint HIGH_INT = 0x8DF5;
        public const uint FRAMEBUFFER = 0x8D40;
        public const uint RENDERBUFFER = 0x8D41;
        public const uint RGBA4 = 0x8056;
        public const uint RGB5_A1 = 0x8057;
        public const uint RGB565 = 0x8D62;
        public const uint DEPTH_COMPONENT16 = 0x81A5;
        public const uint STENCIL_INDEX8 = 0x8D48;
        public const uint RENDERBUFFER_WIDTH = 0x8D42;
        public const uint RENDERBUFFER_HEIGHT = 0x8D43;
        public const uint RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
        public const uint RENDERBUFFER_RED_SIZE = 0x8D50;
        public const uint RENDERBUFFER_GREEN_SIZE = 0x8D51;
        public const uint RENDERBUFFER_BLUE_SIZE = 0x8D52;
        public const uint RENDERBUFFER_ALPHA_SIZE = 0x8D53;
        public const uint RENDERBUFFER_DEPTH_SIZE = 0x8D54;
        public const uint RENDERBUFFER_STENCIL_SIZE = 0x8D55;
        public const uint FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
        public const uint FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
        public const uint FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
        public const uint FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
        public const uint COLOR_ATTACHMENT0 = 0x8CE0;
        public const uint DEPTH_ATTACHMENT = 0x8D00;
        public const uint STENCIL_ATTACHMENT = 0x8D20;
        public const uint NONE = 0;
        public const uint FRAMEBUFFER_COMPLETE = 0x8CD5;
        public const uint FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
        public const uint FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
        public const uint FRAMEBUFFER_INCOMPLETE_DIMENSIONS = 0x8CD9;
        public const uint FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
        public const uint FRAMEBUFFER_BINDING = 0x8CA6;
        public const uint RENDERBUFFER_BINDING = 0x8CA7;
        public const uint MAX_RENDERBUFFER_SIZE = 0x84E8;
        public const uint INVALID_FRAMEBUFFER_OPERATION = 0x0506;
        #endregion

        #region API
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glActiveTexture")]
        public static extern void ActiveTexture(uint texture);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glAttachShader")]
        public static extern void AttachShader(uint program, uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBindAttribLocation")]
        public static extern void BindAttribLocation(uint program, uint index, string name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBindBuffer")]
        public static extern void BindBuffer(uint target, uint buffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBindFramebuffer")]
        public static extern void BindFramebuffer(uint target, uint framebuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBindRenderbuffer")]
        public static extern void BindRenderbuffer(uint target, uint renderbuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBindTexture")]
        public static extern void BindTexture(uint target, uint texture);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBlendColor")]
        public static extern void BlendColor(float red, float green, float blue, float alpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBlendEquation")]
        public static extern void BlendEquation(uint mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBlendEquationSeparate")]
        public static extern void BlendEquationSeparate(uint modeRGB, uint modeAlpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBlendFunc")]
        public static extern void BlendFunc(uint sfactor, uint dfactor);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBlendFuncSeparate")]
        public static extern void BlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBufferData")]
        public static unsafe extern void BufferData(uint target, uint size, void* data, uint usage);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glBufferSubData")]
        public static unsafe extern void BufferSubData(uint target, uint offset, uint size, void* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCheckFramebufferStatus")]
        public static extern uint CheckFramebufferStatus(uint target);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glClear")]
        public static extern void Clear(uint mask);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glClearColor")]
        public static extern void ClearColor(float red, float green, float blue, float alpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glClearDepthf")]
        public static extern void ClearDepthf(float d);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glClearStencil")]
        public static extern void ClearStencil(int s);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glColorMask")]
        public static extern void ColorMask(bool red, bool green, bool blue, bool alpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCompileShader")]
        public static extern void CompileShader(uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCompressedTexImage2D")]
        public static unsafe extern void CompressedTexImage2D(uint target, int level, uint internalformat, uint width, uint height, int border, uint imageSize, void* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCompressedTexSubImage2D")]
        public static unsafe extern void CompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, uint width, uint height, uint format, uint imageSize, void* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCopyTexImage2D")]
        public static extern void CopyTexImage2D(uint target, int level, uint internalformat, int x, int y, uint width, uint height, int border);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCopyTexSubImage2D")]
        public static extern void CopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, uint width, uint height);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCreateProgram")]
        public static extern uint CreateProgram();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCreateShader")]
        public static extern uint CreateShader(uint type);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glCullFace")]
        public static extern void CullFace(uint mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.StdCall, EntryPoint="glDeleteBuffers")]
        public static unsafe extern void DeleteBuffers(uint n, uint* buffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDeleteFramebuffers")]
        public static unsafe extern void DeleteFramebuffers(uint n, uint* framebuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDeleteProgram")]
        public static extern void DeleteProgram(uint program);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDeleteRenderbuffers")]
        public static unsafe extern void DeleteRenderbuffers(uint n, uint* renderbuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDeleteShader")]
        public static extern void DeleteShader(uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDeleteTextures")]
        public static unsafe extern void DeleteTextures(uint n, uint* textures);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDepthFunc")]
        public static extern void DepthFunc(uint func);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDepthMask")]
        public static extern void DepthMask(bool flag);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDepthRangef")]
        public static extern void DepthRangef(float n, float f);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDetachShader")]
        public static extern void DetachShader(uint program, uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDisable")]
        public static extern void Disable(uint cap);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDisableVertexAttribArray")]
        public static extern void DisableVertexAttribArray(uint index);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDrawArrays")]
        public static extern void DrawArrays(uint mode, int first, uint count);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glDrawElements")]
        public static unsafe extern void DrawElements(uint mode, uint count, uint type, IntPtr indices);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glEnable")]
        public static extern void Enable(uint cap);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glEnableVertexAttribArray")]
        public static extern void EnableVertexAttribArray(uint index);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glFinish")]
        public static extern void Finish();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glFlush")]
        public static extern void Flush();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glFramebufferRenderbuffer")]
        public static extern void FramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glFramebufferTexture2D")]
        public static extern void FramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glFrontFace")]
        public static extern void FrontFace(uint mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGenBuffers")]
        public static unsafe extern void GenBuffers(uint n, uint* buffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGenerateMipmap")]
        public static extern void GenerateMipmap(uint target);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGenFramebuffers")]
        public static unsafe extern void GenFramebuffers(uint n, uint* framebuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGenRenderbuffers")]
        public static unsafe extern void GenRenderbuffers(uint n, uint* renderbuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGenTextures")]
        public static unsafe extern void GenTextures(uint n, uint* textures);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetActiveAttrib")]
        public static extern void GetActiveAttrib(uint program, uint index, uint bufSize, out uint length, out uint size, out uint type, StringBuilder name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetActiveUniform")]
        public static extern void GetActiveUniform(uint program, uint index, uint bufSize, out uint length, out uint size, out uint type, StringBuilder name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetAttachedShaders")]
        public static unsafe extern void GetAttachedShaders(uint program, uint maxCount, out uint count, uint* shaders);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetAttribLocation")]
        public static extern int GetAttribLocation(uint program, string name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetBooleanv")]
        public static extern void GetBooleanv(uint pname, out bool data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetBufferParameteriv")]
        public static unsafe extern void GetBufferParameteriv(uint target, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetError")]
        public static extern uint GetError();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetFloatv")]
        public static unsafe extern void GetFloatv(uint pname, float* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetFramebufferAttachmentParameteriv")]
        public static unsafe extern void GetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetIntegerv")]
        public static unsafe extern void GetIntegerv(uint pname, out int data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glGetIntegerv")]
        public static unsafe extern void GetIntegerv(uint pname, int* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetProgramiv")]
        public static unsafe extern void GetProgramiv(uint program, uint pname, out int paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetProgramInfoLog")]
        public static unsafe extern void GetProgramInfoLog(uint program, uint bufSize, out uint length, StringBuilder str);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetRenderbufferParameteriv")]
        public static unsafe extern void GetRenderbufferParameteriv(uint target, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetShaderiv")]
        public static extern void GetShaderiv(uint shader, uint pname, out int paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetShaderInfoLog")]
        public static unsafe extern void GetShaderInfoLog(uint shader, uint bufSize, out uint length, StringBuilder str);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetShaderPrecisionFormat")]
        public static extern void GetShaderPrecisionFormat(uint shadertype, uint precisiontype, out int range, out int precision);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetShaderSource")]
        public static unsafe extern void GetShaderSource(uint shader, uint bufSize, out uint length, string source);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetString")]
        public static unsafe extern string GetString (uint name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetTexParameterfv")]
        public static unsafe extern void GetTexParameterfv(uint target, uint pname, float* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetTexParameteriv")]
        public static extern unsafe void GetTexParameteriv(uint target, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetUniformfv")]
        public static extern unsafe void GetUniformfv(uint program, int location, float* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetUniformiv")]
        public static extern unsafe void GetUniformiv(uint program, int location, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetUniformLocation")]
        public static extern int GetUniformLocation(uint program, string name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetVertexAttribfv")]
        public static extern unsafe void GetVertexAttribfv(uint index, uint pname, float* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetVertexAttribiv")]
        public static extern unsafe void GetVertexAttribiv(uint index, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glGetVertexAttribPointerv")]
        public static extern unsafe void GetVertexAttribPointerv(uint index, uint pname, IntPtr pointer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glHint")]
        public static extern void Hint(uint target, uint mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glIsBuffer")]
        public static extern bool IsBuffer(uint buffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glIsEnabled")]
        public static extern bool IsEnabled(uint cap);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glIsFramebuffer")]
        public static extern bool IsFramebuffer(uint framebuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glIsProgram")]
        public static extern bool IsProgram(uint program);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glIsRenderbuffer")]
        public static extern bool IsRenderbuffer(uint renderbuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glIsShader")]
        public static extern bool IsShader(uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glIsTexture")]
        public static extern bool IsTexture(uint texture);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glLineWidth")]
        public static extern void LineWidth(float width);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glLinkProgram")]
        public static extern void LinkProgram(uint program);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glPixelStorei")]
        public static extern void PixelStorei(uint pname, int param);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glPolygonOffset")]
        public static extern void PolygonOffset(float factor, float units);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glReadPixels")]
        public static extern void ReadPixels(int x, int y, uint width, uint height, uint format, uint type, IntPtr pixels);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glReleaseShaderCompiler")]
        public static extern void ReleaseShaderCompiler();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glRenderbufferStorage")]
        public static extern void RenderbufferStorage(uint target, uint internalformat, uint width, uint height);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glSampleCoverage")]
        public static extern void SampleCoverage(float value, bool invert);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glScissor")]
        public static extern void Scissor(int x, int y, uint width, uint height);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glShaderBinary")]
        public static extern void ShaderBinary(uint count, out uint[] shaders, uint binaryFormat, IntPtr binary, uint length);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glShaderSource")]
        public static unsafe extern void ShaderSource(uint shader, uint count, string[] strings, int* lengths);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glStencilFunc")]
        public static extern void StencilFunc(uint func, int @ref, uint mask);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glStencilFuncSeparate")]
        public static extern void StencilFuncSeparate(uint face, uint func, int @ref, uint mask);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glStencilMask")]
        public static extern void StencilMask(uint mask);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glStencilMaskSeparate")]
        public static extern void StencilMaskSeparate(uint face, uint mask);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glStencilOp")]
        public static extern void StencilOp(uint fail, uint zfail, uint zpass);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glStencilOpSeparate")]
        public static extern void StencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glTexImage2D")]
        public static extern unsafe void TexImage2D(uint target, int level, int internalformat, uint width, uint height, int border, uint format, uint type, void* pixels);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glTexParameterf")]
        public static extern void TexParameterf(uint target, uint pname, float param);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glTexParameterfv")]
        public static extern unsafe void TexParameterfv(uint target, uint pname, float* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glTexParameteri")]
        public static extern void TexParameteri(uint target, uint pname, int param);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glTexParameteriv")]
        public static extern unsafe void TexParameteriv(uint target, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glTexSubImage2D")]
        public static extern unsafe void TexSubImage2D(uint target, int level, int xoffset, int yoffset, uint width, uint height, uint format, uint type, void* pixels);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform1f")]
        public static extern void Uniform1f(int location, float v0);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform1fv")]
        public static extern unsafe void Uniform1fv(int location, uint count, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform1i")]
        public static extern void Uniform1i(int location, int v0);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform1iv")]
        public static extern unsafe void Uniform1iv(int location, uint count, int* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform2f")]
        public static extern void Uniform2f(int location, float v0, float v1);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform2fv")]
        public static extern unsafe void Uniform2fv(int location, uint count, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform2i")]
        public static extern void Uniform2i(int location, int v0, int v1);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform2iv")]
        public static extern unsafe void Uniform2iv(int location, uint count, int* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform3f")]
        public static extern void Uniform3f(int location, float v0, float v1, float v2);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform3fv")]
        public static unsafe extern void Uniform3fv(int location, uint count, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform3i")]
        public static extern void Uniform3i(int location, int v0, int v1, int v2);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform3iv")]
        public static extern unsafe void Uniform3iv(int location, uint count, int* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform4f")]
        public static extern void Uniform4f(int location, float v0, float v1, float v2, float v3);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform4fv")]
        public static extern unsafe void Uniform4fv(int location, uint count, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform4i")]
        public static extern void Uniform4i(int location, int v0, int v1, int v2, int v3);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniform4iv")]
        public static extern unsafe void Uniform4iv(int location, uint count, int* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniformMatrix2fv")]
        public static extern unsafe void UniformMatrix2fv(int location, uint count, bool transpose, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniformMatrix3fv")]
        public static extern unsafe void UniformMatrix3fv(int location, uint count, bool transpose, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUniformMatrix4fv")]
        public static unsafe extern void UniformMatrix4fv(int location, uint count, bool transpose, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glUseProgram")]
        public static extern void UseProgram(uint program);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glValidateProgram")]
        public static extern void ValidateProgram(uint program);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib1f")]
        public static extern void VertexAttrib1f(uint index, float x);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib1fv")]
        public static extern unsafe void VertexAttrib1fv(uint index, float* v);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib2f")]
        public static extern void VertexAttrib2f(uint index, float x, float y);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib2fv")]
        public static extern unsafe void VertexAttrib2fv(uint index, float* v);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib3f")]
        public static extern void VertexAttrib3f(uint index, float x, float y, float z);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib3fv")]
        public static unsafe extern void VertexAttrib3fv(uint index, float* v);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib4f")]
        public static extern void VertexAttrib4f(uint index, float x, float y, float z, float w);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttrib4fv")]
        public static unsafe extern void VertexAttrib4fv(uint index, float* v);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glVertexAttribPointer")]
        public static extern void VertexAttribPointer(uint index, int size, uint type, bool normalized, uint stride, IntPtr pointer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint="glViewport")]
        public static extern void Viewport(int x, int y, uint width, uint height);
        #endregion
    }
}
