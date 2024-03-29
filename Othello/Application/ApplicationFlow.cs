using System.Diagnostics;
using Application;

namespace Application;
using System.Timers;
using Model;
using Random = System.Random;
using Timer = System.Timers.Timer;



public class ApplicationFlow : ISetup, IOthelloGameView, IDataProvidable, IInteractive
{

	private List<Move> _currentGameMoves = new List<Move>();
	private const int TimeToCancelMove = 3; //seconds
	private const int TimeToMakeMove = 20000; //ms

	private readonly IViewApp _viewApp;
	public Player PlayerWhite;
	public Player PlayerBlack;
	public Player CurrentPlayer;
	public readonly ICoordinatesTranslator CoordinatesTranslator;


	private OthelloGame _currentOthelloGame;
	private Configuration _configuration;
	private Timer _autoMoveOnTimer;

	public CellState GameWinner = CellState.Empty;


	public ApplicationFlow(IViewApp viewApp, ICoordinatesTranslator coordinatesTranslator,
		Configuration gameConfiguration)
	{
		_viewApp = viewApp;
		CoordinatesTranslator = coordinatesTranslator;
		_currentOthelloGame = new OthelloGame(this);
		_configuration = gameConfiguration;
		_autoMoveOnTimer = new Timer(TimeToMakeMove);
		_autoMoveOnTimer.AutoReset = false;
		_autoMoveOnTimer.Disposed += ResetAutoMoveOnTimer;
		_autoMoveOnTimer.Elapsed += PerformAutoMoveOnTimer;
	}

	private void PerformAutoMoveOnTimer(object? sender, ElapsedEventArgs e)
	{
		var move = (new Random().Next(0,7), new Random().Next(0,7));
		while (_currentOthelloGame.IsBadMove(move.Item1, move.Item2))
		{
			move = (new Random().Next(0,7), new Random().Next(0,7));
		}
		_viewApp.ShowTimerMoveComing(move.Item1 + 1, CoordinatesTranslator.ConvertNumberToLetter(move.Item2 + 1));
		if (!TryMakeMoveInCurrentGame(move.Item1 + 1, CoordinatesTranslator.ConvertNumberToLetter(move.Item2 + 1),
			    CurrentPlayer))
		{
			_autoMoveOnTimer.Dispose();
			_autoMoveOnTimer.Start();
		}
		else
		{
			_currentGameMoves[^1].MoveByTimer = true;
		}
		

		
	}

	private void ResetAutoMoveOnTimer(object? sender, EventArgs e)
	{
		_autoMoveOnTimer.Stop();
		_autoMoveOnTimer = new Timer(TimeToMakeMove);
		_autoMoveOnTimer.AutoReset = false;
		_autoMoveOnTimer.Disposed += ResetAutoMoveOnTimer;
		_autoMoveOnTimer.Elapsed += PerformAutoMoveOnTimer;
	}


	//	IInteractive


	public bool TryMakeMoveInCurrentGame(int row, string column, Player playerToMakeAction)
	{
		if (GameWinner != CellState.Empty)
		{
			return false;
		}
		_viewApp.ShowMoveMadeAttempt(playerToMakeAction, row, column);
		if (playerToMakeAction != CurrentPlayer || playerToMakeAction.Opponent is null)
		{
			return false;
		}

		if (_currentOthelloGame.IsBadMove(row - 1, CoordinatesTranslator.ConvertLetterToNumber(column) - 1))
		{
			return false;
		}

		var move = new Move(CurrentPlayer, row - 1, CoordinatesTranslator.ConvertLetterToNumber(column) - 1,
			DateTime.Now);
		try
		{
			_currentOthelloGame.MakeMove(move.InternalRow, move.InternalColumn);
		}
		catch (Exception e)
		{
			return false;
		}
		_currentGameMoves.Add(move);
		CurrentPlayer = CurrentPlayer.Opponent;
		if (_configuration.Timer)
		{
			_autoMoveOnTimer.Dispose();
			_autoMoveOnTimer.Start();
		}
		CurrentPlayer.OpponentMoveMaid();
		return true;

	}

