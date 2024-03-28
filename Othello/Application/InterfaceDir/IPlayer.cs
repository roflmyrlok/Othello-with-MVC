using Model;

namespace Application;

public interface IPlayer
{
	void OpponentMoveCanceled(CellState receiver);
	void OpponentMoveMaid(CellState receiver);

}