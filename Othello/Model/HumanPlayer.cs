namespace Model;

public class HumanPlayer : Player
{
	public HumanPlayer(Colour colour, CellState currentPlayerCellState) : base(colour, currentPlayerCellState)
	{
		Colour = colour;
		CurrentPlayerCellState = currentPlayerCellState;
	}
}