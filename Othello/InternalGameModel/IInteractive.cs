namespace Model;

public interface IInteractive
{ 
	public void MakeMove(int row, int column, bool invisible = false);
	public void View();
	public void ShowAvailableMoves();
}