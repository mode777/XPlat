precision highp float;

varying vec3 WorldPos;
varying vec3 Normal;
varying vec2 TexCoord;

uniform vec3 camPos;
uniform sampler2D texture;

uniform vec3 lights;
uniform vec4 ambient;
uniform vec4 lightColor0;
uniform vec4 lightColor1;
uniform vec4 lightColor2;

vec3 RGBMDecode(vec4 rgbm) {
  return 6.0 * rgbm.rgb * rgbm.a;
}

void main() {
  vec3 t = RGBMDecode(texture2D(texture, TexCoord));

  vec3 light0 = lightColor0.rgb * (lightColor0.a * t.r);
  vec3 light1 = lightColor1.rgb * (lightColor1.a * t.g);
  vec3 light2 = lightColor2.rgb * (lightColor2.a * t.b);

  vec3 v = (light0 + light1 + light2) / 3.0;
  float gamma = 2.2;
  vec3 col = pow(v, vec3(1.0 / gamma));
  gl_FragColor = vec4(col, 1.0) + ambient;
}