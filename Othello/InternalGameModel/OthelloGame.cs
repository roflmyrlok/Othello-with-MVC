namespace Model;

public class OthelloGame : IInteractive, IDataProvidable
{
	private readonly IOthelloGameView _gameOthelloGameView;
	private List<List<bool>> _availabilityMask = new List<List<bool>>();
	private GameBoard _currentBoard;
	public CellState CurrentPlayer {  get; private set;  }
	
	public OthelloGame(IOthelloGameView gameOthelloGameView, int columns = 8, int rows = 8)
	{
		_gameOthelloGameView = gameOthelloGameView;
		_currentBoard = new GameBoard(rows, columns);
		CurrentPlayer = CellState.Black;
	}

	public bool IsBadMove(int row, int column)
	{
		return _currentBoard.IsBadMove(row, column, CurrentPlayer);
	}

	public void MakeMove(int row, int column, bool invisible = false)
	{
		SetAvailableMoves();
		if (_currentBoard.IsBadMove(row, column, CurrentPlayer))
		{ 
			_gameOthelloGameView.ShowCellOccupied(CurrentPlayer);
			return;
		}
		_currentBoard.MakeMove(row, column, CurrentPlayer);
		_gameOthelloGameView.ShowMoveMade( CurrentPlayer, row, column);
		EndTurn();
		
		if (invisible)
		{
			return;
		}
		ShowField();
	}
	
	private void EndTurn()
	{
		
		if (CurrentPlayer == CellState.White)
		{
			CurrentPlayer = CellState.Black;
		}
		else if (CurrentPlayer == CellState.Black)
		{
			CurrentPlayer = CellState.White;
		}

		if (_currentBoard.AnyMovesAvailable(CurrentPlayer))
		{
			return;
		}
		_gameOthelloGameView.ShowOnWin(_currentBoard.CalculateWinner());
		
	}
	public void ShowField()
	{
		_gameOthelloGameView.ShowField(_currentBoard, CurrentPlayer);
	}

	public void ShowAvailableMoves()
	{
		SetAvailableMoves();
		_gameOthelloGameView.ShowAvailableMoves(_currentBoard, _availabilityMask, CurrentPlayer);
	}

	private void SetAvailableMoves()
	{
		_availabilityMask = _currentBoard.GetAvailableMoves(CurrentPlayer);
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