using System.Data;
using AppModel;
using AiOthelloModel;

namespace ConsoleController;

public class ConsoleController
{
	private AppFlow _app;

	public ConsoleController(IViewApp viewApp)
	{
		_app = new AppFlow(viewApp);
	}

	public void Start()
	{
		_app.StartNewGame("pudge", true, true, true);
		while (true)
		{
			(int, string) arg; ;
			try
			{
				var input = Console.ReadLine();
				arg = InputHandler.ParseInput(input);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			PassMove(arg);
		}
	}
	
	private void PassMove(object args)
	{
		var tmp = ((int, string)) args;
		_app.MakeMoveInCurrentGame(tmp.Item1, tmp.Item2);
	}
}