namespace Model;

public abstract class Player
{
	public Colour Colour;
	public CellState CurrentPlayerCellState;

	protected Player(Colour colour, CellState currentPlayerCellState)
	{
		if (currentPlayerCellState == CellState.Empty)
		{
			throw new Exception("currentPlayerCellState cant be empty");
		}
		Colour = colour;
	}
}