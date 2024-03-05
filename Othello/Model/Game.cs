namespace Model;

public class Game
{

	private readonly PlayerColour _playerColour;
	private readonly List<IObserver> _gameObservers;
	private List<List<bool>> _availabilityMask = new List<List<bool>>();
	private GameBoard _currentGame;
	private Player _player1;
	private Player _player2;
	private Player _currentPlayer;
	private bool _autoHint = true;


	public Game(IObserver gameObserver)
	{
		_playerColour = new PlayerColour();
		_gameObservers = new List<IObserver>();
		_gameObservers.Add(gameObserver);
	}
	
	public void SetUpNewPvPGame(string player1Colour = "white", string player2Colour = "black", int columns = 8, int rows = 8)
	{
		_player1 = new HumanPlayer(_playerColour.GetColourByName(player1Colour), CellState.Player1); 
		_player2 = new HumanPlayer(_playerColour.GetColourByName(player2Colour), CellState.Player2);
		_currentGame = new GameBoard(rows, columns);
		_observe();
	}

	public void Start()
	{
		_currentGame.Board[3][4].CellState = CellState.Player1;
		_currentGame.Board[4][3].CellState = CellState.Player1;
		_currentGame.Board[4][4].CellState = CellState.Player2;
		_currentGame.Board[3][3].CellState = CellState.Player2;
		_currentPlayer = _player1;
		_observe();
	}
	
	public void MakeMove(int column, int row)
	{
		_setAvailableMoves();
		if (!_availabilityMask[column][row])
		{
			throw new Exception("move not available");
		}
		_currentGame.Board[column - 1][row - 1].CellState = _currentPlayer.CurrentPlayerCellState;
		_endTurn();
	}

	public void Hint()
	{
		_showAvailableMoves();
	}



	private void _endTurn()
	{
		if (_currentPlayer == _player1)
		{
			_currentPlayer = _player2;
		}
		else if (_currentPlayer == _player2)
		{
			_currentPlayer = _player1;
		}
		_observe();
	}
	private void _observe()
	{
		if (_autoHint)
		{
			_showAvailableMoves();
			return;
		}
		foreach (var obs in _gameObservers)
		{
			obs.ShowChange(_currentGame);
			
		}
	}

	private void _showAvailableMoves()
	{
		_setAvailableMoves();
		foreach (var obs in _gameObservers)
		{
			obs.ShowAvailabgleMoves(_currentGame, _availabilityMask);
			
		}
	}

	private void _setAvailableMoves()
	{
		_availabilityMask = _currentGame.GetAvailableMoves();
	}
}