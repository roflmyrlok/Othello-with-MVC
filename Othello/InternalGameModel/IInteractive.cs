namespace Model;

public interface IInteractive
{
	
	GameBoard GetGameBoardData();
	List<List<bool>> GetAvailableMovesData();
	public void MakeMove(int row, int column, bool invisible = false);
	void GetHint();
}