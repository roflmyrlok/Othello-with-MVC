namespace Model;

public interface IProvideData
{
	GameBoard GetGameBoardData();
	List<List<bool>> GetAvailableMovesData();
}