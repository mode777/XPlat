attribute vec2 aPos;
attribute vec2 aUv;

varying vec2 texcoord;

void main(void) {
  texcoord = aUv; 
  gl_Position = vec4(aPos, 0.0, 1.0);
}