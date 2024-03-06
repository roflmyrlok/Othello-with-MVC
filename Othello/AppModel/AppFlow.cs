using System.Timers;
using AiOthelloModel;
using ConsoleController;
using Model;
using Timer = System.Timers.Timer;

namespace AppModel;

public class AppFlow : IAppControl, IView
{
	private Dictionary<string, Game> _availableGames = new Dictionary<string, Game>();
	private Dictionary<Game, Config> _gameConfigs = new Dictionary<Game, Config>();
	private IViewApp _viewApp;
	private Game _currentGame;
	private Config _currentConfig;
	private Game _currentGameSave;
	private DateTime _lastMoveDateTime;
	private Timer _moveTimer = new Timer(20000);

	public AppFlow(IViewApp viewApp)
	{
		_viewApp = viewApp;
	}
	public void StartNewGame(string gameName, bool autoHint, bool vsBot, bool timer)
	{
		var newGame = new Game(this);
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
			Thread.Sleep(new Random(DateTime.Now.Millisecond).Next(1400, 2300));
			_viewApp.ShowEventAiMoveComing(move.Item1 + 1, BoardCoordinatesInternalTranslator.ConvertNumberToLetter(move.Item2 + 1));
			Thread.Sleep(new Random(DateTime.Now.Millisecond).Next(2000, 3000));
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
			_moveTimer.Stop();
			_moveTimer.EndInit();
			_moveTimer = new Timer(20000);
			_moveTimer.Start();
			_moveTimer.Elapsed += MakeMoveOnTimer;
		}
		
	}

	private void MakeMoveOnTimer(object sender, ElapsedEventArgs e)
	{
		var move = Ai.DetermineBestMove(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData());
		_viewApp.ShowEventTimerMoveComing(move.Item1 + 1, BoardCoordinatesInternalTranslator.ConvertNumberToLetter(move.Item2 + 1));
		MakeMoveInCurrentGame(move.Item1 + 1, BoardCoordinatesInternalTranslator.ConvertNumberToLetter(move.Item2 + 1));
		_currentGameSave = _currentGame;
		_lastMoveDateTime = DateTime.Now;
		_moveTimer.Stop();
	}

	public void CancelLastMove()
	{
		_viewApp.ShowEventCancel();
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

	public void ShowChange(GameBoard gameBoard, CellState currentPlayer)
	{
		if (_currentConfig.IsHint())
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
		throw new Exception("cell ocupied");
	}

	public void ShowEventWinCondition(CellState currentPlayer)
	{
		_viewApp.ShowEventWin(currentPlayer);
		_currentGame = null;
	}
}