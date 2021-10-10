precision highp float;

attribute vec3 position;
attribute vec2 texcoord;
attribute vec3 normal;

uniform mat4 cameraLookAt;
uniform mat4 cameraProjection;
uniform mat4 meshTransform;
uniform mat4 meshTransformTransposedInverse;

varying vec2 tc;
varying vec3 wfn;
varying vec3 vertPos;

void main(){
  tc = texcoord;
  wfn = vec3(meshTransformTransposedInverse * vec4(normal, 0.0));
  vec4 vertPos4 = meshTransform * vec4(position, 1.0);
  vertPos = vec3(vertPos4) / vertPos4.w;
  gl_Position = cameraProjection * cameraLookAt * vertPos4;
}
    