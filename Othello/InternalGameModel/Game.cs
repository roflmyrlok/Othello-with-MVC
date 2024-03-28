namespace Model;

public class Game : IInteractive, IProvideData
{
	private readonly IView _gameView;
	private List<List<bool>> _availabilityMask = new List<List<bool>>();
	private GameBoard _currentGame;
	public CellState CurrentPlayer {  get; private set;  }
	
	public Game(IView gameView, int columns = 8, int rows = 8)
	{
		_gameView = gameView;
		_currentGame = new GameBoard(rows, columns);
		CurrentPlayer = CellState.White;
	}
	
	public void MakeMove(int row, int column, bool invisible = false)
	{
		SetAvailableMoves();
		if (_currentGame.IsValidMovePublic(row, column, CurrentPlayer))
		{ 
			_gameView.ShowEventCellOccupied(CurrentPlayer);
			return;
		}
		_currentGame.MakeMove(row, column, CurrentPlayer);
		_gameView.ShowEventMoveMade( CurrentPlayer, row, column);
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

		if (_currentGame.AnyMovesAvailable(CurrentPlayer))
		{
			return;
		}
		_gameView.ShowEventWinCondition(_currentGame.CalculateWinner());
		
	}
	public void ShowField()
	{
		_gameView.ShowField(_currentGame, CurrentPlayer);
	}

	public void ShowAvailableMoves()
	{
		SetAvailableMoves();
		_gameView.ShowAvailableMoves(_currentGame, _availabilityMask, CurrentPlayer);
	}

	private void SetAvailableMoves()
	{
		_availabilityMask = _currentGame.GetAvailableMoves(CurrentPlayer);
	}

	public GameBoard GetGameBoardData()
	{
		SetAvailableMoves();
		return _currentGame;
	}
	public List<List<bool>> GetAvailableMovesData()
	{
		SetAvailableMoves();
		return _availabilityMask;
	}
}