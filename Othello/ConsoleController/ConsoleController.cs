using AiOthelloModel;
using AppModel;

namespace ConsoleController;

public class ConsoleController
{
	private AppFlowStd _app;
	private IViewApp _viewApp;

	public ConsoleController(IViewApp viewApp)
	{
		_viewApp = viewApp;
	}

	public void Start()
	{
		while (true)
		{
			try
			{
				var input = Console.ReadLine();
				if (input == "q")
				{
					return;
				}
				ParseInput(input);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
	private void ParseInput(string newInput)
	{
		try
		{
			if (newInput.Length == 0) { return; }
			var splitInput = newInput.Split(" ");
			var l = splitInput.Length;
			
			switch (l)
			{
				case 1:
					if (splitInput[0] == "hint") {Hint();}
					if (splitInput[0] == "c") {Cancel();}
					break;
				case 4:
					if (splitInput[0] != "ng") { break; }
					var hint = splitInput[1] == "t";
					var bot = splitInput[2] == "t";
					var timer = splitInput[3] == "t";
					NewGame(hint, bot, timer);
					break;
				case 2:
					PassMove(splitInput);
					break;
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("ops, ff");
			Console.WriteLine(e.Message);
			Console.WriteLine(e);
		}
	}
	private void PassMove(string[] splitInput)
	{
		int row = -1;
		string column = "";
		
		foreach (var spl in splitInput)
		{
			var lrow = 0;
			if (!int.TryParse(spl, out lrow))
			{
				column = spl;
			}
			else
			{
				row = lrow;
			}
		}
		_app.MakeMoveInCurrentGame(row, column);
	}
	
	private void NewGame(bool hint, bool bot, bool timer)
	{
		_app = bot ? new AppFlowAgainstBot(_viewApp, new Ai(), new BoardCoordinatesInternalTranslator()) : new AppFlowStd(_viewApp, new Ai(), new BoardCoordinatesInternalTranslator());
		_app.SetNewGame(hint, timer);
	}

	private void Hint()
	{
		_app.GetHint();
	}

	private void Cancel()
	{
		_app.CancelLastMove();
	}
}