#ifdef VERTEX_SHADER
attribute vec4 position;

void main()
{
	gl_Position = position;
}
#endif

#ifdef FRAGMENT_SHADER
void main()
{
	gl_FragColor = vec4(0.4,0.4,0.8,1.0);
}
#endif
