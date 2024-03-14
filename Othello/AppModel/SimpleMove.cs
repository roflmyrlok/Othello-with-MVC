using Model;

namespace AppModel;

public class SimpleMove : Move
{
	private Game _currentGame;
	public SimpleMove(Game currentGame)
	{
		_currentGame = currentGame;
	}
	public override void MakeMove(int row, int column)
	{
		_currentGame.MakeMove(row, column);
	}
}