using System;
using OpenTK;
using pxl;

class MainClass
{
	public static int Main (string[] args)
	{
		Application app = new Application( 400, 300);
		Mesh me = new Mesh ( );
		me.vertcount = 3;

		me.positions = new OpenTK.Vector3[3]
		{
			new Vector3(-1, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(0, 1, 0)
		};

		me.triscount = 1;
		me.triangles = new int[3]
		{
			0, 1, 2
		};


		GLMesh glme = new GLMesh ();
		glme.Create (me);

		app.Loop();
		app.Quit();
		return 0;
	}
}

