using Model;

namespace AppModel;

public interface IAppControl
{
	public bool SetNewGame(bool autoHint, bool timer, IPlayerNotifyable playerNotifyable1,
		IPlayerNotifyable playerNotifyable2);
	public bool TryMakeMoveInCurrentGame(int row, string column, CellState playerToMakeAction);
	bool TryCancelLastMove(CellState playerToMakeAction);
	bool  TryGetHint(CellState playerToMakeAction);
}