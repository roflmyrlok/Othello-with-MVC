using System.Data;
using Model;

namespace ConsoleControllerWithFlex;

public class ConsoleController
{
	private Game _game;

	public void Act(object args)
	{
		MakeMove((List<int>)args);
	}
	public ConsoleController(IObserver iobs)
	{
		_game = new Game(iobs);
		_game.SetUpNewPvPGame();
		_game.Start();
	}
	private void MakeMove(List<int> cords)
	{
		_game.MakeMove(cords[0], cords[1]);
	}
}