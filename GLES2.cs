using System.Runtime.InteropServices;
using System.Text;

namespace GLES2
{
    public static class GL
    {
        private const string nativeLibName = "libGLESv2";

        #region CONSTANTS

        public const uint GL_DEPTH_BUFFER_BIT = 0x00000100;
        public const uint GL_STENCIL_BUFFER_BIT = 0x00000400;
        public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
        public const uint GL_FALSE = 0;
        public const uint GL_TRUE = 1;
        public const uint GL_POINTS = 0x0000;
        public const uint GL_LINES = 0x0001;
        public const uint GL_LINE_LOOP = 0x0002;
        public const uint GL_LINE_STRIP = 0x0003;
        public const uint GL_TRIANGLES = 0x0004;
        public const uint GL_TRIANGLE_STRIP = 0x0005;
        public const uint GL_TRIANGLE_FAN = 0x0006;
        public const uint GL_ZERO = 0;
        public const uint GL_ONE = 1;
        public const uint GL_SRC_COLOR = 0x0300;
        public const uint GL_ONE_MINUS_SRC_COLOR = 0x0301;
        public const uint GL_SRC_ALPHA = 0x0302;
        public const uint GL_ONE_MINUS_SRC_ALPHA = 0x0303;
        public const uint GL_DST_ALPHA = 0x0304;
        public const uint GL_ONE_MINUS_DST_ALPHA = 0x0305;
        public const uint GL_DST_COLOR = 0x0306;
        public const uint GL_ONE_MINUS_DST_COLOR = 0x0307;
        public const uint GL_SRC_ALPHA_SATURATE = 0x0308;
        public const uint GL_FUNC_ADD = 0x8006;
        public const uint GL_BLEND_EQUATION = 0x8009;
        public const uint GL_BLEND_EQUATION_RGB = 0x8009;
        public const uint GL_BLEND_EQUATION_ALPHA = 0x883D;
        public const uint GL_FUNC_SUBTRACT = 0x800A;
        public const uint GL_FUNC_REVERSE_SUBTRACT = 0x800B;
        public const uint GL_BLEND_DST_RGB = 0x80C8;
        public const uint GL_BLEND_SRC_RGB = 0x80C9;
        public const uint GL_BLEND_DST_ALPHA = 0x80CA;
        public const uint GL_BLEND_SRC_ALPHA = 0x80CB;
        public const uint GL_CONSTANT_COLOR = 0x8001;
        public const uint GL_ONE_MINUS_CONSTANT_COLOR = 0x8002;
        public const uint GL_CONSTANT_ALPHA = 0x8003;
        public const uint GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004;
        public const uint GL_BLEND_COLOR = 0x8005;
        public const uint GL_ARRAY_BUFFER = 0x8892;
        public const uint GL_ELEMENT_ARRAY_BUFFER = 0x8893;
        public const uint GL_ARRAY_BUFFER_BINDING = 0x8894;
        public const uint GL_ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
        public const uint GL_STREAM_DRAW = 0x88E0;
        public const uint GL_STATIC_DRAW = 0x88E4;
        public const uint GL_DYNAMIC_DRAW = 0x88E8;
        public const uint GL_BUFFER_SIZE = 0x8764;
        public const uint GL_BUFFER_USAGE = 0x8765;
        public const uint GL_CURRENT_VERTEX_ATTRIB = 0x8626;
        public const uint GL_FRONT = 0x0404;
        public const uint GL_BACK = 0x0405;
        public const uint GL_FRONT_AND_BACK = 0x0408;
        public const uint GL_TEXTURE_2D = 0x0DE1;
        public const uint GL_CULL_FACE = 0x0B44;
        public const uint GL_BLEND = 0x0BE2;
        public const uint GL_DITHER = 0x0BD0;
        public const uint GL_STENCIL_TEST = 0x0B90;
        public const uint GL_DEPTH_TEST = 0x0B71;
        public const uint GL_SCISSOR_TEST = 0x0C11;
        public const uint GL_POLYGON_OFFSET_FILL = 0x8037;
        public const uint GL_SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
        public const uint GL_SAMPLE_COVERAGE = 0x80A0;
        public const uint GL_NO_ERROR = 0;
        public const uint GL_INVALID_ENUM = 0x0500;
        public const uint GL_INVALID_VALUE = 0x0501;
        public const uint GL_INVALID_OPERATION = 0x0502;
        public const uint GL_OUT_OF_MEMORY = 0x0505;
        public const uint GL_CW = 0x0900;
        public const uint GL_CCW = 0x0901;
        public const uint GL_LINE_WIDTH = 0x0B21;
        public const uint GL_ALIASED_POINT_SIZE_RANGE = 0x846D;
        public const uint GL_ALIASED_LINE_WIDTH_RANGE = 0x846E;
        public const uint GL_CULL_FACE_MODE = 0x0B45;
        public const uint GL_FRONT_FACE = 0x0B46;
        public const uint GL_DEPTH_RANGE = 0x0B70;
        public const uint GL_DEPTH_WRITEMASK = 0x0B72;
        public const uint GL_DEPTH_CLEAR_VALUE = 0x0B73;
        public const uint GL_DEPTH_FUNC = 0x0B74;
        public const uint GL_STENCIL_CLEAR_VALUE = 0x0B91;
        public const uint GL_STENCIL_FUNC = 0x0B92;
        public const uint GL_STENCIL_FAIL = 0x0B94;
        public const uint GL_STENCIL_PASS_DEPTH_FAIL = 0x0B95;
        public const uint GL_STENCIL_PASS_DEPTH_PASS = 0x0B96;
        public const uint GL_STENCIL_REF = 0x0B97;
        public const uint GL_STENCIL_VALUE_MASK = 0x0B93;
        public const uint GL_STENCIL_WRITEMASK = 0x0B98;
        public const uint GL_STENCIL_BACK_FUNC = 0x8800;
        public const uint GL_STENCIL_BACK_FAIL = 0x8801;
        public const uint GL_STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
        public const uint GL_STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
        public const uint GL_STENCIL_BACK_REF = 0x8CA3;
        public const uint GL_STENCIL_BACK_VALUE_MASK = 0x8CA4;
        public const uint GL_STENCIL_BACK_WRITEMASK = 0x8CA5;
        public const uint GL_VIEWPORT = 0x0BA2;
        public const uint GL_SCISSOR_BOX = 0x0C10;
        public const uint GL_COLOR_CLEAR_VALUE = 0x0C22;
        public const uint GL_COLOR_WRITEMASK = 0x0C23;
        public const uint GL_UNPACK_ALIGNMENT = 0x0CF5;
        public const uint GL_PACK_ALIGNMENT = 0x0D05;
        public const uint GL_MAX_TEXTURE_SIZE = 0x0D33;
        public const uint GL_MAX_VIEWPORT_DIMS = 0x0D3A;
        public const uint GL_SUBPIXEL_BITS = 0x0D50;
        public const uint GL_RED_BITS = 0x0D52;
        public const uint GL_GREEN_BITS = 0x0D53;
        public const uint GL_BLUE_BITS = 0x0D54;
        public const uint GL_ALPHA_BITS = 0x0D55;
        public const uint GL_DEPTH_BITS = 0x0D56;
        public const uint GL_STENCIL_BITS = 0x0D57;
        public const uint GL_POLYGON_OFFSET_UNITS = 0x2A00;
        public const uint GL_POLYGON_OFFSET_FACTOR = 0x8038;
        public const uint GL_TEXTURE_BINDING_2D = 0x8069;
        public const uint GL_SAMPLE_BUFFERS = 0x80A8;
        public const uint GL_SAMPLES = 0x80A9;
        public const uint GL_SAMPLE_COVERAGE_VALUE = 0x80AA;
        public const uint GL_SAMPLE_COVERAGE_INVERT = 0x80AB;
        public const uint GL_NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
        public const uint GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3;
        public const uint GL_DONT_CARE = 0x1100;
        public const uint GL_FASTEST = 0x1101;
        public const uint GL_NICEST = 0x1102;
        public const uint GL_GENERATE_MIPMAP_HINT = 0x8192;
        public const uint GL_BYTE = 0x1400;
        public const uint GL_UNSIGNED_BYTE = 0x1401;
        public const uint GL_SHORT = 0x1402;
        public const uint GL_UNSIGNED_SHORT = 0x1403;
        public const uint GL_INT = 0x1404;
        public const uint GL_UNSIGNED_INT = 0x1405;
        public const uint GL_FLOAT = 0x1406;
        public const uint GL_FIXED = 0x140C;
        public const uint GL_DEPTH_COMPONENT = 0x1902;
        public const uint GL_ALPHA = 0x1906;
        public const uint GL_RGB = 0x1907;
        public const uint GL_RGBA = 0x1908;
        public const uint GL_LUMINANCE = 0x1909;
        public const uint GL_LUMINANCE_ALPHA = 0x190A;
        public const uint GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
        public const uint GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;
        public const uint GL_UNSIGNED_SHORT_5_6_5 = 0x8363;
        public const uint GL_FRAGMENT_SHADER = 0x8B30;
        public const uint GL_VERTEX_SHADER = 0x8B31;
        public const uint GL_MAX_VERTEX_ATTRIBS = 0x8869;
        public const uint GL_MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
        public const uint GL_MAX_VARYING_VECTORS = 0x8DFC;
        public const uint GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
        public const uint GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
        public const uint GL_MAX_TEXTURE_IMAGE_UNITS = 0x8872;
        public const uint GL_MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
        public const uint GL_SHADER_TYPE = 0x8B4F;
        public const uint GL_DELETE_STATUS = 0x8B80;
        public const uint GL_LINK_STATUS = 0x8B82;
        public const uint GL_VALIDATE_STATUS = 0x8B83;
        public const uint GL_ATTACHED_SHADERS = 0x8B85;
        public const uint GL_ACTIVE_UNIFORMS = 0x8B86;
        public const uint GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
        public const uint GL_ACTIVE_ATTRIBUTES = 0x8B89;
        public const uint GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
        public const uint GL_SHADING_LANGUAGE_VERSION = 0x8B8C;
        public const uint GL_CURRENT_PROGRAM = 0x8B8D;
        public const uint GL_NEVER = 0x0200;
        public const uint GL_LESS = 0x0201;
        public const uint GL_EQUAL = 0x0202;
        public const uint GL_LEQUAL = 0x0203;
        public const uint GL_GREATER = 0x0204;
        public const uint GL_NOTEQUAL = 0x0205;
        public const uint GL_GEQUAL = 0x0206;
        public const uint GL_ALWAYS = 0x0207;
        public const uint GL_KEEP = 0x1E00;
        public const uint GL_REPLACE = 0x1E01;
        public const uint GL_INCR = 0x1E02;
        public const uint GL_DECR = 0x1E03;
        public const uint GL_INVERT = 0x150A;
        public const uint GL_INCR_WRAP = 0x8507;
        public const uint GL_DECR_WRAP = 0x8508;
        public const uint GL_VENDOR = 0x1F00;
        public const uint GL_RENDERER = 0x1F01;
        public const uint GL_VERSION = 0x1F02;
        public const uint GL_EXTENSIONS = 0x1F03;
        public const uint GL_NEAREST = 0x2600;
        public const uint GL_LINEAR = 0x2601;
        public const uint GL_NEAREST_MIPMAP_NEAREST = 0x2700;
        public const uint GL_LINEAR_MIPMAP_NEAREST = 0x2701;
        public const uint GL_NEAREST_MIPMAP_LINEAR = 0x2702;
        public const uint GL_LINEAR_MIPMAP_LINEAR = 0x2703;
        public const uint GL_TEXTURE_MAG_FILTER = 0x2800;
        public const uint GL_TEXTURE_MIN_FILTER = 0x2801;
        public const uint GL_TEXTURE_WRAP_S = 0x2802;
        public const uint GL_TEXTURE_WRAP_T = 0x2803;
        public const uint GL_TEXTURE = 0x1702;
        public const uint GL_TEXTURE_CUBE_MAP = 0x8513;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP = 0x8514;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
        public const uint GL_MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
        public const uint GL_TEXTURE0 = 0x84C0;
        public const uint GL_TEXTURE1 = 0x84C1;
        public const uint GL_TEXTURE2 = 0x84C2;
        public const uint GL_TEXTURE3 = 0x84C3;
        public const uint GL_TEXTURE4 = 0x84C4;
        public const uint GL_TEXTURE5 = 0x84C5;
        public const uint GL_TEXTURE6 = 0x84C6;
        public const uint GL_TEXTURE7 = 0x84C7;
        public const uint GL_TEXTURE8 = 0x84C8;
        public const uint GL_TEXTURE9 = 0x84C9;
        public const uint GL_TEXTURE10 = 0x84CA;
        public const uint GL_TEXTURE11 = 0x84CB;
        public const uint GL_TEXTURE12 = 0x84CC;
        public const uint GL_TEXTURE13 = 0x84CD;
        public const uint GL_TEXTURE14 = 0x84CE;
        public const uint GL_TEXTURE15 = 0x84CF;
        public const uint GL_TEXTURE16 = 0x84D0;
        public const uint GL_TEXTURE17 = 0x84D1;
        public const uint GL_TEXTURE18 = 0x84D2;
        public const uint GL_TEXTURE19 = 0x84D3;
        public const uint GL_TEXTURE20 = 0x84D4;
        public const uint GL_TEXTURE21 = 0x84D5;
        public const uint GL_TEXTURE22 = 0x84D6;
        public const uint GL_TEXTURE23 = 0x84D7;
        public const uint GL_TEXTURE24 = 0x84D8;
        public const uint GL_TEXTURE25 = 0x84D9;
        public const uint GL_TEXTURE26 = 0x84DA;
        public const uint GL_TEXTURE27 = 0x84DB;
        public const uint GL_TEXTURE28 = 0x84DC;
        public const uint GL_TEXTURE29 = 0x84DD;
        public const uint GL_TEXTURE30 = 0x84DE;
        public const uint GL_TEXTURE31 = 0x84DF;
        public const uint GL_ACTIVE_TEXTURE = 0x84E0;
        public const uint GL_REPEAT = 0x2901;
        public const uint GL_CLAMP_TO_EDGE = 0x812F;
        public const uint GL_MIRRORED_REPEAT = 0x8370;
        public const uint GL_FLOAT_VEC2 = 0x8B50;
        public const uint GL_FLOAT_VEC3 = 0x8B51;
        public const uint GL_FLOAT_VEC4 = 0x8B52;
        public const uint GL_INT_VEC2 = 0x8B53;
        public const uint GL_INT_VEC3 = 0x8B54;
        public const uint GL_INT_VEC4 = 0x8B55;
        public const uint GL_BOOL = 0x8B56;
        public const uint GL_BOOL_VEC2 = 0x8B57;
        public const uint GL_BOOL_VEC3 = 0x8B58;
        public const uint GL_BOOL_VEC4 = 0x8B59;
        public const uint GL_FLOAT_MAT2 = 0x8B5A;
        public const uint GL_FLOAT_MAT3 = 0x8B5B;
        public const uint GL_FLOAT_MAT4 = 0x8B5C;
        public const uint GL_SAMPLER_2D = 0x8B5E;
        public const uint GL_SAMPLER_CUBE = 0x8B60;
        public const uint GL_VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
        public const uint GL_VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
        public const uint GL_VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
        public const uint GL_VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
        public const uint GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
        public const uint GL_VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
        public const uint GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
        public const uint GL_IMPLEMENTATION_COLOR_READ_TYPE = 0x8B9A;
        public const uint GL_IMPLEMENTATION_COLOR_READ_FORMAT = 0x8B9B;
        public const uint GL_COMPILE_STATUS = 0x8B81;
        public const uint GL_INFO_LOG_LENGTH = 0x8B84;
        public const uint GL_SHADER_SOURCE_LENGTH = 0x8B88;
        public const uint GL_SHADER_COMPILER = 0x8DFA;
        public const uint GL_SHADER_BINARY_FORMATS = 0x8DF8;
        public const uint GL_NUM_SHADER_BINARY_FORMATS = 0x8DF9;
        public const uint GL_LOW_FLOAT = 0x8DF0;
        public const uint GL_MEDIUM_FLOAT = 0x8DF1;
        public const uint GL_HIGH_FLOAT = 0x8DF2;
        public const uint GL_LOW_INT = 0x8DF3;
        public const uint GL_MEDIUM_INT = 0x8DF4;
        public const uint GL_HIGH_INT = 0x8DF5;
        public const uint GL_FRAMEBUFFER = 0x8D40;
        public const uint GL_RENDERBUFFER = 0x8D41;
        public const uint GL_RGBA4 = 0x8056;
        public const uint GL_RGB5_A1 = 0x8057;
        public const uint GL_RGB565 = 0x8D62;
        public const uint GL_DEPTH_COMPONENT16 = 0x81A5;
        public const uint GL_STENCIL_INDEX8 = 0x8D48;
        public const uint GL_RENDERBUFFER_WIDTH = 0x8D42;
        public const uint GL_RENDERBUFFER_HEIGHT = 0x8D43;
        public const uint GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
        public const uint GL_RENDERBUFFER_RED_SIZE = 0x8D50;
        public const uint GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
        public const uint GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
        public const uint GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
        public const uint GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
        public const uint GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
        public const uint GL_COLOR_ATTACHMENT0 = 0x8CE0;
        public const uint GL_DEPTH_ATTACHMENT = 0x8D00;
        public const uint GL_STENCIL_ATTACHMENT = 0x8D20;
        public const uint GL_NONE = 0;
        public const uint GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS = 0x8CD9;
        public const uint GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
        public const uint GL_FRAMEBUFFER_BINDING = 0x8CA6;
        public const uint GL_RENDERBUFFER_BINDING = 0x8CA7;
        public const uint GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
        #endregion

