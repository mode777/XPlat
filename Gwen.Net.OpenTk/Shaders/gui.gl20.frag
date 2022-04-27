#version 120

varying vec2 frag_uv;
varying vec4 frag_color;

uniform sampler2D tex;

uniform float uUseTexture;

void main(void)
{
	vec4 texColor = texture2D(tex, frag_uv);
	if (uUseTexture > 0.0)
		gl_FragColor = texColor * frag_color;
	else
		gl_FragColor = frag_color;
}