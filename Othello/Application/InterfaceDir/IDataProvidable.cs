using Model;

namespace Application;

public interface IDataProvidable
{
	GameBoard? TryGetGameBoardData(Player dataReceiver);
	List<List<bool>>? TryGetAvailableMovesData(Player dataReceiver);
}