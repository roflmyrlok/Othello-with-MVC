using AiOthelloModel;
using AppModel;
using Model;

namespace ConsoleController;

public class ConsoleController
{
	private AppFlow _app;
	private IViewApp _viewApp;
	private IPlayerNotifyable _playerNotifyable1;
	private IPlayerNotifyable _playerNotifyable2;

	public ConsoleController(IViewApp viewApp)
	{
		_viewApp = viewApp;
		while (true)
		{
			try
			{
				var input = Console.ReadLine();
				if (input == "q")
				{
					break;
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
				if (l != 4 && (splitInput[0] != "ng") )
				{
					Console.WriteLine("badinput");
				}
				var hint = splitInput[1] == "t";
				var bot = splitInput[2] == "t";
				var timer = splitInput[3] == "t";
				_app = new AppFlow(_viewApp, new BoardCoordinatesInternalTranslator());
				_playerNotifyable1 = new HumanPlayerNotifyable(CellState.Player1, _app);
				if (bot)
				{
					_playerNotifyable2 = new BotPlayerNotifyable(CellState.Player2, _app, new Ai());
				}
				else
				{
					_playerNotifyable2 = new HumanPlayerNotifyable(CellState.Player2, _app);
				}

				
				_app.SetNewGame(hint, timer, _playerNotifyable1,
					_playerNotifyable2);
			}
			catch (Exception e)
			{
				Console.WriteLine("ops, ff");
				Console.WriteLine(e.Message);
				Console.WriteLine(e);
			}
		}
	}