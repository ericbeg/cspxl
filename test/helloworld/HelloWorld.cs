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

    public static void BuildScene()
    {
        Shader shader = GetShader();
        Shader.fallback = shader;
        BlendFile bf = BlendFile.Open("cube.blend");

        GameObject scene = bf.Load("SCScene") as GameObject;
             

        GameObject go = bf.Load("OBCube") as GameObject;

        GameObject obcam = bf.Load("OBCamera") as GameObject;

        bf.Close(); bf = null;

        go.AddComponent<Rotator>();

        Camera cam = obcam.GetComponent<Camera>();
        //cam.perspective = false;
        //cam.scale = 4.0f;
        //cam.fovy = 0.2f*(2.0f*(float)Math.PI);
        Camera.active = cam;
        cam.backgroundColor = Color4.Black;
        //cam.near = 0.1f;
        //cam.far = 60.0f;
    }

	public static int Main (string[] args)
	{

		Application app = new Application( 400, 300);
        Console.WriteLine( pxl.GLHelper.infoString );

        BuildScene();
		app.Loop( 60.0f );
		app.Quit();
		return 0;
	}
}

