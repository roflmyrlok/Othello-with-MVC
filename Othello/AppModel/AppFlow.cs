using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace AppModel;

public class AppFlow : IAppControl, IView
{
	private readonly Dictionary<string, Game> _availableGames = new Dictionary<string, Game>();
	private readonly Dictionary<Game, Config> _gameConfigs = new Dictionary<Game, Config>();
	private readonly IViewApp _viewApp;
	private readonly IMoveProvider _moveProvider;
	private readonly ICoordinatesTranslator _coordinatesTranslator;
	private Game _currentGame;
	private Config _currentConfig;
	private DateTime _lastMoveDateTime;
	private Timer _autoMoveOnTimer = new Timer(20000);
	private List<(int, int)> _currentGameMoves = new List<(int, int)>();
	private Timer  _botMoveOnTimer	= new (new Random(DateTime.Now.Millisecond).Next(2300, 3000));

	public AppFlow(IViewApp viewApp, IMoveProvider moveProvider, ICoordinatesTranslator coordinatesTranslator)
	{
		_viewApp = viewApp;
		_moveProvider = moveProvider;
		_coordinatesTranslator = coordinatesTranslator;
		_currentGame = new Game(this);
		_currentConfig = new Config();
		_botMoveOnTimer.AutoReset = false;
		_autoMoveOnTimer.AutoReset = false;
		_botMoveOnTimer.Disposed +=  StopBotTimerAfterBotMoves;
		_autoMoveOnTimer.Disposed += StopAutoTurnTimerAfterAutoTurnMove;
	}

	private void StopAutoTurnTimerAfterAutoTurnMove(object? sender, EventArgs e)
	{
		//if bot made move disable cancel, reset bot timer
		_lastMoveDateTime = DateTime.Now.AddSeconds(-3);
		_autoMoveOnTimer = new Timer();
		_autoMoveOnTimer.Interval  = 20000;
		_autoMoveOnTimer.AutoReset = false;
	}

	private void StopBotTimerAfterBotMoves(object? sender, EventArgs e)
	{
		//if bot made move disable cancel, reset bot timer
		_lastMoveDateTime = DateTime.Now.AddSeconds(-3);
		_botMoveOnTimer = new Timer();
		_botMoveOnTimer.Interval = new Random(DateTime.Now.Millisecond).Next(2300, 3000);
		_botMoveOnTimer.AutoReset = false;
	}

	public void SetNewGame(string gameName, bool autoHint, bool vsBot, bool timer)
	{
		var newGame = new Game(this);
		_availableGames.Add(gameName, newGame);
		_gameConfigs.Add(newGame, new Config(autoHint, vsBot, timer));
		_currentGame = newGame;
		_currentConfig = _gameConfigs[_currentGame];
		_viewApp.ShowAvailableMoves(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData(), _currentGame.CurrentPlayer.CurrentPlayerCellState);
	}

	public void MakeMoveInCurrentGame(int row, string column)
	{
		if (_currentConfig.Win)
		{
			_viewApp.ShowEventWin(_currentConfig.WCellState);
			return;
		}
		var internalColumn = _coordinatesTranslator.ConvertLetterToNumber(column) - 1;
		var internalRow = row - 1;
		switch (_currentConfig.BotOpponent)
		{
			case false:
				new SimpleMove(_currentGame).MakeMove(internalRow, internalColumn);
				break;
			case true:
				if (_botMoveOnTimer.Enabled) {_viewApp.ShowEventInputDuringBotMove(); return;}
				
				new MoveWithAiAnswer(_moveProvider, _viewApp, _coordinatesTranslator, _currentGame, _botMoveOnTimer).MakeMove(internalRow, internalColumn);
				break;
		}

		if (_currentConfig.Timer)
		{
			_autoMoveOnTimer.Start();
			_autoMoveOnTimer.Elapsed += MakeAutoMoveOnOnTimer;
		}
		
	}
	private void MakeAutoMoveOnOnTimer(object? sender, ElapsedEventArgs e)
	{
		var responseMove =
			_moveProvider.DetermineBestMove(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData());
		_viewApp.ShowEventTimerMoveComing(responseMove.Item1, _coordinatesTranslator.ConvertNumberToLetter(responseMove.Item2));
		_currentGame.MakeMove(responseMove.Item1, responseMove.Item2);
		_lastMoveDateTime = DateTime.Now.AddSeconds(-3); //u cannot cancel move on timer
	}

	public void CancelLastMove()
	{
		if (_lastMoveDateTime.Date.AddSeconds(3) < DateTime.Now)
		{
			_viewApp.ShowEventCannotRestoreMove();
			return;
		}
		_viewApp.ShowEventCancel();
		var newGame = new Game(this);
		_currentGame = newGame;
		
		for (int i = 0; i < _currentGameMoves.Count - 1; i++)
		{
			_currentGame.MakeMove(_currentGameMoves[i].Item1, _currentGameMoves[i].Item2, true);
		}
		_botMoveOnTimer.Stop();
		_currentGameMoves = _currentGameMoves[..^1];
		_viewApp.ShowAvailableMoves(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData(), _currentGame.CurrentPlayer.CurrentPlayerCellState);
		_lastMoveDateTime = DateTime.Now.AddSeconds(-3); // cannot cancel more moves
	}

	public void GetHint()
	{
		_currentGame.ShowAvailableMoves();
	}

	public void ShowChange(GameBoard gameBoard, CellState currentPlayer)
	{
		if (_currentConfig.AutoHint)
		{
			ShowAvailableMoves(gameBoard, _currentGame.GetAvailableMovesData(), currentPlayer);
			return;
		}
		_viewApp.ShowChange(gameBoard, currentPlayer);
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer)
	{
		_viewApp.ShowAvailableMoves(gameBoard, movesMask, currentPlayer);
	}

	public void ShowEventCellOccupied(CellState currentPlayer)
	{
		_viewApp.ShowEventCellOccupied(currentPlayer);
		throw new Exception("cell occupied");
	}

	public void ShowEventWinCondition(CellState currentPlayer)
	{
		_viewApp.ShowEventWin(currentPlayer);
		_currentConfig.Win = true;
		_currentConfig.WCellState = currentPlayer;
	}
}