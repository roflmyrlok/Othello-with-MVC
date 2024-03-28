namespace Model;

public interface IDataProvidable
{
	GameBoard GetGameBoardData();
	List<List<bool>> GetAvailableMovesData();
}