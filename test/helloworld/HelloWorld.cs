using System;
using System.IO;
using System.Reflection;

//using OpenTK;
using OpenTK.Graphics;
using pxl;

public class Rotator : Behaviour
{
    public bool isRotating = true;

    public override void Start()
    {
        Console.WriteLine("Rotator started");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //Console.WriteLine(".");
    }
    public override void Update()
    {
        Vector2 pos = Input.mouseDeltaPosition;
        //Console.WriteLine(string.Format("({0},{1})", pos.x, pos.y));


        if (Input.IsKeyDown(Key.G))
        {
            System.GC.Collect();
            Console.WriteLine("GC");
        }


        if (Input.IsKeyDown(Key.A))
        {
            Console.WriteLine("A down");
        }

        if (Input.IsKeyPressed(Key.A))
        {
            Console.WriteLine("A pressed");
        }

        if (Input.IsKeyUp(Key.A))
        {
            Console.WriteLine("A up");
        }
        
        if (Input.IsMouseButtonDown(MouseButton.Left))
        {
            Console.WriteLine("MLB down");
        }

        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            Console.WriteLine("MLB pressed");
        }

        if (Input.IsMouseButtonUp(MouseButton.Left))
        {
            Console.WriteLine("MLB up");
        }
         

        if (Input.IsKeyDown(Key.Space))
        {
            isRotating = false;
        }
        else
        {
            isRotating = true;
        }

        if (isRotating)
        {
            gameObject.transform.localRotation =
            Quaternion.FromAngleAxis(Time.t * 0.123f, Vector3.zAxis) *
            Quaternion.FromAngleAxis(Time.t * 0.423f, Vector3.yAxis) *
            Quaternion.FromAngleAxis(Time.t * 0.923f, Vector3.xAxis)
            ;
        }


    }
}

class MainClass
{

    public static Shader GetShader()
    {
        string shaderSource = System.Text.Encoding.ASCII.GetString(global::cspxl.Properties.Resources.Shader_Diffuse);
        //string shaderSource = File.ReadAllText("simple.glsl");
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

        
        if (false)
        {
            File.WriteAllText("dna.txt", bf.DNAString);
            File.WriteAllText("data.txt", bf.fileBlockString);
        }

        bf.Close(); bf = null;


        GameObject obcam = GameObject.Find("OBCamera");
        Camera cam = obcam.GetComponent<Camera>();
        cam.backgroundColor = Color4.Black;

        //GameObject go = GameObject.Find("OBSuzanne");go.AddComponent<Rotator>();

        GameObject[] objects = GameObject.instances;

        GameObject[] gos = GameObject.FindObjectsOfType<Transform>();

        foreach (var go in gos)
        {
            if (go.GetComponent<MeshRenderer>() != null)
            {
                if (go.transform.parent != null && go.transform.parent == GameObject.Find("SCScene").transform)
                    go.AddComponent<Rotator>();
                Matrix4 m = go.transform.matrix;
            }
        }



    }

    public static int Main(string[] args)
    {


        Application app = new Application(400, 300);
        Console.WriteLine(pxl.GLHelper.infoString);

        BuildScene();

        app.Loop(70.0f);

        foreach (var o in GameObject.instances)
            o.Dispose();

        app.Quit();

        return 0;
    }
}

