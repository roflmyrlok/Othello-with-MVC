namespace Model;

public class Game : IInteractive
{
	private readonly IView _gameView;
	private List<List<bool>> _availabilityMask = new List<List<bool>>();
	private GameBoard _currentGame;
	public Player CurrentPlayer {  get; private set;  }
	
	public Game(IView gameView, int columns = 8, int rows = 8)
	{
		_gameView = gameView;
		_currentGame = new GameBoard(rows, columns);
		CurrentPlayer = new Player( CellState.Player1);
	}
	
	public void MakeMove(int row, int column, bool invisible = false)
	{
		_setAvailableMoves();
		if (_currentGame.IsValidMovePublic(row, column, CurrentPlayer.CurrentPlayerCellState))
		{ 
			_gameView.ShowEventCellOccupied(CurrentPlayer.CurrentPlayerCellState);
			return;
		}
		_currentGame.MakeMove(row, column, CurrentPlayer.CurrentPlayerCellState);
		_endTurn();
		if (invisible)
		{
			return;
		}
		_view();
	}
	
	private void _endTurn()
	{
		
		if (CurrentPlayer.CurrentPlayerCellState == CellState.Player1)
		{
			CurrentPlayer.CurrentPlayerCellState = CellState.Player2;
		}
		else if (CurrentPlayer.CurrentPlayerCellState == CellState.Player2)
		{
			CurrentPlayer.CurrentPlayerCellState = CellState.Player1;
		}

		if (_currentGame.AnyMovesAvailable(CurrentPlayer.CurrentPlayerCellState))
		{
			return;
		}
		_gameView.ShowEventWinCondition(_currentGame.CalculateWinner());
		
	}
	private void _view()
	{
		_gameView.ShowChange(_currentGame, CurrentPlayer.CurrentPlayerCellState);
	}

	private void _showAvailableMoves()
	{
		_setAvailableMoves();
		_gameView.ShowAvailableMoves(_currentGame, _availabilityMask, CurrentPlayer.CurrentPlayerCellState);
	}

	private void _setAvailableMoves()
	{
		_availabilityMask = _currentGame.GetAvailableMoves(CurrentPlayer.CurrentPlayerCellState);
	}

	public GameBoard GetGameBoardData()
	{
		_setAvailableMoves();
		return _currentGame;
	}
	public List<List<bool>> GetAvailableMovesData()
	{
		_setAvailableMoves();
		return _availabilityMask;
	}

	public void GetHint()
	{
		_setAvailableMoves();
	}
}