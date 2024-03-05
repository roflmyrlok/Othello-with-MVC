namespace Model;

public interface IView
{
	void ShowChange(GameBoard gameBoard);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask);
	
	
}