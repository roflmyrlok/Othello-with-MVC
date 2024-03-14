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
	private IMoveProvider _moveProvider;
	private ICoordinatesTranslator _coordinatesTranslator;
	private Game _currentGame;
	private Config _currentConfig;
	private DateTime _lastMoveDateTime;
	private Timer _moveTimer = new Timer(20000);
	private List<(int, int)> _moves = new List<(int, int)>();

	public AppFlow(IViewApp viewApp, IMoveProvider moveProvider, ICoordinatesTranslator coordinatesTranslator)
	{
		_viewApp = viewApp;
		_moveProvider = moveProvider;
		_coordinatesTranslator = coordinatesTranslator;
	}
	public void StartNewGame(string gameName, bool autoHint, bool vsBot, bool timer)
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
		var internalColumn = _coordinatesTranslator.ConvertLetterToNumber(column) - 1;
		var internalRow = row - 1;
		if (_currentConfig.IsBot())
		{
			_currentGame.MakeMove(internalRow, internalColumn);
			var move = _moveProvider.DetermineBestMove(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData());
			Thread.Sleep(new Random(DateTime.Now.Millisecond).Next(1400, 2300));
			_viewApp.ShowEventAiMoveComing(move.Item1 + 1, _coordinatesTranslator.ConvertNumberToLetter(move.Item2 + 1));
			Thread.Sleep(new Random(DateTime.Now.Millisecond).Next(2000, 3000));
			_currentGame.MakeMove(move.Item1, move.Item2);
		}
		else
		{
			_currentGame.MakeMove(internalRow, internalColumn);
		}
		_moves.Add(new (internalRow, internalColumn));
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
		var move = _moveProvider.DetermineBestMove(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData());
		_viewApp.ShowEventTimerMoveComing(move.Item1 + 1, _coordinatesTranslator.ConvertNumberToLetter(move.Item2 + 1));
		MakeMoveInCurrentGame(move.Item1 + 1, _coordinatesTranslator.ConvertNumberToLetter(move.Item2 + 1));
		_moves.Add(new (move.Item1, move.Item2));
		_lastMoveDateTime = DateTime.Now;
		_moveTimer.Stop();
	}

	public void CancelLastMove()
	{
		_viewApp.ShowEventCancel();
			if (_lastMoveDateTime.Date.AddSeconds(3) <= DateTime.Now)
			{
				var newGame = new Game(this);
				_currentGame = newGame;
				for (int i = 0; i < _moves.Count - 1; i++)
				{
					_currentGame.MakeMove(_moves[i].Item1, _moves[i].Item2, true);
				}
			}
			_moves = _moves[..^1];
			_viewApp.ShowAvailableMoves(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData(), _currentGame.CurrentPlayer.CurrentPlayerCellState);
			return;
		
		
		
	}

	public void GetHint()
	{
		_currentGame.ShowAvailableMoves();
	}

	public void PickNewGame(string gameName)
	{
		_currentGame = _availableGames[gameName];
		_currentConfig = _gameConfigs[_currentGame];
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
		throw new Exception("cell occupied");
	}

	public void ShowEventWinCondition(CellState currentPlayer)
	{
		_viewApp.ShowEventWin(currentPlayer);
		_currentGame = null;
	}
}