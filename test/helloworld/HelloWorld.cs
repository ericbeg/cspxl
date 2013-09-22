using System;

using pxl;

class MainClass
{
	public static int Main (string[] args)
	{
		/*
		Application app = new Application();
		app.Loop();
		app.Quit();
		*/
		Console.WriteLine( "sizeof(float)="+ sizeof(float) );
		
		double f = 3.1415;
		
		Console.WriteLine( "IsLittleEndian" + BitConverter.IsLittleEndian );
		
		Byte[] bytes = BitConverter.GetBytes( f );
		
		foreach( Byte b in bytes )
		{
			Console.WriteLine( "[" + b + "]" );
		}
		
		return 0;
	}
}

