namespace Model;

public interface IInteractive
{ 
	public bool IsBadMove(int row, int column);
	public void MakeMove(int row, int column, bool invisible = false);
	public void ShowField();
	public void ShowAvailableMoves();
}