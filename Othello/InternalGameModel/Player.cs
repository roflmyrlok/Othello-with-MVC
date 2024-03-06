namespace Model;

public class Player
{
	public CellState CurrentPlayerCellState;

	public Player(CellState currentPlayerCellState)
	{
		if (currentPlayerCellState == CellState.Empty)
		{
			throw new Exception("currentPlayerCellState cant be empty");
		}
		CurrentPlayerCellState = currentPlayerCellState;
	}
}