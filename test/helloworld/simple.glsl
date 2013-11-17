#ifdef VERTEX_SHADER

uniform mat4 modelViewProjectionMatrix;
uniform mat4 modelViewMatrix;

//uniform mat4 modelMatrix;
//uniform mat4 viewMatrix;
//uniform mat4 projectionMatrix;

uniform mat3 normalMatrix;
//uniform float _Time;

attribute vec4 position;
attribute vec3 normal;
attribute vec2 uv;

varying vec3 nor;
varying vec2 frag_uv;

void main()
{
	nor = normalMatrix*normal;
   frag_uv = uv;
	gl_Position = modelViewProjectionMatrix*position;
}
#endif

#ifdef FRAGMENT_SHADER

uniform sampler2D mainTex;
uniform sampler2D normalTex;
uniform float _Time;

varying vec3 nor;
varying vec2 frag_uv;

void main()
{
	vec4 col = texture2D( mainTex, frag_uv);
	vec4 col2 = texture2D( normalTex, frag_uv);

	float b = cos(0.1*6.283*_Time)*0.5 + 0.5;
	//gl_FragColor = (col*(1.0-b) + col2*(b) + vec4(nor, 0.0)*0.6)*dot( nor, vec3( 0.0, 0.0, 1.0) );
	gl_FragColor = (col*(1.0-b) + col2*(b))*( dot( nor, vec3( 0.0, 0.6, 0.6) ) + 0.1);
	//gl_FragColor = col*(1.0-b) + col2*(b);
	//gl_FragColor = vec4( nor*0.5 + 0.5, 1.0);
}
#endif