        #region API
        public const uint GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glActiveTexture(uint texture);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glAttachShader(uint program, uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBindAttribLocation(uint program, uint index, string name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBindBuffer(uint target, uint buffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBindFramebuffer(uint target, uint framebuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBindRenderbuffer(uint target, uint renderbuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBindTexture(uint target, uint texture);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBlendColor(float red, float green, float blue, float alpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBlendEquation(uint mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBlendEquationSeparate(uint modeRGB, uint modeAlpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBlendFunc(uint sfactor, uint dfactor);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glBufferData(uint target, uint size, void* data, uint usage);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glBufferSubData(uint target, uint offset, uint size, IntPtr data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint glCheckFramebufferStatus(uint target);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glClear(uint mask);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glClearColor(float red, float green, float blue, float alpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glClearDepthf(float d);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glClearStencil(int s);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glColorMask(bool red, bool green, bool blue, bool alpha);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glCompileShader(uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glCompressedTexImage2D(uint target, int level, uint internalformat, uint width, uint height, int border, uint imageSize, void* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, uint width, uint height, uint format, uint imageSize, void* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, uint width, uint height, int border);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, uint width, uint height);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint glCreateProgram();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint glCreateShader(uint type);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glCullFace(uint mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glDeleteBuffers(uint n, uint* buffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glDeleteFramebuffers(uint n, uint* framebuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDeleteProgram(uint program);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glDeleteRenderbuffers(uint n, uint* renderbuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDeleteShader(uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glDeleteTextures(uint n, uint* textures);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDepthFunc(uint func);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDepthMask(bool flag);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDepthRangef(float n, float f);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDetachShader(uint program, uint shader);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDisable(uint cap);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDisableVertexAttribArray(uint index);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glDrawArrays(uint mode, int first, uint count);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glDrawElements(uint mode, uint count, uint type, IntPtr indices);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glEnable(uint cap);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glEnableVertexAttribArray(uint index);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glFinish();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glFlush();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glFrontFace(uint mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGenBuffers(uint n, uint* buffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glGenerateMipmap(uint target);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGenFramebuffers(uint n, uint* framebuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGenRenderbuffers(uint n, uint* renderbuffers);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGenTextures(uint n, uint* textures);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glGetActiveAttrib(uint program, uint index, uint bufSize, out uint length, out uint size, out uint type, StringBuilder name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glGetActiveUniform(uint program, uint index, uint bufSize, out uint length, out uint size, out uint type, StringBuilder name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetAttachedShaders(uint program, uint maxCount, out uint count, uint* shaders);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int glGetAttribLocation(uint program, string name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glGetBooleanv(uint pname, out bool data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetBufferParameteriv(uint target, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint glGetError();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetFloatv(uint pname, float* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetIntegerv(uint pname, int* data);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetProgramiv(uint program, uint pname, out int paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetProgramInfoLog(uint program, uint bufSize, out uint length, StringBuilder str);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetRenderbufferParameteriv(uint target, uint pname, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glGetShaderiv(uint shader, uint pname, out int paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetShaderInfoLog(uint shader, uint bufSize, out uint length, StringBuilder str);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, out int range, out int precision);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetShaderSource(uint shader, uint bufSize, out uint length, string source);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern string glGetString (uint name);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glGetTexParameterfv(uint target, uint pname, float* paras);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glGetTexParameteriv(uint target, uint pname, int* paras);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glGetUniformfv(uint program, int location, float*params);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glGetUniformiv(uint program, int location, int* paras);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int glGetUniformLocation(uint program, string name);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glGetVertexAttribfv(uint index, uint pname, float*params);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glGetVertexAttribiv(uint index, uint pname, int* paras);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glGetVertexAttribPointerv(uint index, uint pname, IntPtr* pointer);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glHint(uint target, uint mode);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool glIsBuffer(uint buffer);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool glIsEnabled(uint cap);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool glIsFramebuffer(uint framebuffer);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool glIsProgram(uint program);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool glIsRenderbuffer(uint renderbuffer);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool glIsShader(uint shader);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool glIsTexture(uint texture);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glLineWidth(float width);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glLinkProgram(uint program);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glPixelStorei(uint pname, int param);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glPolygonOffset(float factor, float units);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glReadPixels(int x, int y, uint width, uint height, uint format, uint type, IntPtr pixels);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glReleaseShaderCompiler(void);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glRenderbufferStorage(uint target, uint internalformat, uint width, uint height);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glSampleCoverage(float value, bool invert);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glScissor(int x, int y, uint width, uint height);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glShaderBinary(uint count, const out uint[] shaders, uint binaryFormat, const IntPtr binary, uint length);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glShaderSource(uint shader, uint count, string[] strings, int* lengths);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glStencilFunc(uint func, int ref, uint mask);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glStencilFuncSeparate(uint face, uint func, int ref, uint mask);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glStencilMask(uint mask);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glStencilMaskSeparate(uint face, uint mask);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glStencilOp(uint fail, uint zfail, uint zpass);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glStencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glTexImage2D(uint target, int level, int internalformat, uint width, uint height, int border, uint format, uint type, const IntPtr pixels);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glTexParameterf(uint target, uint pname, float param);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glTexParameterfv(uint target, uint pname, const float*params);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glTexParameteri(uint target, uint pname, int param);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glTexParameteriv(uint target, uint pname, const int* paras);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, uint width, uint height, uint format, uint type, const IntPtr pixels);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform1f(int location, float v0);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform1fv(int location, uint count, const float* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform1i(int location, int v0);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform1iv(int location, uint count, const int* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform2f(int location, float v0, float v1);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform2fv(int location, uint count, const float* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform2i(int location, int v0, int v1);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform2iv(int location, uint count, const int* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glUniform3f(int location, float v0, float v1, float v2);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glUniform3fv(int location, uint count, float* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform3i(int location, int v0, int v1, int v2);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform3iv(int location, uint count, const int* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform4f(int location, float v0, float v1, float v2, float v3);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform4fv(int location, uint count, const float* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform4i(int location, int v0, int v1, int v2, int v3);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniform4iv(int location, uint count, const int* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniformMatrix2fv(int location, uint count, bool transpose, const float* value);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glUniformMatrix3fv(int location, uint count, bool transpose, const float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void glUniformMatrix4fv(int location, uint count, bool transpose, float* value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glUseProgram(uint program);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glValidateProgram(uint program);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib1f(uint index, float x);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib1fv(uint index, const float* v);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib2f(uint index, float x, float y);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib2fv(uint index, const float* v);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib3f(uint index, float x, float y, float z);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib3fv(uint index, const float* v);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib4f(uint index, float x, float y, float z, float w);
        // [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void glVertexAttrib4fv(uint index, const float* v);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glVertexAttribPointer(uint index, int size, uint type, bool normalized, uint stride, IntPtr pointer);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void glViewport(int x, int y, uint width, uint height);
        #endregion
    }
}
