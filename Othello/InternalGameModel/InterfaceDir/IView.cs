namespace Model;

public interface IView
{
	void ShowChange(GameBoard gameBoard, CellState currentPlayer);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer);
	void ShowEventCellOccupied(CellState currentPlayer);
	void ShowEventWinCondition(CellState currentPlayer);

}