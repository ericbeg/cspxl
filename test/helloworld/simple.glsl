#ifdef VERTEX_SHADER

uniform mat4 modelViewProjectionMatrix;
uniform float _Time;


attribute vec4 position;
attribute vec3 normal;
attribute vec4 color;


varying vec4 albedo;
varying vec3 nor;

void main()
{
	
	vec3 offset = vec3(cos(_Time), sin(_Time), 0.0)*0.8;
	gl_Position = vec4(position.xyz + offset, 1.0);
	//gl_Position = modelViewProjectionMatrix*position;
	albedo = color;
	//albedo = vec4(1,1,1,1);
	nor = normal;
	
}
#endif

#ifdef FRAGMENT_SHADER
varying vec4 albedo;
varying vec3 nor;
void main()
{
	//gl_FragColor = vec4(0.0,0.8,0.0,1.0);
	gl_FragColor = albedo*dot( normalize(nor) , vec3(0,0,1 ) ) ;
}
#endif

