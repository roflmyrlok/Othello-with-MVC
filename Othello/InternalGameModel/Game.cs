namespace Model;

public class Game : IInteractive, IProvideData
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
		_gameView.ShowEventMoveMade( CurrentPlayer.CurrentPlayerCellState, row, column);
		_endTurn();
		if (invisible)
		{
			return;
		}
		View();
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
	public void View()
	{
		_gameView.ShowChange(_currentGame, CurrentPlayer.CurrentPlayerCellState);
	}

	public void ShowAvailableMoves()
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
}