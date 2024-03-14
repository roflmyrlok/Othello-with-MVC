using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace AppModel;

public class AppFlowStd : IAppControl, IView
{
	protected readonly IViewApp ViewApp;
	protected readonly IMoveProvider MoveProvider;
	protected readonly ICoordinatesTranslator CoordinatesTranslator;
	protected Game CurrentGame;
	protected Config CurrentConfig;
	protected DateTime LastMoveDateTime;
	protected Timer AutoMoveOnTimer;
	protected List<(int, int)> CurrentGameMoves = new List<(int, int)>();

	public AppFlowStd(IViewApp viewApp, IMoveProvider moveProvider, ICoordinatesTranslator coordinatesTranslator)
	{
		ViewApp = viewApp;
		MoveProvider = moveProvider;
		CoordinatesTranslator = coordinatesTranslator;
		CurrentGame = new Game(this);
		CurrentConfig = new Config();
		AutoMoveOnTimer	= new Timer(20000);
		AutoMoveOnTimer.AutoReset = false;
		AutoMoveOnTimer.Disposed += StopAutoTurnTimerAfterAutoTurnMove;
		AutoMoveOnTimer.Elapsed += MakeAutoMoveOnOnTimer;
	}

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

	public virtual void MakeMoveInCurrentGame(int row, string column)
	{
		if (CurrentConfig.Win)
		{
			ViewApp.ShowEventWin(CurrentConfig.WCellState);
			return;
		}
		var internalColumn = CoordinatesTranslator.ConvertLetterToNumber(column) - 1;
		var internalRow = row - 1;
		
		CurrentGame.MakeMove(internalRow, internalColumn);
		CurrentGameMoves.Add((internalRow, internalColumn));
		LastMoveDateTime = DateTime.Now;

		if (CurrentConfig.Timer)
		{
			AutoMoveOnTimer.Dispose();
			LastMoveDateTime = DateTime.Now;
			AutoMoveOnTimer.Start();
		}
		
	}
	protected void MakeAutoMoveOnOnTimer(object? sender, ElapsedEventArgs e)
	{
		AutoMoveOnTimer.Dispose();
		var responseMove =
			MoveProvider.DetermineBestMove(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData());
		ViewApp.ShowEventTimerMoveComing(responseMove.Item1, CoordinatesTranslator.ConvertNumberToLetter(responseMove.Item2));
		CurrentGame.MakeMove(responseMove.Item1, responseMove.Item2);
		LastMoveDateTime = DateTime.Now.AddSeconds(-3);  // cannot cancel auto move
	}

	public virtual void CancelLastMove()
	{
		if (LastMoveDateTime.AddSeconds(3) < DateTime.Now)
		{
			ViewApp.ShowEventCannotCancelMove();
			Console.WriteLine("std");
			throw new Exception("cannot cancel");
		}
		ViewApp.ShowEventCancel();
		if (CurrentConfig.Timer)
		{
			AutoMoveOnTimer.Dispose();
			AutoMoveOnTimer.Start();
		}
		var newGame = new Game(this);
		CurrentGame = newGame;

		if (CurrentGameMoves.Count == 1)
		{
			CurrentGameMoves = new List<(int, int)>();
			ViewApp.ShowAvailableMoves(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData(), CurrentGame.CurrentPlayer.CurrentPlayerCellState);
			LastMoveDateTime = DateTime.Now.AddSeconds(-3); // cannot cancel more moves
			return;
		}
		for (int i = 0; i < CurrentGameMoves.Count - 1; i++)
		{
			CurrentGame.MakeMove(CurrentGameMoves[i].Item1, CurrentGameMoves[i].Item2, true);
		}
		CurrentGameMoves = CurrentGameMoves[..^1];
		ViewApp.ShowAvailableMoves(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData(), CurrentGame.CurrentPlayer.CurrentPlayerCellState);
		LastMoveDateTime = DateTime.Now.AddSeconds(-3); // cannot cancel more moves
	}

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
}