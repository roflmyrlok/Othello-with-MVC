namespace Model;

public interface IOthelloGameView
{
	void ShowField(GameBoard gameBoard, CellState currentPlayer);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer);
	void ShowCellOccupied(CellState currentPlayer);
	void ShowOnWin(CellState currentPlayer);
	void ShowMoveMade(CellState player, int row, int column);

}