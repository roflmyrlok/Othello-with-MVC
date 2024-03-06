using System.Timers;
using AiOthelloModel;
using ConsoleController;
using Model;
using Timer = System.Timers.Timer;

namespace AppModel;

public class AppFlow : IAppControl
{
	private Dictionary<string, Game> _availableGames = new Dictionary<string, Game>();
	private Dictionary<Game, Config> _gameConfigs = new Dictionary<Game, Config>();
	private IView _view;
	private IViewApp _viewApp;
	private Game _currentGame;
	private Config _currentConfig;
	private Game _currentGameSave;
	private DateTime _lastMoveDateTime;
	private Timer moveTimer;

	public AppFlow(IViewApp viewApp)
	{
		_viewApp = viewApp;
		_view = new LocalGameView(viewApp);
	}
	public void StartNewGame(string gameName, bool autoHint, bool vsBot, bool timer)
	{
		var newGame = new Game(_view);
		_availableGames.Add(gameName, newGame);
		_gameConfigs.Add(newGame, new Config(autoHint, vsBot, timer));
		_currentGame = newGame;
		_currentConfig = _gameConfigs[_currentGame];
		_currentGameSave = (Game)_currentGame.Clone();
		_currentGame.SetUpNewGame();
	}

	public void MakeMoveInCurrentGame(int row, string column)
	{
		var tmpSave = _currentGame.Clone();
		var internalColumn = BoardCoordinatesInternalTranslator.ConvertLetterToNumber(column) - 1;
		var internalRow = row - 1;
		if (_currentConfig.IsBot())
		{
			_currentGame.MakeMove(internalRow, internalColumn);
			var move = Ai.DetermineBestMove(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData());
			_currentGame.MakeMove(move.Item1, move.Item2);
		}
		else
		{
			_currentGame.MakeMove(internalRow, internalColumn);
		}
		_currentGameSave = (Game)tmpSave;
		_lastMoveDateTime = DateTime.Now;
		if (_currentConfig.IsTimer())
		{
			moveTimer = new Timer(20000); // 20 seconds in milliseconds
			moveTimer.Elapsed += MakeMoveOnTimer;
		}
		moveTimer.Start();
	}

	private void MakeMoveOnTimer(object sender, ElapsedEventArgs e)
	{
		var move = Ai.DetermineBestMove(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData());
		_currentGame.MakeMove(move.Item1, move.Item2);
		_currentGameSave = _currentGame;
		_lastMoveDateTime = DateTime.Now;
	}

	public void CancelLastMove()
	{
		if (_currentConfig.IsTimer())
		{
			if (_lastMoveDateTime.Date.AddSeconds(3) <= DateTime.Now)
			{
				_currentGame = _currentGameSave;
			}
			return;
		}
		_currentGame = _currentGameSave;
	}

	public void GetHint()
	{
		_currentGame.GetHint();
	}

	public void PickNewGame(string gameName)
	{
		_currentGame = _availableGames[gameName];
		_currentConfig = _gameConfigs[_currentGame];
		_currentGameSave = (Game)_currentGame.Clone();
		_lastMoveDateTime = DateTime.Now;
	}
}