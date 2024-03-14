using Model;

namespace AppModel;

public interface IMoveProvider
{
	public (int, int) DetermineBestMove(GameBoard gameBoard, List<List<bool>> am);
}