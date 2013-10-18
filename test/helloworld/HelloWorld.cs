using System;
using System.IO;

using System.Drawing;

using OpenTK;
using OpenTK.Graphics;

using pxl;

class MainClass
{

    public static Mesh GetTriangle()
    {
        Mesh me = new GLMesh();
        me.vertcount = 4;

        me.positions = new OpenTK.Vector3[]
		{
			new Vector3(-1, -1, 0),
			new Vector3(1, -1, 0),
			new Vector3(1, 1, 0),
			new Vector3(-1, 1, 0)
		};

        me.normals = new OpenTK.Vector3[]
		{
			new Vector3(-1.0f,0.0f, 0.5f),
			new Vector3(1.0f, 0.0f, 0.5f),
			new Vector3(0.0f, 0.0f, 1.0f),
			new Vector3(0.0f, 0.0f, 1.0f)
		};

        me.uvs = new OpenTK.Vector2[]
		{
			new Vector2(0.0f,0.0f),
			new Vector2(1.0f, 0.0f),
			new Vector2(1.0f, 1.0f),
			new Vector2(0.0f, 1.0f)
		};

        me.colors = new OpenTK.Vector4[]
		{
			new Vector4(1, 0, 0, 1),
			new Vector4(0, 1, 0, 1),
			new Vector4(0, 0, 1, 1),
			new Vector4(1, 1, 1, 1)
		};

        //me.colors = null;

        me.triscount = 2;
        me.triangles = new uint[]
		{
			0, 1, 2,
			0, 2, 3
		};


        GLMesh glme = me as GLMesh;
        glme.Apply();

        return me;
    }

    public static Shader GetShader()
    {
        string shaderSource = File.ReadAllText("simple.glsl");
        Console.WriteLine(shaderSource);
        Shader shader = new GLShader();
        shader.source = shaderSource;

        GLTexture texture = new GLTexture();

        Bitmap img = new Bitmap("shiphull.jpg");
        texture.Copy(img);

        shader.texture = texture;
        
        shader.Apply();

        return shader;
    }

	public static int Main (string[] args)
	{
		Application app = new Application( 400, 300);
        Console.WriteLine( pxl.GLHelper.infoString );
        
        Material material = new Material();
        material.shader = GetShader();

        GameObject go = new GameObject();
        Renderer rdr = go.AddComponent<Renderer>();
        rdr.mesh     = GetTriangle();
        rdr.material = material;

        GameObject obCam  = new GameObject();
        Camera cam = obCam.AddComponent<Camera>();
        Camera.active = cam;
        cam.backgroundColor = Color4.Chocolate;
        

		app.Loop();
		app.Quit();
		return 0;
	}
}

