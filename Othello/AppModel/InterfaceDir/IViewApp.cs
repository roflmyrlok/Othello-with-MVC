using Model;

namespace AppModel;

public interface IViewApp
{
	void ShowChange(GameBoard gameBoard, CellState currentPlayer);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer);
	void ShowEventCellOccupied(CellState currentPlayer);
	void ShowEventAiMoveComing(int row, string column);
	void ShowEventTimerMoveComing(int row, string column);
	void ShowEventCancel(); 
	void ShowEventWin(CellState cellState);
	void ShowEventCannotCancelMove();
	void ShowEventInputDuringBotMove();
}