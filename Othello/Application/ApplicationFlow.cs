using System.Diagnostics;
using System.Timers;
using Model;
using Random = System.Random;
using Timer = System.Timers.Timer;

namespace Application;

public class ApplicationFlow : IAppControl, IView, IGameProvider
{
	
	public List<(int, int)> CurrentGameMoves = new List<(int, int)>();
	private const int TimeToCancelMove = 3;
	
	public readonly IViewApp ViewApp;
	public List<IPlayer> PlayerNotifyable;
	public readonly ICoordinatesTranslator CoordinatesTranslator;
	
	
	public Game CurrentGame;
	public Config CurrentConfig;
	public CellState LastPlayerToMakeMove;
	public DateTime LastMoveDateTime;
	public Timer AutoMoveOnTimer;

	public CellState GameWinner = CellState.Empty;
	

	public ApplicationFlow(IViewApp viewApp, ICoordinatesTranslator coordinatesTranslator)
	{
		ViewApp = viewApp;
		CoordinatesTranslator = coordinatesTranslator;
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

	private void MakeAutoMoveOnOnTimer(object? sender, ElapsedEventArgs e)
	{
		
			try
			{
				if (_internalMoveOnTimer())
				{
					throw new Exception("bad move on timer, try again (T-T) (anluck)");
				}
			}
			catch (Exception exception)
			{
				_internalMoveOnTimer();
			}
		
	}

	private bool _internalMoveOnTimer()
	{
		var Krow = 0;
		var t =  CurrentGame.GetAvailableMovesData();
		foreach (var row in t)
		{
			var Kcolumn = 0;
			foreach (var column in row)
			{
				if (column)
				{
					if (TryMakeMoveInCurrentGame(Krow + 1, CoordinatesTranslator.ConvertNumberToLetter(Kcolumn + 1),
						    GetOps(LastPlayerToMakeMove)))
					{
						ViewApp.ShowEventTimerMoveComing(Krow+1, CoordinatesTranslator.ConvertNumberToLetter(Kcolumn+1));
						return true;
					}
				}
				Kcolumn++;
			}
			Krow++;

		}
		return TryMakeMoveInCurrentGame(new Random().Next(1, 8),
			CoordinatesTranslator.ConvertNumberToLetter(new Random().Next(1, 8)), GetOps(LastPlayerToMakeMove));
	}
	
	public bool SetNewGame(bool autoHint, bool timer,  IPlayer  playerNotifyable1 , IPlayer playerNotifyable2)
	{
		GameWinner = CellState.Empty;
		PlayerNotifyable = new List<IPlayer>();
		PlayerNotifyable.Add(playerNotifyable1);
		PlayerNotifyable.Add(playerNotifyable2);
		try
		{
			var newGame = new Game(this);
			CurrentGame = newGame;
			CurrentConfig = new Config(autoHint, timer);
			ViewApp.ShowAvailableMoves(CurrentGame.GetGameBoardData(), CurrentGame.GetAvailableMovesData(), CurrentGame.CurrentPlayer);
			_notifyOpponentMoveMade(CellState.White);
			return true;
		}
		catch (Exception e)
		{
			return false;
		}
		
	}

	public bool TryMakeMoveInCurrentGame(int eRow, string eColumn, CellState playerToMakeAction)
	{
		ViewApp.ShowEventMoveMadeAttempt(playerToMakeAction ,eRow, eColumn);
		var column = CoordinatesTranslator.ConvertLetterToNumber(eColumn) - 1;
		var row = eRow - 1;
		
		if (LastPlayerToMakeMove == playerToMakeAction)
		{
			return false;
		}
		try
		{
			CurrentGame.MakeMove(row, column);
		}
		catch (Exception e)
		{
			return false;
		}
		LastPlayerToMakeMove = playerToMakeAction;
		LastMoveDateTime = DateTime.Now;
		if (CurrentConfig.Timer)
		{
			AutoMoveOnTimer.Dispose();
			AutoMoveOnTimer.Start();
		}
		_notifyOpponentMoveMade(GetOps(playerToMakeAction));
		return true;
		
	}

	public bool TryCancelLastMove(CellState playerToMakeAction)
	{
		if (LastPlayerToMakeMove != playerToMakeAction)
		{
			return false;
		}

		if (LastMoveDateTime.AddSeconds(TimeToCancelMove) >= DateTime.Now)
		{
			return false;
		}

		if (CurrentGameMoves.Count == 0)
		{
			return false;
		}
		
		for (int i = 0; i < CurrentGameMoves.Count - 1; i++)
		{
			CurrentGame.MakeMove(CurrentGameMoves[i].Item1, CurrentGameMoves[i].Item2, true);
		}
		CurrentGameMoves = CurrentGameMoves.Count == 1 ? new List<(int, int)>() : CurrentGameMoves[..^1];
		LastMoveDateTime = DateTime.Now.AddSeconds(- TimeToCancelMove); // cannot cancel more moves
		LastPlayerToMakeMove = GetOps(LastPlayerToMakeMove);
		CurrentGame.ShowAvailableMoves();
		NotifyOpponentMoveCanceled(GetOps(playerToMakeAction));
		return true;
	}

	public bool TryGetHint(CellState playerToMakeAction)
	{
		if (LastPlayerToMakeMove == playerToMakeAction)
		{
			return false;
		}
		CurrentGame.ShowAvailableMoves();
		return true;
	}

	private CellState GetOps(CellState I)
	{
		if (I == CellState.White)
		{
			return CellState.Black;
		}
		if (I == CellState.Black)
		{
			return CellState.White;
		}
		//if (I == CellState.Empty){
		throw new Exception("how?");
		
	}
	
	
	
	// IView Implementation can be moved in separate class?
	public void ShowField(GameBoard gameBoard, CellState currentPlayer)
	{
		if (CurrentConfig.AutoHint)
		{
			ShowAvailableMoves(gameBoard, CurrentGame.GetAvailableMovesData(), currentPlayer);
			return;
		}
		ViewApp.ShowChange(gameBoard, currentPlayer);
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask, CellState currentPlayer)
	{
		ViewApp.ShowAvailableMoves(gameBoard, movesMask, currentPlayer);
	}

	public void ShowEventCellOccupied(CellState currentPlayer)
	{
		ViewApp.ShowEventCellOccupied(currentPlayer);
		CurrentGame.ShowField();
		throw new Exception("cell occupied");
	}

	public void ShowEventWinCondition(CellState currentPlayer)
	{
		ViewApp.ShowEventWin(currentPlayer);
		GameWinner = currentPlayer;
	}

	public void ShowEventMoveMade(CellState player, int row, int column)
	{
		ViewApp.ShowEventMoveMade(player,  row + 1, CoordinatesTranslator.ConvertNumberToLetter(column + 1));
	}


	private void _notifyOpponentMoveMade(CellState receiver)
	{
		foreach (var player in PlayerNotifyable)
		{
			player.OpponentMoveMaid(receiver);
		}
	}
	private void NotifyOpponentMoveCanceled(CellState receiver)
	{
		foreach (var player in PlayerNotifyable)
		{
			player.OpponentMoveCanceled(receiver);
		}
	}

	public GameBoard? TryGetGameBoardData(CellState dataReceiver)
	{
		if (dataReceiver == GetOps(LastPlayerToMakeMove))
		{
			return CurrentGame.GetGameBoardData();
		}

		return null;
	}

	public List<List<bool>>? TryGetAvailableMovesData(CellState dataReceiver)
	{
		if (dataReceiver == GetOps(LastPlayerToMakeMove))
		{
			return CurrentGame.GetAvailableMovesData();
		}

		return null;
	}
}