precision mediump float;

varying vec3 Normal;  
varying vec3 FragPos;  
varying vec4 Color;  
  
uniform vec3 lightPos; 
uniform vec3 viewPos; 
uniform vec4 lightColor;
uniform vec4 objectColor;

void main()
{
    // ambient
    float ambientStrength = 0.2;
    vec3 ambient = ambientStrength * lightColor.xyz;
  	
    // diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor.xyz;
    
    // specular
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 0.5);
    vec3 specular = specularStrength * spec * lightColor.xyz;  
        
    vec3 result = (ambient + diffuse + specular) * Color.xyz;
    gl_FragColor = vec4(result, 1.0);
}