using Model;

namespace Application;

public interface IViewApp
{
	void ShowChange(GameBoard gameBoard, CellState currentPlayer);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer);
	void ShowEventCellOccupied(CellState currentPlayer);
	void ShowEventWin(CellState cellState);
	void ShowEventCannotCancelMove();
	void ShowEventCancel();
	public void ShowEventTimerMoveComing(int row, string column);
	void ShowEventMoveMade(CellState player, int row, string column);
	void ShowEventMoveMadeAttempt(CellState player, int row, string column);
}