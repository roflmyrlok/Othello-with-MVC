namespace Model;

public class Player
{
	public Colour Colour;
	public CellState CurrentPlayerCellState;

	public Player(Colour colour, CellState currentPlayerCellState)
	{
		if (currentPlayerCellState == CellState.Empty)
		{
			throw new Exception("currentPlayerCellState cant be empty");
		}
		Colour = colour;
		CurrentPlayerCellState = currentPlayerCellState;
	}
}