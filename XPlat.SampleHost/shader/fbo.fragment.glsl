const mediump float SAMPLES = 128.0;
const mediump float LOD = 4.0; // gaussian done on MIPmap at scale LOD
const mediump float S_LOD = exp2(LOD); // tile size = 2^LOD
const mediump float SIGMA = SAMPLES * .25;

uniform mediump sampler2D uTex;
uniform mediump vec2 uTexRes;
uniform mediump vec2 uScreenRes;

varying mediump vec2 texcoord;

mediump float gaussian(vec2 i) {
    return exp( -.5* dot(i/=SIGMA,i) ) / ( 6.28 * SIGMA*SIGMA );
}

mediump vec4 blur(sampler2D sp, mediump vec2 uv, mediump vec2 scale) {
    mediump vec4 result = vec4(0);  
    mediump float s = floor(SAMPLES/S_LOD);
    
    for (mediump float i = 0.0; i < s*s; i++ ) {
        mediump vec2 d = vec2(mod(i,s), floor(i/s))*S_LOD - SAMPLES/2.;
        //result += gaussian(d) * texture2D( sp, uv + scale * d , float(LOD) );
        result += gaussian(d) * texture2D( sp, uv + scale * d , LOD);
    }
    
    return result / result.a;
}

void main(void) {
  mediump vec4 col = blur(uTex, texcoord, 1./uScreenRes);
  gl_FragColor = col;
}
