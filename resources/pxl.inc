
// Uniforms
uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4 modelViewProjectionMatrix;
uniform mat4 modelViewMatrix;
uniform mat3 normalMatrix;

uniform vec4 color;

uniform float _Time;

uniform sampler2D mainTex;
uniform sampler2D normalTex;


// Compute light parameters ( viewDir, lightDir, ... )
// in tangent space, and pass them as varying to fragment shader
// Lighting
uniform struct
{
  vec4 position;
  vec3 direction;

  vec4 color;

  float blend;
  float cutoffCos;
  float exponent;
  float shadowBufferBias;

  float energy;
  vec4 attenuation;

  mat4 viewInvLampViewProjectionMatrix;
  bool isShadowBufferEnable;
} Light;

vec4 LightColor( vec4 position, vec3 normal, vec3 lightDir, vec3 viewDir )
{
      vec4 light;
      float d = length( position - Light.position);
      
      float att = 1.0 /
               (
                  Light.attenuation[0]
               
               +  Light.attenuation[1]*d
               +  Light.attenuation[2]*d*d
               +  Light.attenuation[3]*d*d*d
              );

      light = Light.color*att;
      return light;
}


// Vertex attributes
#ifdef VERTEX_SHADER
attribute vec4 vert_position;
attribute vec3 vert_normal;
attribute vec2 vert_uv;
#endif
