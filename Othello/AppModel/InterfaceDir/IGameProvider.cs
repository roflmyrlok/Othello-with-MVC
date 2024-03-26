using Model;

namespace AppModel;

public interface IGameProvider
{
	GameBoard? TryGetGameBoardData(CellState dataReceiver);
	List<List<bool>>? TryGetAvailableMovesData(CellState dataReceiver);
}