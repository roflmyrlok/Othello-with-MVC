namespace Model;

public interface IView
{
	void ShowField(GameBoard gameBoard, CellState currentPlayer);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer);
	void ShowEventCellOccupied(CellState currentPlayer);
	void ShowEventWinCondition(CellState currentPlayer);
	void ShowEventMoveMade(CellState player, int row, int column);

}