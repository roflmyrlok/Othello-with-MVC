namespace Application;

public interface IInteractive
{
	bool TryMakeMoveInCurrentGame(int row, string column, Player playerToMakeAction);
	bool TryCancelLastMove(Player playerToMakeAction);
	bool TryGetHint(Player playerToMakeAction);
}