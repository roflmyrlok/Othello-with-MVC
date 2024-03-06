using System.Data;
using Model;
using AiOthelloModel;

namespace ConsoleController;

public class ConsoleController
{
	private Game _game;

	public void Act(object args)
	{
		var tmp = ((int, string)) args;
		_game.MakeMove(tmp.Item1, tmp.Item2);
	}

	public ConsoleController(IView iobs, IInputErrorNotifier ierrN, bool bot = true, bool hint = true)
	{
		_game = new Game(iobs, ierrN);
		_game.SetUpNewGame(bot, hint);
	}
}