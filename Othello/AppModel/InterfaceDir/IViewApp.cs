using Model;

namespace AppModel;

public interface IViewApp
{
	void ShowChange(GameBoard gameBoard, CellState currentPlayer);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer);
	void ShowEventCellOccupied(CellState currentPlayer);
	void ShowEventWin(CellState cellState);
	void ShowEventCannotCancelMove();
	void ShowEventCancel();
	void ShowEventMoveMade(CellState player, int row, string column);
	void ShowEventMoveMadeAttempt(CellState player, int row, string column);
}