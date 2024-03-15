using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace AppModel;

public class AppFlowPvP : AppFlow
{
	public AppFlowPvP(IViewApp viewApp, IMoveProvider moveProvider, ICoordinatesTranslator coordinatesTranslator) : base(viewApp, moveProvider, coordinatesTranslator)
	{
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

	public override void MakeMoveInCurrentGame(int row, string column)
	{
		lock (Lock) // Lock critical section
		{
			if (CurrentConfig.Timer && AutoMoveOnTimer.Enabled)
			{
				Locked = true;
			}		
		}

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
		
		if (CurrentConfig.Timer && AutoMoveOnTimer.Enabled)
		{
			AutoMoveOnTimer.Dispose();
			Locked = false;
			LastMoveDateTime = DateTime.Now;
			AutoMoveOnTimer.Start();
		}
		
	}
	protected override void MakeAutoMoveOnOnTimer(object? sender, ElapsedEventArgs e)
	{
		AutoMoveOnTimer.Dispose();
		var responseMove =
			MoveProvider.DetermineBestMove(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData());
		ViewApp.ShowEventTimerMoveComing(responseMove.Item1, CoordinatesTranslator.ConvertNumberToLetter(responseMove.Item2));
		CurrentGame.MakeMove(responseMove.Item1, responseMove.Item2);
		CurrentGameMoves.Add(responseMove);
		LastMoveDateTime = DateTime.Now.AddSeconds(-3);  // cannot cancel auto move
	}

	public override void CancelLastMove()
	{
		lock (Lock) // Lock critical section
		{
			if (CurrentConfig.Timer && AutoMoveOnTimer.Enabled)
			{
				Locked = true;
			}		
		}
		if (LastMoveDateTime.AddSeconds(3) < DateTime.Now)
		{
			ViewApp.ShowEventCannotCancelMove();
			Console.WriteLine("std");
			throw new Exception("cannot cancel");
		}
		ViewApp.ShowEventCancel();
		
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
		if (CurrentConfig.Timer && AutoMoveOnTimer.Enabled)
		{
			AutoMoveOnTimer.Dispose();
			Locked = false;
			LastMoveDateTime = DateTime.Now;
			AutoMoveOnTimer.Start();
		}
	}
}