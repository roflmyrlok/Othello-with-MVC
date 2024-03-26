using System.Timers;
using AiOthelloModel;
using AppModel;
using Model;
using Timer = System.Timers.Timer;

namespace ConsoleController;

public class BotPlayerNotifyable : GenericPlayerNotifyable
{
	private AiOthelloModel.Ai _bot;
	private Timer _timer;
	public BotPlayerNotifyable(CellState currentPlayerCellState, AppFlow currentGame, Ai bot) : base (currentPlayerCellState, currentGame)
	{
		_bot = bot;
		_timer = new Timer();
		_timer.Interval = new Random().Next(2500, 3100);
		_timer.AutoReset = false;
		_timer.Disposed += _resetTimer;
		_timer.Elapsed += TimerMidCover;
	}

	private void _resetTimer(object? sender, EventArgs e)
	{
		_timer = new Timer();
		_timer.Interval = new Random().Next(2500, 3100);
		_timer.AutoReset = false;
		_timer.Disposed += _resetTimer;
		_timer.Elapsed += TimerMidCover;
	}
	public override void OpponentMoveMaid(CellState receiver)
	{
		if (_timer.Enabled)
		{
			_timer.Dispose();
		}
		if (receiver != this._currentPlayerCellState)
		{
			return;
		}

		_timer.Elapsed += TimerMidCover;
		_timer.Start();
	}

	public override void OpponentMoveCanceled(CellState receiver)
	{
		if (receiver != this._currentPlayerCellState)
		{
			return;
		}
		if (_timer.Enabled)
		{
			_timer.Dispose();
		}
		PlayerRequestAction();
	}

	private void TimerMidCover(object? sender, ElapsedEventArgs e)
	{
		PlayerRequestAction();
	}

	protected override void PlayerRequestAction()
	{
		var availableMoves =  CurrentGame.TryGetAvailableMovesData(this._currentPlayerCellState);
		var gameBoard = CurrentGame.TryGetGameBoardData(this._currentPlayerCellState);
		if (gameBoard == null || availableMoves == null)
		{
			throw new Exception("critical bot no data access");
		}
		var move = _bot.DetermineBestMove(gameBoard, availableMoves);
		_makeMoveOnTimer(move.Item1, CurrentGame.CoordinatesTranslator.ConvertNumberToLetter(move.Item2));
	}

	private void _makeMoveOnTimer(int row, string column)
	{
		CurrentGame.TryMakeMoveInCurrentGame(row, column, this._currentPlayerCellState);

	}
}