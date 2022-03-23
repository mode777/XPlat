precision mediump float;

varying vec3 vNormal;  
varying vec3 vFragPos;  
varying vec2 vUv;
  
//uniform vec3 lightPos; 
//uniform vec3 viewPos; 
//uniform vec4 lightColor;
//uniform vec4 objectColor;
uniform vec3 uLightPos;
uniform vec3 uViewPos;
uniform vec3 uLightColor;
const vec3 uAmbientColor = vec3(0.29);
//const vec3 uObjectColor = vec3(0.5,0.5,0.5);

uniform sampler2D uTexture;

void main()
{
    // ambient
    vec3 ambient = uAmbientColor;
  	
    // diffuse 
    vec3 norm = normalize(vNormal);
    vec3 lightDir = normalize(uLightPos - vFragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * uLightColor;
    
    // specular
    float specularStrength = 0.5;
    vec3 viewDir = normalize(uViewPos - vFragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 8.0);
    vec3 specular = specularStrength * spec * uLightColor;  
        
    vec3 result = (ambient + diffuse + specular) * texture2D(uTexture, vUv).rgb;
    gl_FragColor = vec4(result, 1.0);
}