precision highp float;

attribute vec3 aPos;
attribute vec3 aNormal;
attribute vec2 aUv;

//varying vec2 TexCoords;
varying vec3 WorldPos;
varying vec3 Normal;
varying vec2 TexCoord;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void main()
{
    //TexCoords = aTexCoords;
    WorldPos = vec3(model * vec4(aPos, 1.0));
    Normal = mat3(model) * aNormal;   
    TexCoord = aUv;

    gl_Position =  projection * view * vec4(WorldPos, 1.0);
}