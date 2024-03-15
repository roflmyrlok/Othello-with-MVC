using System.Timers;
using Model;

namespace AppModel;
using Timer = System.Timers.Timer;

public class AppFlowPvE : AppFlow
{
	private Timer  _botMoveOnTimer	= new (new Random(DateTime.Now.Millisecond).Next(2300, 3000));
	public AppFlowPvE(IViewApp viewApp, IMoveProvider moveProvider, ICoordinatesTranslator coordinatesTranslator) : base(viewApp, moveProvider, coordinatesTranslator)
	{
		_botMoveOnTimer.AutoReset = false;
		_botMoveOnTimer.Disposed +=  StopBotTimerAfterBotMoves;
		_botMoveOnTimer.Elapsed += BotMoveOnTimerResponse;
	}
	
	private void StopBotTimerAfterBotMoves(object? sender, EventArgs e)
	{
		//if bot made move reset bot timer
		_botMoveOnTimer = new Timer();
		_botMoveOnTimer.Interval  = new Random(DateTime.Now.Millisecond).Next(2300, 3000);
		_botMoveOnTimer.AutoReset = false;
		_botMoveOnTimer.Disposed +=  StopBotTimerAfterBotMoves;
		_botMoveOnTimer.Elapsed += BotMoveOnTimerResponse;
	}

	public override void MakeMoveInCurrentGame(int row, string column)
	{
		lock (Lock) // Lock critical section
		{
			if (CurrentConfig.Timer && AutoMoveOnTimer.Enabled)
			{
				Locked = true;
			}
			Locked = false;
		}

		if (CurrentConfig.Win)
		{
			ViewApp.ShowEventWin(CurrentConfig.WCellState);
			return;
		}
		var internalColumn = this.CoordinatesTranslator.ConvertLetterToNumber(column) - 1;
		var internalRow = row - 1;
		
		if (_botMoveOnTimer.Enabled) {ViewApp.ShowEventInputDuringBotMove(); return;}

		MoveWithAiAnswer(internalRow, internalColumn);
		
		if (CurrentConfig.Timer)
		{
			AutoMoveOnTimer.Dispose();
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
		_botMoveOnTimer.Dispose();
		var aiResponseMove =
			MoveProvider.DetermineBestMove(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData());
		ViewApp.ShowEventAiMoveComing(aiResponseMove.Item1 + 1, CoordinatesTranslator.ConvertNumberToLetter(aiResponseMove.Item2 + 1));
		Thread.Sleep(500);
		CurrentGame.MakeMove(aiResponseMove.Item1, aiResponseMove.Item2);
		CurrentGameMoves.Add((aiResponseMove.Item1, aiResponseMove.Item2));
	}

	public override void CancelLastMove()
	{
		lock (Lock) // Lock critical section
		{
			if (!_botMoveOnTimer.Enabled)
			{
				ViewApp.ShowEventCannotCancelMove();
				return;
			}
			Locked = true;
		}

		if (LastMoveDateTime.AddSeconds(3) < DateTime.Now)
		{
			ViewApp.ShowEventCannotCancelMove();
		}
		ViewApp.ShowEventCancel();
		if (CurrentConfig.Timer)
		{
			AutoMoveOnTimer.Dispose();
			AutoMoveOnTimer.Start();
		}
		_botMoveOnTimer.Stop();
		_botMoveOnTimer.Dispose();
		
		lock (Lock)
		{
			Locked = false;
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

	private void MoveWithAiAnswer(int row, int column)
	{
		CurrentGame.MakeMove(row, column);
		CurrentGameMoves.Add((row, column));
		LastMoveDateTime = DateTime.Now;
		if (CurrentConfig.Timer && AutoMoveOnTimer.Enabled)
		{
			AutoMoveOnTimer.Dispose();
			lock (Lock)
			{
				Locked = false;
			}
		}
		_botMoveOnTimer.Start();
		
	}

	private void BotMoveOnTimerResponse(object? sender, ElapsedEventArgs e)
	{
		_botMoveOnTimer.Dispose();
		var responseMove =
			MoveProvider.DetermineBestMove(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData());
		ViewApp.ShowEventAiMoveComing(responseMove.Item1 + 1, CoordinatesTranslator.ConvertNumberToLetter(responseMove.Item2 + 1));
		Thread.Sleep(500);
		CurrentGame.MakeMove(responseMove.Item1, responseMove.Item2);
		CurrentGameMoves.Add((responseMove.Item1, responseMove.Item2));
		
	}
}