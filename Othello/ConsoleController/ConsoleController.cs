using AppModel;

namespace ConsoleController;

public class ConsoleController
{
	private AppFlow _app;

	public ConsoleController(AppFlow appFlow)
	{
		_app = appFlow;
	}

	public void Start()
	{
		Console.WriteLine("Create new game:" + "ng name tForHint tForBot tForTimer");
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
			if (newInput.Length == 0)
			{
				return;
			}
			string column = "";
			int row = 0;
			var splitInput = newInput.Split(" ");
			var l = splitInput.Length;
			
			switch (l)
			{
				case 1:
					if (splitInput[0] == "hint")
					{
						Hint();
					}
					if (splitInput[0] == "c")
					{
						Cancel();
					}
					break;
				case 5:
					if (splitInput[0] != "ng")
					{
						break;
					}
					var name = splitInput[1];
					var hint = splitInput[2] == "t";
					var bot = splitInput[3] == "t";
					var timer = splitInput[4] == "t";
					NewGame(name, hint, bot, timer);
					break;

					
				case 2:
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
					PassMove(row, column);
					break;
			}
		}
		catch (Exception e)
		{
			//Console.WriteLine(e);
			//throw new Exception("cannot process the input");
		}
	}
	private void PassMove(int row, string column)
	{
		_app.MakeMoveInCurrentGame(row, column);
	}
	
	private void NewGame(string name, bool hint, bool bot, bool timer)
	{
		_app.SetNewGame(name, hint, bot, timer);
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