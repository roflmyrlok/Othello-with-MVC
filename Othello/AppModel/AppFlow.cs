/*



not rdy for use 




*/
/*
using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace AppModel;

public abstract class AppFlow : IAppControl, IView
{
	protected readonly IViewApp ViewApp;
	protected readonly IMoveProvider MoveProvider;
	protected readonly ICoordinatesTranslator CoordinatesTranslator;
	protected Game CurrentGame;
	protected Config CurrentConfig;
	protected DateTime LastMoveDateTime;
	protected Timer AutoMoveOnTimer;
	protected List<(int, int)> CurrentGameMoves = new List<(int, int)>();


	private void StopAutoTurnTimerAfterAutoTurnMove(object? sender, EventArgs e)
	{
		//if bot made move reset bot timer
		AutoMoveOnTimer = new Timer();
		AutoMoveOnTimer.Interval  = 20000;
		AutoMoveOnTimer.AutoReset = false;
		AutoMoveOnTimer.Disposed += StopAutoTurnTimerAfterAutoTurnMove;
		AutoMoveOnTimer.Elapsed += MakeAutoMoveOnOnTimer;
	}

	public void SetNewGame(bool autoHint, bool timer)
	{
		var newGame = new Game(this);
		CurrentGame = newGame;
		CurrentConfig = new Config(autoHint, timer);
		ViewApp.ShowAvailableMoves(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData(), CurrentGame.CurrentPlayer.CurrentPlayerCellState);
	}

	public abstract void MakeMoveInCurrentGame(int row, string column);
	protected abstract void MakeAutoMoveOnOnTimer(object? sender, ElapsedEventArgs e);

	public abstract void CancelLastMove();

	public void GetHint()
	{
		CurrentGame.ShowAvailableMoves();
	}

	public void ShowChange(GameBoard gameBoard, CellState currentPlayer)
	{
		if (CurrentConfig.AutoHint)
		{
			ShowAvailableMoves(gameBoard, CurrentGame.GetAvailableMovesData(), currentPlayer);
			return;
		}
		ViewApp.ShowChange(gameBoard, currentPlayer);
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer)
	{
		ViewApp.ShowAvailableMoves(gameBoard, movesMask, currentPlayer);
	}

	public void ShowEventCellOccupied(CellState currentPlayer)
	{
		ViewApp.ShowEventCellOccupied(currentPlayer);
		throw new Exception("cell occupied");
	}

	public void ShowEventWinCondition(CellState currentPlayer)
	{
		ViewApp.ShowEventWin(currentPlayer);
		CurrentConfig.Win = true;
		CurrentConfig.WCellState = currentPlayer;
	}
}*/