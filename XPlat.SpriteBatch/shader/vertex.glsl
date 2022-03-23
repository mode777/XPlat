attribute vec2 aPos;
attribute vec2 aUv;
attribute vec4 aColor;

uniform vec2 uTextureSize;
uniform vec2 uViewportSize;

varying vec2 vUv;
varying vec4 vColor;

void main(void) {
  vUv = aUv / uTextureSize;
  vColor = aColor;
  
  gl_Position = vec4(aPos.x / uViewportSize.x * 2.0 - 1.0, (1.0-(aPos.y / uViewportSize.y)) * 2.0 - 1.0, 0.0, 1.0);
}