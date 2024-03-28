using Model;

namespace Application;

public interface IGameProvider
{
	GameBoard? TryGetGameBoardData(CellState dataReceiver);
	List<List<bool>>? TryGetAvailableMovesData(CellState dataReceiver);
}