uniform sampler2D uTexture;

varying mediump vec2 vUv;
varying mediump vec4 vColor;

void main(void) {
  mediump vec4 color = texture2D(uTexture, vUv) * vColor;
  gl_FragColor = color; /*mix(vec4(1.0,0.0,1.0,1.0), color, color.a);*/
}
