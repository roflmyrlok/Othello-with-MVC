using System.Timers;
using Model;

namespace AppModel;
using Timer = System.Timers.Timer;

public class AppFlowAgainstBot : AppFlowStd
{
	private Timer  _botMoveOnTimer	= new (new Random(DateTime.Now.Millisecond).Next(2300, 3000));
	public AppFlowAgainstBot(IViewApp viewApp, IMoveProvider moveProvider, ICoordinatesTranslator coordinatesTranslator) : base(viewApp, moveProvider, coordinatesTranslator)
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

	public override void CancelLastMove()
	{
		if (!_botMoveOnTimer.Enabled)
		{
			ViewApp.ShowEventCannotCancelMove();
			Console.WriteLine(1);
			return;
		}
		try
		{
			base.CancelLastMove();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return;
		}
		_botMoveOnTimer.Stop();
		_botMoveOnTimer.Dispose();
	}

	private void MoveWithAiAnswer(int row, int column)
	{
		CurrentGame.MakeMove(row, column);
		CurrentGameMoves.Add((row, column));
		LastMoveDateTime = DateTime.Now;
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