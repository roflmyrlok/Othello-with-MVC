using AiOthelloModel;
using Application;
using Model;

namespace ConsoleController;

public class ConsoleController
{
	private ApplicationFlow? _application;
	private readonly IViewApp _viewApp;
	private Player? _whitePlayer;
	private Player? _blackPlayer;

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

		if (newInput.Length == 0)
		{
			return;
		}
		var splitInput = newInput.Split(" ").ToList();
		var l = splitInput.Count;
		switch (l)
		{
			case 4:
				if (splitInput[0] != "ng")
				{
					throw new Exception("cant resolve");
				}
				CreateNewGame(splitInput[1..]);
				break;
			case 3:
				if (splitInput[0] == "w" || splitInput[0] == "b")
				{
					MakeMove(splitInput);
					break;
					
				}
				throw new Exception("wrong input");
				
			case 2:
				if (splitInput[0] != "w" || splitInput[0] != "b")
				{
					throw new Exception("wrong input");
				}

				if (splitInput[1] != "u")
				{
					throw new Exception("wrong input");
				}

				UndoMove(splitInput[0]);
				break;
		}
	}

	private void CreateNewGame(List<string> splitInput)
	{
		var hint = splitInput[0] == "t";
		var bot = splitInput[1] == "t";
		var timer = splitInput[2] == "t";

		_application = new ApplicationFlow(_viewApp, new BoardCoordinatesInternalTranslator(),
			new Configuration(hint, timer));

		if (bot && hint && timer)
		{
			_whitePlayer = new BotPlayer(CellState.White, _application, new SimpleBotMoveProvider(),
				new BoardCoordinatesInternalTranslator());
			_blackPlayer = new BotPlayer(CellState.Black, _application, new SimpleBotMoveProvider(),
				new BoardCoordinatesInternalTranslator());
		}
		else if (bot)
		{
			_blackPlayer = new BotPlayer(CellState.Black, _application, new SimpleBotMoveProvider(),
				new BoardCoordinatesInternalTranslator());
			_whitePlayer = new HumanPlayerWithConsoleInput(CellState.White, _application,
				new BoardCoordinatesInternalTranslator());
		}
		else
		{
			_whitePlayer = new HumanPlayerWithConsoleInput(CellState.White, _application,
				new BoardCoordinatesInternalTranslator());
			_blackPlayer = new HumanPlayerWithConsoleInput(CellState.Black, _application,
				new BoardCoordinatesInternalTranslator());
		}
		_whitePlayer.ProvideOpponent(_blackPlayer);
		_blackPlayer.ProvideOpponent(_whitePlayer);
		_application.Setup(hint, timer, _whitePlayer,
			_blackPlayer);
		Console.WriteLine("pudge");
	}

	private void MakeMove(List<string> splitInput)
	{

		HumanPlayerWithConsoleInput playerWithConsoleInput;
		try
		{
			if (splitInput[0] == "w")
			{
				playerWithConsoleInput = (HumanPlayerWithConsoleInput) _whitePlayer;
			}
			else if (splitInput[0] == "b")
			{
				playerWithConsoleInput = (HumanPlayerWithConsoleInput) _blackPlayer;
			}
			else
			{
				throw new Exception();
			}
		}
		catch (Exception e)
		{
			throw new Exception("Player is not under your control");
		}

		var row = -1;
		int.TryParse(splitInput[1], out row);
		playerWithConsoleInput.TryMakeMove(row, splitInput[2]);
	}

	private void UndoMove(string playerColour)
	{
		HumanPlayerWithConsoleInput playerWithConsoleInput;
		try
		{
			if (playerColour == "w")
			{
				playerWithConsoleInput = (HumanPlayerWithConsoleInput) _whitePlayer;
			}
			else if (playerColour == "b")
			{
				playerWithConsoleInput = (HumanPlayerWithConsoleInput) _blackPlayer;
			}
			else
			{
				throw new Exception();
			}
		}
		catch (Exception e)
		{
			throw new Exception("Player is not under your control");
		}

		playerWithConsoleInput.TryCancelMove();
	}
}