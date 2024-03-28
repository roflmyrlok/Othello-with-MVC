using Model;

namespace Application;

public interface IAppControl
{
	public bool SetNewGame(bool autoHint, bool timer, IPlayer playerNotifyable1,
		IPlayer playerNotifyable2);
	public bool TryMakeMoveInCurrentGame(int row, string column, CellState playerToMakeAction);
	bool TryCancelLastMove(CellState playerToMakeAction);
	bool  TryGetHint(CellState playerToMakeAction);
}