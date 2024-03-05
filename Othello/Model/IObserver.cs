namespace Model;

public interface IObserver
{
	void ShowChange(GameBoard gameBoard);
	void ShowInputError(string inputError);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask);
}