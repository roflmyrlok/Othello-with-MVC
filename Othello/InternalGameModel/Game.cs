namespace Model;

public class Game : IInteractive
{
	private readonly IView _gameView;
	private List<List<bool>> _availabilityMask = new List<List<bool>>();
	private GameBoard _currentGame;
	private Player _currentPlayer;


	public Game(IView gameView)
	{
		_gameView = gameView;
	}
	
	public void SetUpNewGame(int columns = 8, int rows = 8)
	{
		_currentGame = new GameBoard(rows, columns);
		_currentPlayer = new Player( CellState.Player1);
		_view();
	}
	
	public void MakeMove(int row, int column)
	{
		_setAvailableMoves();
		if (_currentGame.IsValidMovePublic(row, column, _currentPlayer.CurrentPlayerCellState))
		{ 
			_gameView.ShowEventCellOccupied(_currentPlayer.CurrentPlayerCellState);
			return;
		}
		_currentGame.MakeMove(row, column, _currentPlayer.CurrentPlayerCellState);
		_endTurn();
		_view();
	}
	
	private void _endTurn()
	{
		
		if (_currentPlayer.CurrentPlayerCellState == CellState.Player1)
		{
			_currentPlayer.CurrentPlayerCellState = CellState.Player2;
		}
		else if (_currentPlayer.CurrentPlayerCellState == CellState.Player2)
		{
			_currentPlayer.CurrentPlayerCellState = CellState.Player1;
		}

		if (_currentGame.AnyMovesAvailable(_currentPlayer.CurrentPlayerCellState))
		{
			return;
		}
		_gameView.ShowEventWinCondition(_currentGame.CalculateWinner());
		
	}
	private void _view()
	{
		_gameView.ShowChange(_currentGame, _currentPlayer.CurrentPlayerCellState);
	}

	private void _showAvailableMoves()
	{
		_setAvailableMoves();
		_gameView.ShowAvailableMoves(_currentGame, _availabilityMask, _currentPlayer.CurrentPlayerCellState);
	}

	private void _setAvailableMoves()
	{
		_availabilityMask = _currentGame.GetAvailableMoves(_currentPlayer.CurrentPlayerCellState);
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
	public object Clone()
	{
		return this.MemberwiseClone();
	}

	public void GetHint()
	{
		_setAvailableMoves();
	}
}