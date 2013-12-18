
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


// Vertex attributes
#ifdef VERTEX_SHADER
attribute vec4 vert_position;
attribute vec3 vert_normal;
attribute vec2 vert_uv;
#endif