using AiOthelloModel;

namespace Model;

public class Game
{

	private readonly PlayerColour _playerColour;
	private readonly IView _gameView;
	private readonly IInputErrorNotifier _errorNotifier;
	private List<List<bool>> _availabilityMask = new List<List<bool>>();
	private GameBoard _currentGame;
	private Player _player1;
	private Player _player2;
	private Player _currentPlayer;
	private bool _autoHint = true;
	private List<string> _alphabet = new List<string>
	{
		"a","b","c","d","e","f","g","h","i","g","k"
	};


	public Game(IView gameView, IInputErrorNotifier errorNotifier)
	{
		_playerColour = new PlayerColour();
		_gameView = gameView;
		_errorNotifier = errorNotifier;
	}
	
	public void SetUpNewGame(string player1Colour = "white", string player2Colour = "black", int columns = 8, int rows = 8)
	{
		_player1 = new Player(_playerColour.GetColourByName(player1Colour), CellState.Player1); 
		_player2 = new Player(_playerColour.GetColourByName(player2Colour), CellState.Player2);
		_currentGame = new GameBoard(rows, columns);
		_currentPlayer = _player1;
	}

	public void Start()
	{
		_currentGame.Board[3][4].CellState = CellState.Player1;
		_currentGame.Board[4][3].CellState = CellState.Player1;
		_currentGame.Board[4][4].CellState = CellState.Player2;
		_currentGame.Board[3][3].CellState = CellState.Player2;
		_currentPlayer = _player1;
		_view();
	}
	
	public void MakeMove(int columnR, int rowR)
	{
		var column = columnR - 1;
		var row = rowR - 1;
		_setAvailableMoves();
		if (!_availabilityMask[row][column])
		{ 
			//tmp solution
			_showError("move not available");
			return;
		}
		_currentGame.MakeMove(row, column, _currentPlayer.CurrentPlayerCellState);
		_endTurn();
	}
	
	public void MakeMoveWithAiAnswer(int columnR, int rowR)
	{
		var column = columnR - 1;
		var row = rowR - 1;
		_setAvailableMoves();
		if (!_availabilityMask[row][column])
		{ 
			//tmp solution
			_showError("move not available");
			return;
		}
		_currentGame.MakeMove(row, column, _currentPlayer.CurrentPlayerCellState);
		_endTurn();
		Thread.Sleep(300);
		_setAvailableMoves();
		var tmp = Ai.DetermineBestMove((_currentGame, _availabilityMask));
		_showError("ai move incoming " + (tmp.Item1 + 1) + " " + _alphabet[ tmp.Item2 + 1]);
		MakeMove(tmp.Item2, tmp.Item1);
		
	}

	public void Hint()
	{
		_showAvailableMoves();
	}



	private void _endTurn()
	{
		if (_currentPlayer.CurrentPlayerCellState == _player1.CurrentPlayerCellState)
		{
			_currentPlayer.CurrentPlayerCellState = _player2.CurrentPlayerCellState;
		}
		else if (_currentPlayer.CurrentPlayerCellState == _player2.CurrentPlayerCellState)
		{
			_currentPlayer.CurrentPlayerCellState = _player1.CurrentPlayerCellState;
		}
		_view();
		//tmp solution
		_showError("currentplayer : " + _currentPlayer.CurrentPlayerCellState);
	}
	private void _view()
	{
		if (_autoHint)
		{
			_setAvailableMoves();
			_showAvailableMoves();
			return;
		}
		_gameView.ShowChange(_currentGame);
	}

	private void _showError(string msg)
	{
		_errorNotifier.ShowInputError(msg);
	}

	private void _showAvailableMoves()
	{
		_setAvailableMoves();
		_gameView.ShowAvailableMoves(_currentGame, _availabilityMask);
	}

	private void _setAvailableMoves()
	{
		_availabilityMask = _currentGame.GetAvailableMoves(_currentPlayer.CurrentPlayerCellState);
	}
	public (GameBoard, List<List<bool>>) GetAiData()
	{
		_setAvailableMoves();
		return (_currentGame, _availabilityMask);
	}
}