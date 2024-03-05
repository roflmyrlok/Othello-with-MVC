namespace Model;

public interface IObserver
{
	public void ShowChange(GameBoard gameBoard);

	public void ShowInputError(string inputError);
}