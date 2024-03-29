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
				if (splitInput[1] != "u" && splitInput[1] != "hint")
				{
					throw new Exception("wrong input");
				}
				if (splitInput[0] == "w" || splitInput[0] == "b")
				{
					if (splitInput[1] == "hint")
					{
						Hint(splitInput[0]);
					}
					if (splitInput[1] == "u")
					{
						UndoMove(splitInput[0]);
					}
				}
				break;
		}
	}

	private void CreateNewGame(List<string> splitInput)
	{
		if (_application is not null)
		{
			//wont work if 2 bots playing game, fix? unknown
			_application = null;
		}
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
			_whitePlayer = new HumanPlayerWithConsoleInput(CellState.White, _application);
		}
		else
		{
			_whitePlayer = new HumanPlayerWithConsoleInput(CellState.White, _application);
			_blackPlayer = new HumanPlayerWithConsoleInput(CellState.Black, _application);
		}
		_whitePlayer.ProvideOpponent(_blackPlayer);
		_blackPlayer.ProvideOpponent(_whitePlayer);
		_application.Setup(hint, timer, _whitePlayer,
			_blackPlayer);
	}

	private void MakeMove(List<string> splitInput)
	{
		int.TryParse(splitInput[1], out var row);
		switch (splitInput[0])
		{
			case "w" when _whitePlayer is HumanPlayerWithConsoleInput player:
				player.TryMakeMove(row, splitInput[2]);
				break;
			case "b" when _blackPlayer is HumanPlayerWithConsoleInput player:
				player.TryMakeMove(row, splitInput[2]);
				break;
		}
	}

	private void UndoMove(string playerColour)
	{
		switch (playerColour)
		{
			case "w" when _whitePlayer is HumanPlayerWithConsoleInput player:
				player.TryCancelMove();
				break;
			case "b" when _blackPlayer is HumanPlayerWithConsoleInput player:
				player.TryCancelMove();
				break;
		}
	}

	private void Hint(string playerColour)
	{
		switch (playerColour)
		{
			case "w" when _whitePlayer is HumanPlayerWithConsoleInput player:
				player.TryGetHint();
				break;
			case "b" when _blackPlayer is HumanPlayerWithConsoleInput player:
				player.TryGetHint();
				break;
		}
	}
}