attribute vec2 aPos;
attribute vec2 aUv;
attribute vec4 aColor;

uniform vec2 uTextureSize;
uniform vec2 uViewportSize;
uniform mat4 uViewProjection;
uniform mat4 uModel;

varying vec2 vUv;
varying vec4 vColor;

void main(void) {
  vUv = aUv / uTextureSize;
  vColor = aColor;
  
  gl_Position = uViewProjection * uModel * vec4(aPos, 0.0, 1.0);
}