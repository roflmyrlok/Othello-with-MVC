using System.Data;
using Model;

namespace ConsoleController;

public class ConsoleController
{
	private Game _game;

	public void Act(object args)
	{
		var tmp = ((int, int)) args;
		MakeMove(tmp);
	}
	public ConsoleController(IView iobs, IInputErrorNotifier ierrN)
	{
		_game = new Game(iobs, ierrN);
		_game.SetUpNewPvPGame();
		_game.Start();
	}
	private void MakeMove((int,int) t)
	{
		_game.MakeMove(t.Item1, t.Item2);
	}
}