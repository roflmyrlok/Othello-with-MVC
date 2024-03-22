namespace AppModel;

public interface IAppControl
{ 
	void SetNewGame(bool autoHint, bool Timer);
	void MakeMoveInCurrentGame(int row, string column);
	void CancelLastMove();
	void GetHint();
}