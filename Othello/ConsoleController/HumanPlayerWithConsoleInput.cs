using Application;
using Model;

namespace ConsoleController;

public class HumanPlayerWithConsoleInput : Player
{
	private ICoordinatesTranslator _translator;
	public HumanPlayerWithConsoleInput(CellState playerCellState, ApplicationFlow currentApplicationInstance, ICoordinatesTranslator translator) : base (playerCellState, currentApplicationInstance)
	{
		_translator = translator;
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