	public bool TryCancelLastMove(Player playerToMakeAction)
	{
		var callTime = DateTime.Now;
		if (CurrentPlayer == playerToMakeAction)
		{
			return false;
		}
		if (_currentGameMoves[^1].PerformedBy != playerToMakeAction)
		{
			if (_currentGameMoves[^1].MoveMadeAt < callTime)
			{
				return false;
			}
		}
		if (_currentGameMoves[^1].MoveByTimer)
		{
			if (_currentGameMoves[^1].MoveMadeAt < callTime)
			{
				return false;
			}
		}
		if (_currentGameMoves[^1].MoveMadeAt < DateTime.Now.AddSeconds(-TimeToCancelMove))
		{
			if (_currentGameMoves[^1].MoveMadeAt < callTime)
			{
				return false;
			}
		}
		try
		{
			CurrentPlayer = PlayerBlack;
			var newOthelloGame = new OthelloGame(this);
			for (var index = 0; index < _currentGameMoves.Count - 1; index++)
			{
				var move = _currentGameMoves[index];
				newOthelloGame.MakeMove(move.InternalRow, move.InternalColumn, true);
			}
			_currentGameMoves = _currentGameMoves[..^1];
			_currentGameMoves[^1].MoveByTimer = true; // cant undo anymore
			CurrentPlayer = _currentGameMoves[^1].PerformedBy.Opponent;
			_currentOthelloGame = newOthelloGame;
			_viewApp.ShowUndoMade();
			_viewApp.ShowChange(newOthelloGame.GetGameBoardData(), CurrentPlayer);
			CurrentPlayer.Opponent.OpponentMoveCanceled();
			CurrentPlayer.OpponentMoveMaid();
			return true;
		}
		catch (Exception e)
		{
			return false;
		}
	}

	public bool TryGetHint(Player playerToMakeAction)
	{
		if (playerToMakeAction == CurrentPlayer)
		{
			var gb = _currentOthelloGame.GetGameBoardData();
			var am = _currentOthelloGame.GetAvailableMovesData();
			_viewApp.ShowAvailableMoves(gb, am, playerToMakeAction);
			return true;
		}
		return false;
	}
//Setup

	public bool Setup(bool autoHint, bool timer,  Player  playerWhite , Player playerBlack)
	{
		GameWinner = CellState.Empty;
		PlayerWhite = (playerWhite);
		PlayerBlack = (playerBlack);
		CurrentPlayer = PlayerBlack;
		try
		{
			var newGame = new OthelloGame(this);
			_currentOthelloGame = newGame;
			_configuration = new Configuration(autoHint, timer);
			ShowField(_currentOthelloGame.GetGameBoardData(), CurrentPlayer.PlayerCellState);
			playerBlack.OpponentMoveMaid();
			return true;
		}
		catch (Exception e)
		{
			return false;
		}
		
	}


//IDataProvidable
	
	public GameBoard? TryGetGameBoardData(Player dataReceiver)
	{
		if (dataReceiver == CurrentPlayer)
		{
			return _currentOthelloGame.GetGameBoardData();
		}

		return null;
	}

	public List<List<bool>>? TryGetAvailableMovesData(Player dataReceiver)
	{
		if (dataReceiver == CurrentPlayer)
		{
			return _currentOthelloGame.GetAvailableMovesData();
		}

		return null;
	} 
//IOthelloGameView

	public void ShowField(GameBoard gameBoard, CellState currentPlayer)
	{
		if (_configuration.AutoHint)
		{
			var am = _currentOthelloGame.GetAvailableMovesData();
			ShowAvailableMoves(gameBoard, am, currentPlayer);
			return;
		}

		switch (currentPlayer)
		{
			case CellState.White:
				_viewApp.ShowChange(gameBoard, PlayerBlack);
				break;
			case CellState.Black:
				_viewApp.ShowChange(gameBoard, PlayerWhite);
				break;
			default:
				ShowOnWin(GameWinner);
				break;
		}
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer)
	{
		if (currentPlayer == CellState.White)
		{
			_viewApp.ShowAvailableMoves(gameBoard, movesMask, PlayerWhite );
		}
		else if (currentPlayer == CellState.Black)
		{
			_viewApp.ShowAvailableMoves(gameBoard, movesMask, PlayerBlack );
		}
		else
		{
			ShowOnWin(GameWinner);	
		}
	}

	public void ShowCellOccupied(CellState currentPlayer)
	{
		_viewApp.ShowCellOccupied(CurrentPlayer);
		_currentOthelloGame.ShowField();
	}

	public void ShowOnWin(CellState currentPlayer)
	{
		_viewApp.ShowOnWin(CurrentPlayer);
		GameWinner = currentPlayer;
	}

	public void ShowMoveMade(CellState player, int row, int column)
	{
		if (player == CellState.White)
		{
			_viewApp.ShowMoveMade(PlayerWhite,  row + 1, CoordinatesTranslator.ConvertNumberToLetter(column + 1));
		}
		else if (player == CellState.Black)
		{
			_viewApp.ShowMoveMade(PlayerBlack,  row + 1, CoordinatesTranslator.ConvertNumberToLetter(column + 1));
		}
		else
		{
			ShowOnWin(GameWinner);	
		}
	}
	
	
}