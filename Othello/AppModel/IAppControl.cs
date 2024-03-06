namespace AppModel;

public interface IAppControl
{
	void StartNewGame(string gameName, bool autoHint, bool vsBot, bool Timer);
	void MakeMoveInCurrentGame(int row, string column);
	void CancelLastMove();
	void GetHint();
	void PickNewGame(string gameName);
}