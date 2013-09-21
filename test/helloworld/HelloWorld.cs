using System;
using pxl;
class MainClass
{
	public static int Main (string[] args)
	{
		Application app = new Application();
		
		app.Loop();
		app.Quit();
		
		return 0;
	}
}

