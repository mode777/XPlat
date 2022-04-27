#version 120

attribute vec2 in_screen_coords;
attribute vec2 in_uv;
attribute vec4 in_color;

varying vec2 frag_uv;
varying vec4 frag_color;

uniform vec2 uScreenSize;

void main(void)
{
	frag_uv = in_uv;
	frag_color = in_color;

	vec2 ndc_position = 2.0 * (in_screen_coords / uScreenSize) - 1.0;
	ndc_position.y *= -1.0;

	gl_Position = vec4(ndc_position, 0.0, 1);
}