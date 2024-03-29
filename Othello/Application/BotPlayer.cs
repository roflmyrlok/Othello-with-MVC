using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace Application;

public class BotPlayer : Player
{
	private IBotMoveProvider _bot;
	private Timer _timer;
	private ICoordinatesTranslator _translator;
	public BotPlayer(CellState playerCellState, ApplicationFlow currentApplicationInstance, IBotMoveProvider bot, ICoordinatesTranslator translator) : base (playerCellState, currentApplicationInstance)
	{
		_bot = bot;
		_translator = translator;
		_timer = new Timer();
		_timer.Interval = new Random().Next(2500, 3100);
		_timer.AutoReset = false;
		_timer.Elapsed += TryMakeAction;
		_timer.Disposed += TimerReset;
	}

	private void TimerReset(object? sender, EventArgs e)
	{
		_timer.Stop();
		_timer = new Timer();
		_timer.Interval = new Random().Next(2500, 3100);
		_timer.AutoReset = false;
		_timer.Elapsed += TryMakeAction;
		_timer.Disposed += TimerReset;
	}

	private void TryMakeAction(object? sender, ElapsedEventArgs e)
	{
		if (TryMakeMove())
		{
			_timer.Dispose();
			return;
		}
		_timer.Dispose();
		_timer.Start();
	}

	private (int, string) GetMove()
	{
		var tmp = this;
		var availableMoves =  CurrentApplicationInstance.TryGetAvailableMovesData(tmp);
		var gameBoard = CurrentApplicationInstance.TryGetGameBoardData(this);
		if (gameBoard == null || availableMoves == null)
		{
			throw new Exception("critical - bot no data access, how?");
		}
		var move = _bot.DetermineBestMove(gameBoard, availableMoves);
		return (move.Item1 + 1, _translator.ConvertNumberToLetter(move.Item2 + 1));
	}

	public override void OpponentMoveCanceled()
	{
		if (_timer.Enabled)
		{
			_timer.Dispose();
		}
	}

	protected override void RequestMoveAction()
	{
		if (_timer.Enabled)
		{
			_timer.Dispose();
		}
		_timer.Start();
	}

	private bool TryMakeMove()
	{
		var move = GetMove();
		var success = CurrentApplicationInstance.TryMakeMoveInCurrentGame(move.Item1, move.Item2, this);
		return success;
	}
}