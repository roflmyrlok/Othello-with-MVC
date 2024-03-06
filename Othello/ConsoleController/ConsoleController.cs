using System.Data;
using Model;
using AiOthelloModel;

namespace ConsoleController;

public class ConsoleController
{
	private Game _game;
	private int _gameMode;

	public void Act(object args)
	{
		var tmp = ((int, int)) args;
		
		if (_gameMode == 0)
		{
			_game.MakeMoveWithAiAnswer(tmp.Item1, tmp.Item2);
		}
		else
		{
			_game.MakeMove(tmp.Item1, tmp.Item2);
		}
		
	}

	public ConsoleController(IView iobs, IInputErrorNotifier ierrN, int gameMode = 0)
	{
		_game = new Game(iobs, ierrN);
		_gameMode = gameMode;
		_game.SetUpNewGame();
		_game.Start();
	}
}