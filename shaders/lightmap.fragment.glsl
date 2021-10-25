precision highp float;

varying vec3 WorldPos;
varying vec3 Normal;
varying vec2 TexCoord;

uniform vec3 camPos;
uniform sampler2D texture;

void main()
{		
   gl_FragColor = texture2D(texture, TexCoord);
}