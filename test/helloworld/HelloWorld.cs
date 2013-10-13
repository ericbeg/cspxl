using System;
using System.IO;
using OpenTK;
using pxl;

class MainClass
{
	public static int Main (string[] args)
	{
		Application app = new Application( 400, 300);
        Console.WriteLine( pxl.GLHelper.infoString );

        string shaderSource = File.ReadAllText("../../simple.glsl");
        Console.WriteLine(shaderSource);
        Shader shader = new GLShader();
        shader.source = shaderSource;
        shader.Apply();
        shader.Link();
        
        
        Mesh me = new GLMesh ( );
		me.vertcount = 3;

		me.positions = new OpenTK.Vector3[3]
		{
			new Vector3(-1, -1, 0),
			new Vector3(1, -1, 0),
			new Vector3(0, 1, 0)
		};
		
		me.normals = new OpenTK.Vector3[3]
		{
			new Vector3(-1.0f,0.0f, 0.5f),
			new Vector3(1.0f, 0.0f, 0.5f),
			new Vector3(0.0f, 0.0f, 1.0f)
		};


		me.colors = new OpenTK.Vector4[3]
		{
			new Vector4(1, 0, 0, 1),
			new Vector4(0, 1, 0, 1),
			new Vector4(0, 0, 1, 1)
		};

		//me.colors = null;
		
		me.triscount = 1;
		me.triangles = new uint[3]
		{
			0, 1, 2
		};


		GLMesh glme = me as GLMesh;
		glme.Apply();

        Material material = new Material();
        material.shader = shader;

        GameObject go = new GameObject();
        Renderer rdr = go.AddComponent<Renderer>();
        rdr.mesh     = glme;
        rdr.material = material;

		app.Loop();
		app.Quit();
		return 0;
	}
}

