precision mediump float;

//attribute vec3 aPos;
attribute vec2 vertexPacked;
//attribute vec3 aNormal;

varying vec3 FragPos;
varying vec3 Normal;
varying vec4 Color;

uniform mat4 model;
uniform mat4 modelTransInv;
uniform mat4 view;
uniform mat4 projection;
uniform sampler2D texture;

vec3 unpack_pos(float v){
    float x = mod(v, 32.0);
    float y = mod(floor(v/32.0), 32.0);
    float z = mod(floor(v/1024.0), 32.0);
    return vec3(x,y,z);
}

vec3 unpack_norm(float v){
    float x = mod(v, 2.0);
    float y = mod(floor(v/2.0), 2.0);
    float z = mod(floor(v/4.0), 2.0);
    float s = mod(floor(v/8.0), 2.0) * 2.0 - 1.0;
    return vec3(x*s, y*s, z*s);
}

vec4 unpack_color(float v){
    float val = floor(v/256.0);
    vec2 coord = vec2((val+0.5)/256.0,0.5);
    return texture2D(texture, coord);
}

void main()
{
    vec3 pos = unpack_pos(vertexPacked.x);
    vec3 norm = unpack_norm(vertexPacked.y);
    FragPos = vec3(model * vec4(pos, 1.0));
    Normal = (modelTransInv * vec4(norm, 1.0)).xyz; 
    Color = unpack_color(vertexPacked.y); 
    
    gl_Position = projection * view * vec4(FragPos, 1.0);
}