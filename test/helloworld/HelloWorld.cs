using System;
using System.IO;

using System.Drawing;

using OpenTK;
using OpenTK.Graphics;

using pxl;

public class Rotator : Behaviour
{
    public override void Update()
    {
        gameObject.transform.localRotation = 
            Quaternion.FromAxisAngle(Vector3.UnitY,  Time.t)*
            Quaternion.FromAxisAngle(Vector3.UnitX,  0.3458f*Time.t);
            Quaternion.FromAxisAngle(Vector3.UnitZ,  0.2356f*Time.t);

    }
}

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


        shader.samplerNames = new string[]{"mainTex", "secondTex"};
        
        shader.Apply();

        return shader;
    }

	public static int Main (string[] args)
	{
		Application app = new Application( 400, 300);
        Console.WriteLine( pxl.GLHelper.infoString );
        
        Material material = new Material();
        GLTexture texture0 = new GLTexture();
        GLTexture texture1 = new GLTexture();

        Bitmap img0 = new Bitmap("shiphull.jpg");
        Bitmap img1 = new Bitmap("floor.jpg");

        texture0.Copy(img0);
        texture1.Copy(img1);

        material.textures = new Texture[] { texture0, texture1 };        
        material.shader = GetShader();

        GameObject go = new GameObject();
        Renderer rdr = go.AddComponent<Renderer>();
        rdr.mesh     = GetTriangle();
        rdr.material = material;

        go.AddComponent<Rotator>();

        GameObject obCam  = new GameObject();
        Camera cam = obCam.AddComponent<Camera>();
        Camera.active = cam;
        cam.backgroundColor = Color4.Chocolate;
        

		app.Loop( 20.0f );
		app.Quit();
		return 0;
	}
}

