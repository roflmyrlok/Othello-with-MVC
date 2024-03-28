using Model;

namespace Application;

public interface IBotMoveProvider
{
	public (int, int) DetermineBestMove(GameBoard gameBoard, List<List<bool>> am);
}