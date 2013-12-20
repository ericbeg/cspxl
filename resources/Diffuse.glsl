
varying vec3 nor;
varying vec2 uv;


#ifdef VERTEX_SHADER
void main()
{
	gl_Position = modelViewProjectionMatrix*vert_position;
	nor = normalize( normalMatrix*vert_normal );
	uv = vert_uv;
}
#endif


#ifdef FRAGMENT_SHADER
void main()
{
	vec4 col1 = texture2D( mainTex, uv);
	vec4 col2 = texture2D( normalTex, uv);

	float b = cos(0.1*6.283*_Time)*0.5 + 0.5;
	float d = ( dot( nor, vec3( 0.0, 0.6, 0.6) ) + 0.1);
	gl_FragColor = mix(col1, col2, b)*Light.color*d;
}
#endif

