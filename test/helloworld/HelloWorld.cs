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
        bf.Close(); bf = null;

        
        GameObject obcam = GameObject.Find("OBCamera");
        Camera cam = obcam.GetComponent<Camera>();
        cam.backgroundColor = Color4.Black;

        //GameObject go = GameObject.Find("OBSuzanne");
        //go.AddComponent<Rotator>();
        
        GameObject[] gos = GameObject.FindObjectsOfType<Transform>();
        foreach (var go in gos)
        {
            if( go.GetComponent<Renderer>() != null )
                go.AddComponent<Rotator>();
        }

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

