using Application;
using Model;

namespace ConsoleController;

public abstract class GenericPlayer : IPlayer
{
	protected ApplicationFlow CurrentGame;
	protected CellState _currentPlayerCellState;
	
	protected GenericPlayer(CellState currentPlayerCellState, ApplicationFlow currentGame)
	{
		CurrentGame = currentGame;
		_currentPlayerCellState = currentPlayerCellState;
	}
	
	public abstract void OpponentMoveCanceled(CellState receiver);
	public abstract void OpponentMoveMaid(CellState receiver);
	protected abstract void PlayerRequestAction();
	
}