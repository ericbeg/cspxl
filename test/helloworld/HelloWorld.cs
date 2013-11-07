using System;
using System.IO;

using System.Drawing;

//using OpenTK;
using OpenTK.Graphics;

using pxl;

public class Rotator : Behaviour
{
    public override void Update()
    {
        
        gameObject.transform.localRotation =
        Quaternion.FromAngleAxis(Time.t * 0.123f, Vector3.zAxis) *
        Quaternion.FromAngleAxis(Time.t * 0.423f, Vector3.yAxis) *
        Quaternion.FromAngleAxis(Time.t * 0.923f, Vector3.xAxis)
        ;
    }
}

class MainClass
{

    public static Shader GetShader()
    {
        string shaderSource = File.ReadAllText("simple.glsl");
        Console.WriteLine(shaderSource);
        Shader shader = new GLShader();
        shader.source = shaderSource;


        
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

        material.SetTexture("mainTex", texture0);
        material.SetTexture("secondTex", texture1);

        material.shader = GetShader();


        BlendFile bf = BlendFile.Open("cube.blend");
        GameObject go = bf.Load("OBSuzanne") as GameObject;
        bf.Close(); bf = null;

        Renderer rdr = go.GetComponent<Renderer>();
        rdr.material = material;


        go.AddComponent<Rotator>();

        GameObject obCam  = new GameObject();
        obCam.transform.position = new Vector3(0.0f, 0.0f, 2.0f);
        Camera cam = obCam.AddComponent<Camera>();
        //cam.perspective = false;
        cam.scale = 4.0f;
        cam.fovy = 0.2f*(2.0f*(float)Math.PI);
        Camera.active = cam;
        cam.backgroundColor = Color4.Black;
        cam.near = 0.1f;
        cam.far = 60.0f;

		app.Loop( 60.0f );
		app.Quit();
		return 0;
	}
}

