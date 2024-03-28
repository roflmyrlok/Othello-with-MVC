using Model;

namespace Application;

public interface IViewApp
{
	void ShowChange(GameBoard gameBoard, Player currentPlayer);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, Player currentPlayer);
	void ShowCellOccupied(Player occupiedBy);
	void ShowOnWin(Player winner);
	public void ShowTimerMoveComing(int row, string column);
	void ShowMoveMade(Player madeBy, int row, string column);
	void ShowMoveMadeAttempt(Player madeBy, int row, string column);
}