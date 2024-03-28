namespace Application;
using Model;

public abstract class Player
{
	protected internal ApplicationFlow CurrentApplicationInstance;
	public CellState PlayerCellState;
	protected internal  Player? Opponent;
	
	protected Player(CellState playerCellState, ApplicationFlow currentApplicationInstance)
	{
		CurrentApplicationInstance = currentApplicationInstance;
		PlayerCellState = playerCellState;
	}

	public void ProvideOpponent(Player opponent)
	{
		Opponent = opponent;
	}

	public abstract void OpponentMoveCanceled();

	public virtual void OpponentMoveMaid()
	{
		RequestMoveAction();
	}
	protected abstract void RequestMoveAction();

}