#ifdef VERTEX_SHADER

uniform mat4 modelViewProjectionMatrix;
uniform float _Time;

attribute vec4 position;
attribute vec3 normal;
attribute vec4 color;
attribute vec2 uv;

varying vec4 albedo;
varying vec3 nor;
varying vec2 frag_uv;

void main()
{
	
	vec3 offset = vec3(cos(_Time), sin(_Time), 0.0)*0.1;
	gl_Position = vec4(position.xyz + offset, 1.0);
	//gl_Position = modelViewProjectionMatrix*position;
	albedo = color;
	//albedo = vec4(1,1,1,1);
	nor = normal;
	frag_uv = uv;
	
}
#endif

#ifdef FRAGMENT_SHADER

uniform sampler2D mainTex;
uniform sampler2D secondTex;
uniform float _Time;

varying vec4 albedo;
varying vec3 nor;
varying vec2 frag_uv;

void main()
{
	vec4 col = texture2D( mainTex, frag_uv);
	vec4 col2 = texture2D( secondTex, frag_uv);
	//gl_FragColor = vec4(0.0,0.8,0.0,1.0);
	//gl_FragColor = albedo*dot( normalize(nor) , vec3(0,0,1 ) ) ;

	float b = cos(0.1*6.283*_Time)*0.5 + 0.5;

	gl_FragColor = col*(1.0-b) + col2*(b);
}
#endif

