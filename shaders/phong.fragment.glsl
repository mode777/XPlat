precision highp float;

varying vec3 wfn;
varying vec3 vertPos; 

uniform vec3 cameraPosition; 

const vec3 lightDirection = vec3(0.0, -1.0, -1.0);
const vec4 ambientColor = vec4(0.094, 0.0, 0.0, 1.0);
const vec4 diffuseColor = vec4(0.5, 0.0, 0.0, 1.0);
const vec4 specularColor = vec4(1.0, 1.0, 1.0, 1.0);
const float shininess = 40.0;
const vec4 lightColor = vec4(1.0, 1.0, 1.0, 1.0);

vec3 blinnPhongBRDF(vec3 lightDir, vec3 viewDir, vec3 normal, vec3 phongDiffuseCol, vec3 phongSpecularCol, float phongShininess) {
  vec3 color = phongDiffuseCol;
  vec3 halfDir = normalize(viewDir + lightDir);
  float specDot = max(dot(halfDir, normal), 0.0);
  color += pow(specDot, phongShininess) * phongSpecularCol;
  return color;
}

void main() {
  vec3 lightDir = normalize(-lightDirection);
  vec3 viewDir = normalize(cameraPosition - vertPos);
  vec3 n = normalize(wfn);

  vec3 luminance = ambientColor.rgb;
  
  float illuminance = dot(lightDir, n);
  if(illuminance > 0.0) {
    vec3 brdf = blinnPhongBRDF(lightDir, viewDir, n, diffuseColor.rgb, specularColor.rgb, shininess);
    luminance += brdf * illuminance * lightColor.rgb;
  }

  gl_FragColor = vec4(luminance, 1.0);
} 