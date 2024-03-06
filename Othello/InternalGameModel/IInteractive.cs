namespace Model;

public interface IInteractive
{
	void SetUpNewGame(int columns = 8, int rows = 8);
	GameBoard GetGameBoardData();
	List<List<bool>> GetAvailableMovesData();
	void MakeMove(int row, int column);
	object Clone();
	void GetHint();
}