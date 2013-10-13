#ifdef VERTEX_SHADER
attribute vec4 position;
attribute vec3 normal;
attribute vec4 color;


varying vec4 albedo;
varying vec3 nor;

void main()
{
	//gl_Position = vec4(position.xyz, 1.0);
	gl_Position = position;
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

