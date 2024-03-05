namespace Model;

public class Game
{

	private PlayerColour PlayerColour;
	private List<IObserver> GameObservers;
	private GameBoard CurrentGame;
	private Player Player1;
	private Player Player2;
	private int lastPlayer;


	public Game(IObserver gameObserver)
	{
		PlayerColour = new PlayerColour();
		GameObservers = new List<IObserver>();
		GameObservers.Add(gameObserver);
	}
	
	public void SetUpNewPvPGame(string player1Colour = "white", string player2Colour = "black", int columns = 8, int rows = 8)
	{
		Player1 = new HumanPlayer(PlayerColour.GetColourByName(player1Colour), CellState.Player1); 
		Player2 = new HumanPlayer(PlayerColour.GetColourByName(player2Colour), CellState.Player2);
		CurrentGame = new GameBoard(rows, columns);
		_observe();
	}

	public void Start()
	{
		CurrentGame.Board[3][4].CellState = CellState.Player1;
		CurrentGame.Board[4][3].CellState = CellState.Player1;
		CurrentGame.Board[4][4].CellState = CellState.Player2;
		CurrentGame.Board[3][3].CellState = CellState.Player2;
		lastPlayer = 2;
		_observe();
	}
	
	public void MakeMove(int column, int row)
	{
		Player currPlayerMove = null;
		if (lastPlayer == 1)
		{
			currPlayerMove = Player2;
			lastPlayer = 2;
		}
		else if (lastPlayer == 2)
		{
			currPlayerMove = Player1;
			lastPlayer = 1;
		}
		//proper logic for internal move call from here
		if (CurrentGame.Board[column - 1][row - 1].CellState != CellState.Available)
		{
			_errorObserve("cell not available");
			return;
		}
		CurrentGame.Board[column - 1][row - 1].CellState = currPlayerMove.CurrentPlayerCellState;
		
		_observe();
	}
	

	private void _observe()
	{
		foreach (var obs in GameObservers)
		{
			obs.ShowChange(CurrentGame);
			
		}
	}
	private void _errorObserve(string errorType)
	{
		foreach (var obs in GameObservers)
		{
			obs.ShowInputError(errorType);
		}
	}
}