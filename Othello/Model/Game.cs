using AiOthelloModel;
using ConsoleController;

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
	private bool _botOpponent = true;


	public Game(IView gameView, IInputErrorNotifier errorNotifier)
	{
		_playerColour = new PlayerColour();
		_gameView = gameView;
		_errorNotifier = errorNotifier;
	}
	
	public void SetUpNewGame(bool bot = false, bool hint = false, string player1Colour = "white", string player2Colour = "black", 
																									int columns = 8, int rows = 8)
	{
		_player1 = new Player(_playerColour.GetColourByName(player1Colour), CellState.Player1); 
		_player2 = new Player(_playerColour.GetColourByName(player2Colour), CellState.Player2);
		_currentGame = new GameBoard(rows, columns);
		_currentPlayer = new Player(new Colour(1,1,1), CellState.Player1);
		_botOpponent = bot;
		_autoHint = hint;
		_view();
	}

	public void MakeMove(int row, string column)
	{
		var internalColumn = BoardCoordinatesInternalTranslator.ConvertLetterToNumber(column) - 1;
		var internalRow = row - 1;
		if (_botOpponent)
		{
			MakeMoveWithAiAnswer(internalRow, internalColumn);
		}
		else
		{
			MakeMove(internalRow, internalColumn);
		}
	}
	
	
	private void MakeMove(int row, int column)
	{
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
	
	private void MakeMoveWithAiAnswer(int row, int column)
	{
		_setAvailableMoves();
		if (!_availabilityMask[row][column])
		{ 
			//tmp solution
			_showError("move not available");
			return;
		}
		_currentGame.MakeMove(row, column, _currentPlayer.CurrentPlayerCellState);
		_endTurn();
		_view();
		Thread.Sleep(2000);
		_showError("currentplayer: bot");
		Thread.Sleep(3000);
		_setAvailableMoves();
		var tmp = Ai.DetermineBestMove((_currentGame, _availabilityMask));
		_showError("ai move incoming " + (tmp.Item1 + 1) + " " + BoardCoordinatesInternalTranslator.ConvertNumberToLetter(tmp.Item2 + 1));
		_currentGame.MakeMove(tmp.Item1, tmp.Item2, _currentPlayer.CurrentPlayerCellState);
		_endTurn();
		Thread.Sleep(3000);
		_view();
		Thread.Sleep(2000);
		_showError("currentplayer: " + _currentPlayer.CurrentPlayerCellState);
	}

	public void Hint()
	{
		_showAvailableMoves();
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