using AppModel;
using Model;

namespace ConsoleController;

public abstract class GenericPlayerNotifyable : IPlayerNotifyable
{
	protected AppFlow CurrentGame;
	protected CellState _currentPlayerCellState;
	
	protected GenericPlayerNotifyable(CellState currentPlayerCellState, AppFlow currentGame)
	{
		CurrentGame = currentGame;
		_currentPlayerCellState = currentPlayerCellState;
	}
	
	public abstract void OpponentMoveCanceled(CellState receiver);
	public abstract void OpponentMoveMaid(CellState receiver);
	protected abstract void PlayerRequestAction();
	
}