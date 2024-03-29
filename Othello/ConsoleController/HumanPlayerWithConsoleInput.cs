using Application;
using Model;

namespace ConsoleController;

public class HumanPlayerWithConsoleInput : Player
{
	
	public HumanPlayerWithConsoleInput(CellState playerCellState, ApplicationFlow currentApplicationInstance) : base (playerCellState, currentApplicationInstance)
	{
	}
	public override void OpponentMoveCanceled()
	{
		
	}

	protected override void RequestMoveAction()
	{
		
	}

	public bool TryMakeMove(int row, string column)
	{

		return CurrentApplicationInstance.TryMakeMoveInCurrentGame(row, column, this);
	}

	public bool TryCancelMove()
	{
		return CurrentApplicationInstance.TryCancelLastMove(this);
	}
}