
const float PI = 3.14159265359;

float distributionGGX (vec3 N, vec3 H, float roughness){
    float a2    = roughness * roughness * roughness * roughness;
    float NdotH = max (dot (N, H), 0.0);
    float denom = (NdotH * NdotH * (a2 - 1.0) + 1.0);
    return a2 / (PI * denom * denom);
}

float geometrySchlickGGX (float NdotV, float roughness){
    float r = (roughness + 1.0);
    float k = (r * r) / 8.0;
    return NdotV / (NdotV * (1.0 - k) + k);
}

float geometrySmith (vec3 N, vec3 V, vec3 L, float roughness){
    return geometrySchlickGGX (max (dot (N, L), 0.0), roughness) * 
           geometrySchlickGGX (max (dot (N, V), 0.0), roughness);
}

vec3 fresnelSchlick (float cosTheta, vec3 F0){
    return F0 + (1.0 - F0) * pow (1.0 - cosTheta, 5.0);
}

// https://www.shadertoy.com/view/4d2XWV by Inigo Quilez
float sphereIntersect(vec3 ro, vec3 rd, vec4 sph) {
	vec3 oc = ro - sph.xyz;
	float b = dot( oc, rd );
	float c = dot( oc, oc ) - sph.w*sph.w;
	float h = b*b - c;
	if( h<0.0 ) return -1.0;
	return -b - sqrt( h );
}

void main (){
    vec3 lightPos   = vec3(1.25, 1.0, -2);
    vec3 lightColor = vec3(1.0);
    vec3 totColor = vec3(0.0);
    
    vec3 ro = vec3(0.0, 0.0, -2.0);

    for (float x = 0.0; x <= 1.0; x += 1.) {
        for (float y = 0.0; y <= 1.0; y += 1.) {
            
            vec3 rd = normalize(vec3(vScreen + vec2(x, y) / iResolution.y, 1.2));
            float d = sphereIntersect(ro, rd, vec4(0,0,0,1));
            
            if (d > 0.) {
                vec3 worldPos = ro + d * rd;
                vec3 N = normalize (worldPos);
                vec3 V = -rd;
                vec3 L = normalize (lightPos - worldPos);
                vec3 H = normalize (V + L);
                
                // Cook-Torrance BRDF
                vec3  F0 = mix (vec3 (0.04), pow(albedo, vec3 (2.2)), metallic);
                float NDF = distributionGGX(N, H, roughness);
                float G   = geometrySmith(N, V, L, roughness);
                vec3  F   = fresnelSchlick(max(dot(H, V), 0.0), F0);        
                vec3  kD  = vec3(1.0) - F;
                kD *= 1.0 - metallic;	  
                
                vec3  numerator   = NDF * G * F;
                float denominator = 4.0 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0);
                vec3  specular    = numerator / max(denominator, 0.001);  
                    
                float NdotL = max(dot(N, L), 0.0);                
                vec3  color = lightColor * (kD * pow(albedo, vec3 (2.2)) / PI + specular) * 
                              (NdotL / dot(lightPos - worldPos, lightPos - worldPos));
                
                totColor += color;
            }
        }
    }
    
    // HDR tonemapping gamma correct
    fragColor = vec4(pow(totColor/(totColor + 1.0), vec3 (1.0/2.2)), 1.0);
}