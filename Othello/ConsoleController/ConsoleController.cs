using AiOthelloModel;
using Application;
using Model;

namespace ConsoleController;

public class ConsoleController
{
	private ApplicationFlow _application;
	private IViewApp _viewApp;
	private IPlayer _playerNotifyable1;
	private IPlayer _playerNotifyable2;

	public ConsoleController(IViewApp viewApp)
	{
		_viewApp = viewApp;
		while (true)
		{
			if (_application != null)
			{
				if (_application.GameWinner != CellState.Empty)
				{
					continue;
				}
			}
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
				Console.WriteLine("Provide input for game creation");
				var splitInput = newInput.Split(" ");
				var l = splitInput.Length;
				if (l != 4 && (splitInput[0] != "ng") )
				{
					return;
					Console.WriteLine("badinput");
				}
				var hint = splitInput[1] == "t";
				var bot = splitInput[2] == "t";
				var timer = splitInput[3] == "t";
				_application = new ApplicationFlow(_viewApp, new BoardCoordinatesInternalTranslator());
				_playerNotifyable1 = new HumanPlayer(CellState.White, _application);
				if (bot)
				{
					_playerNotifyable2 = new BotPlayer(CellState.Black, _application, new Ai());
				}
				else
				{
					_playerNotifyable2 = new HumanPlayer(CellState.Black, _application);
				}

				
				_application.SetNewGame(hint, timer, _playerNotifyable1,
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