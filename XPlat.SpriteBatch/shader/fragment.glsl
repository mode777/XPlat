uniform sampler2D uTexture;

varying mediump vec2 vUv;
varying mediump vec4 vColor;

void main(void) {
  gl_FragColor = texture2D(uTexture, vUv) * vColor;
}
