namespace AppModel;

public interface IAppControl
{ 
	void SetNewGame(string gameName, bool autoHint, bool vsBot, bool Timer);
	void MakeMoveInCurrentGame(int row, string column);
	void CancelLastMove();
	void GetHint();
}