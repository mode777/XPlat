#define NR_POINT_LIGHTS 2

precision mediump float;

// inputs
varying vec3 vNormal;  
varying vec3 vFragPos;  
varying vec2 vUv;

// struct Material {
//     sampler2D diffuse;
//     sampler2D specular;
//     float shininess;
// }; 

// struct DirLight {
//     vec3 direction;
	
//     vec3 ambient;
//     vec3 diffuse;
//     vec3 specular;
// };

struct PointLight {
    vec3 position;
    vec3 color;
    float range;
    float intensity;
};

// struct SpotLight {
//     vec3 position;
//     vec3 direction;
//     float cutOff;
//     float outerCutOff;
  
//     float constant;
//     float linear;
//     float quadratic;
  
//     vec3 ambient;
//     vec3 diffuse;
//     vec3 specular;       
// };

uniform vec3 uViewPos;
//uniform DirLight dirLight;
uniform PointLight uPointLights[NR_POINT_LIGHTS];
//uniform SpotLight spotLight;

//uniform Material material;
const vec3 uAmbientColor = vec3(0.25, 0.27, 0.30);
uniform float uRoughness;
uniform float uMetallic;
uniform sampler2D uTexture;

// function prototypes
//vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(vec4 tex, PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
//vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{    
    // properties
    vec3 norm = normalize(vNormal);
    vec3 viewDir = normalize(uViewPos - vFragPos);
    
    // == =====================================================
    // Our lighting is set up in 3 phases: directional, point lights and an optional flashlight
    // For each phase, a calculate function is defined that calculates the corresponding color
    // per lamp. In the main() function we take all the calculated colors and sum them up for
    // this fragment's final color.
    // == =====================================================
    vec4 tex = texture2D(uTexture, vUv);
    vec3 result;

    // phase 1: directional lighting
    // result = CalcDirLight(dirLight, norm, viewDir);
    
    // phase 2: point lights
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalcPointLight(tex, uPointLights[i], norm, vFragPos, viewDir);    
    
    // phase 3: spot light
    // result += CalcSpotLight(spotLight, norm, FragPos, viewDir);    

    result = result + (tex.xyz * uAmbientColor);
    // vec3 gamma = vec3(1.0/2.2);
    // result = pow(linear, gamma);
    gl_FragColor = vec4(result, 1.0);
}

// calculates the color when using a directional light.
// vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
// {
//     vec3 lightDir = normalize(-light.direction);
//     // diffuse shading
//     float diff = max(dot(normal, lightDir), 0.0);
//     // specular shading
//     vec3 reflectDir = reflect(-lightDir, normal);
//     float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
//     // combine results
//     vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
//     vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
//     vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
//     return (ambient + diffuse + specular);
// }

// calculates the color when using a point light.
vec3 CalcPointLight(vec4 tex, PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    //vec3 reflectDir = reflect(-lightDir, normal);
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    float spec = pow(max(dot(normal, halfwayDir), 0.0), pow(2.0, (1.0 - uRoughness) * 8.0));
    //float spec = pow(max(dot(viewDir, reflectDir), 0.0), /*material.shininess*/32.0);
    
    // attenuation
    float distance = length(light.position - fragPos);
    //float attenuation = max( min( 1.0 - pow(distance / light.range, 4.0), 1.0), 0.0) / (distance * distance);
    float attenuation = 1.0 / (1.0 + 0.4 * pow(distance,2.0));
    //float attenuation = max( min( 1.0 - pow(distance / light.range, 2.0), 1.0), 0.0);
    //clamp(1.0 - dist*dist/(radius*radius), 0.0, 1.0);
    //float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
    
    // combine results
    //vec3 frag = vec3(tex) * light.color;
    //vec3 frag = vec3(texture2D(uTexture, vUv)) * vec3(1.0,1.0,1.0) * 10.0 * attenuation;

    //vec3 ambient = light.ambient * frag;

    vec3 diffuse = (0.2 + (1.0-uMetallic)*0.8) * diff * tex.xyz * light.color * light.intensity /* * (uRoughness * light.color)*/;
    vec3 specular = ((1.1 - uRoughness)*0.9) * spec * mix(light.color, tex.xyz * 3.0, uMetallic * 0.5) * light.intensity;

    return attenuation * (diffuse + specular);
}

// calculates the color when using a spot light.
// vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
// {
//     vec3 lightDir = normalize(light.position - fragPos);
//     // diffuse shading
//     float diff = max(dot(normal, lightDir), 0.0);
//     // specular shading
//     vec3 reflectDir = reflect(-lightDir, normal);
//     float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
//     // attenuation
//     float distance = length(light.position - fragPos);
//     float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
//     // spotlight intensity
//     float theta = dot(lightDir, normalize(-light.direction)); 
//     float epsilon = light.cutOff - light.outerCutOff;
//     float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
//     // combine results
//     vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
//     vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
//     vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
//     ambient *= attenuation * intensity;
//     diffuse *= attenuation * intensity;
//     specular *= attenuation * intensity;
//     return (ambient + diffuse + specular);
// }