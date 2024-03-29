namespace Model;

public class OthelloGame : IInteractive, IDataProvidable
{
	private readonly IOthelloGameView _gameOthelloGameView;
	private List<List<bool>> _availabilityMask = new List<List<bool>>();
	private GameBoard _currentBoard; 
	private CellState _currentPlayer {  get; set;  }
	
	public OthelloGame(IOthelloGameView gameOthelloGameView, int columns = 8, int rows = 8)
	{
		_gameOthelloGameView = gameOthelloGameView;
		_currentBoard = new GameBoard(rows, columns);
		_currentPlayer = CellState.Black;
	}

	public bool IsBadMove(int row, int column)
	{
		return _currentBoard.IsBadMove(row, column, _currentPlayer);
	}

	public void MakeMove(int row, int column, bool invisible = false)
	{
		SetAvailableMoves();
		if (_currentBoard.IsBadMove(row, column, _currentPlayer))
		{ 
			_gameOthelloGameView.ShowCellOccupied(_currentPlayer);
			return;
		}
		_currentBoard.MakeMove(row, column, _currentPlayer);
		_gameOthelloGameView.ShowMoveMade( _currentPlayer, row, column);
		EndTurn();
		
		if (invisible)
		{
			return;
		}
		ShowField();
	}
	
	private void EndTurn()
	{
		
		if (_currentPlayer == CellState.White)
		{
			_currentPlayer = CellState.Black;
		}
		else if (_currentPlayer == CellState.Black)
		{
			_currentPlayer = CellState.White;
		}

		if (_currentBoard.AnyMovesAvailable(_currentPlayer))
		{
			return;
		}
		_gameOthelloGameView.ShowOnWin(_currentBoard.CalculateWinner());
		
	}
	public void ShowField()
	{
		_gameOthelloGameView.ShowField(_currentBoard, _currentPlayer);
	}

	public void ShowAvailableMoves()
	{
		SetAvailableMoves();
		_gameOthelloGameView.ShowAvailableMoves(_currentBoard, _availabilityMask, _currentPlayer);
	}

	private void SetAvailableMoves()
	{
		_availabilityMask = _currentBoard.GetAvailableMoves(_currentPlayer);
	}

	public GameBoard GetGameBoardData()
	{
		SetAvailableMoves();
		return _currentBoard;
	}
	public List<List<bool>> GetAvailableMovesData()
	{
		SetAvailableMoves();
		return _availabilityMask;
	}
}