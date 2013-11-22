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

        if ( false )
        {
            File.WriteAllText("dna.txt", bf.DNAString);
            File.WriteAllText("data.txt", bf.fileBlockString);
        }

        bf.Close(); bf = null;

        
        GameObject obcam = GameObject.Find("OBCamera");
        Camera cam = obcam.GetComponent<Camera>();
        cam.backgroundColor = Color4.Black;

        //GameObject go = GameObject.Find("OBSuzanne");go.AddComponent<Rotator>();

        GameObject[] objects =  GameObject.instances;

        GameObject[] gos = GameObject.FindObjectsOfType<Transform>();
        
        foreach (var go in gos)
        {
            if (go.GetComponent<Renderer>() != null)
            {
                if( go.transform.parent != null && go.transform.parent == GameObject.Find("SCScene").transform )
                    go.AddComponent<Rotator>();
                Matrix4 m = go.transform.matrix;
            }
        }
        
        

    }

	public static int Main (string[] args)
	{

		Application app = new Application( 400, 300);
        Console.WriteLine( pxl.GLHelper.infoString );
        Matrix4 m = Matrix4.Translate(new Vector3(1.0f, 0.0f, 0.0f));
        Matrix4 inv = Matrix4.Transpose( Matrix4.Inverse(m) );


        BuildScene();
		app.Loop( 60.0f );
		app.Quit();
		return 0;
	}
}

