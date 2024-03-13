namespace Model;

public interface IInteractive
{
	void SetUpNewGame(int columns = 8, int rows = 8);
	GameBoard GetGameBoardData();
	List<List<bool>> GetAvailableMovesData();
	public void MakeMove(int row, int column, bool invisible = false);
	void GetHint();
}