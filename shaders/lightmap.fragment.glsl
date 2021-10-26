precision highp float;

varying vec3 WorldPos;
varying vec3 Normal;
varying vec2 TexCoord;

uniform vec3 camPos;
uniform sampler2D texture;

uniform vec3 lights;

vec3 RGBMDecode(vec4 rgbm) {
  return 6.0 * rgbm.rgb * rgbm.a;
}

void main()
{		
	vec3 t = RGBMDecode(texture2D(texture, TexCoord)) * lights;
	
	float v = (t.r + t.g + t.b) / 3.0;
   float gamma = 2.2;
   vec3 col = pow(vec3(v,v,v), vec3(1.0/gamma));
   gl_FragColor = vec4(col, 1.0);
}