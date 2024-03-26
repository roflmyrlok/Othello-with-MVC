using Model;

namespace AppModel;

public interface IPlayerNotifyable
{
	void OpponentMoveCanceled(CellState receiver);
	void OpponentMoveMaid(CellState receiver);

}