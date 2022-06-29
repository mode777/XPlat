precision mediump float;

attribute vec3 aPos;
attribute vec3 aNormal;
attribute vec2 aUv;

varying vec3 vFragPos;
varying vec3 vNormal;
varying vec2 vUv;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;
uniform mat4 uNormal;
uniform vec2 uTextureSize;

void main()
{
    vec2 uv = aUv;

    #ifdef PIXEL_SCALE_UV
    uv = uv / uTextureSize;
    #endif
    
    vUv = uv;
    vFragPos = (uModel * vec4(aPos, 1.0)).xyz;
    vNormal = (uNormal * vec4(aNormal, 1.0)).xyz;  
    
    gl_Position = uProjection * uView * vec4(vFragPos, 1.0);
}