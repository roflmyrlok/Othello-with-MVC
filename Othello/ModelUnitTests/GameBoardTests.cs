using Xunit;
using Model;

public class GameBoardTests
{
    private readonly GameBoard _gameBoard;
    private int fourth = 4 - 1;
    private int fifth = 5 - 1;

    public GameBoardTests()
    {
        _gameBoard = new GameBoard(8, 8); // Assuming standard 8x8 Reversi board
    }

    [Fact]
    public void TestInitialBoardSetup()
    {
        // Assert that the initial board setup is correct
        Assert.Equal(8, _gameBoard.Columns);
        Assert.Equal(8, _gameBoard.Rows);

        
        // Check some specific cells for initial state
        // Add assertions to check the initial configuration
        Assert.Equal(CellState.Player2, _gameBoard.Board[fifth][fifth].CellState);
        Assert.Equal(CellState.Player2, _gameBoard.Board[fourth][fourth].CellState);
        Assert.Equal(CellState.Player1, _gameBoard.Board[fifth][fourth].CellState);
        Assert.Equal(CellState.Player1, _gameBoard.Board[fourth][fifth].CellState);

        // Ensure other cells are empty
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (i != 3 && i != 4 && j != 3 && j != 4)
                {
                    Assert.Equal(CellState.Empty, _gameBoard.Board[i][j].CellState);
                }
            }
        }
    }

    [Fact]
    public void TestAvailableMovesForPlayer1()
    {
        // Make some moves to set up a scenario
        _gameBoard.MakeMove(2, 3, CellState.Player1);
        _gameBoard.MakeMove(2, 4, CellState.Player2);

        // Get available moves for Player 1
        var availableMoves = _gameBoard.GetAvailableMoves(CellState.Player1);

        // Assert specific cells should be available for Player 1
        Assert.True(availableMoves[1][5]);
        Assert.True(availableMoves[2][5]);
        Assert.True(availableMoves[3][5]);
        Assert.True(availableMoves[4][5]);
        Assert.True(availableMoves[5][5]);
    }
    
    [Fact]
    public void TestAvailableMovesForPlayer2()
    {
        // Make some moves to set up a scenario
        _gameBoard.MakeMove(2, 3, CellState.Player1);
        _gameBoard.MakeMove(2, 4, CellState.Player2);
        _gameBoard.MakeMove(1, 5, CellState.Player1);

        // Get available moves for Player 2
        var availableMoves = _gameBoard.GetAvailableMoves(CellState.Player2);

        // Assert specific cells should be available for Player 2
        Assert.True(availableMoves[1][2]);
        Assert.True(availableMoves[2][2]);
        Assert.True(availableMoves[3][2]);
        Assert.True(availableMoves[4][2]);
        Assert.True(availableMoves[5][2]);
        Assert.True(availableMoves[1][4]);
    }

    [Fact]
    public void TestMakeInvalidMove()
    {
        // Attempt to make an invalid move
        Assert.Throws<InvalidOperationException>(() => _gameBoard.MakeMove(0, 0, CellState.Player1));
    }

    [Fact]
    public void TestFlippingOpponentPieces()
    {
        // Set up a scenario where Player 1 makes a move
        _gameBoard.MakeMove(2, 3, CellState.Player1);

        // Check that opponent's pieces are flipped
        Assert.Equal(CellState.Player1, _gameBoard.Board[2][3].CellState);
        Assert.Equal(CellState.Player1, _gameBoard.Board[3][3].CellState);
        Assert.Equal(CellState.Player1, _gameBoard.Board[4][3].CellState);
        Assert.Equal(CellState.Player1, _gameBoard.Board[3][4].CellState);
    }

    [Fact]
    public void TestNoAvailableMoves()
    {
        // Player1 makes moves that leave only one available move for Player2
        _gameBoard.MakeMove(4, 5, CellState.Player1);
        _gameBoard.MakeMove(5, 3, CellState.Player2);
        _gameBoard.MakeMove(4, 2, CellState.Player1);
        _gameBoard.MakeMove(3, 5, CellState.Player2);
        _gameBoard.MakeMove(2, 4, CellState.Player1);
        _gameBoard.MakeMove(5, 5, CellState.Player2);
        _gameBoard.MakeMove(4, 6, CellState.Player1);
        _gameBoard.MakeMove(5, 4, CellState.Player2);
        _gameBoard.MakeMove(6, 4, CellState.Player1);
        
        
        Assert.False(_gameBoard.AnyMovesAvailable(CellState.Player2));
    }

    [Fact]
    public void TestGameOverScenario()
    {
        // Player1 makes moves that leave only one available move for Player2
        _gameBoard.MakeMove(4, 5, CellState.Player1);
        _gameBoard.MakeMove(5, 3, CellState.Player2);
        _gameBoard.MakeMove(4, 2, CellState.Player1);
        _gameBoard.MakeMove(3, 5, CellState.Player2);
        _gameBoard.MakeMove(2, 4, CellState.Player1);
        _gameBoard.MakeMove(5, 5, CellState.Player2);
        _gameBoard.MakeMove(4, 6, CellState.Player1);
        _gameBoard.MakeMove(5, 4, CellState.Player2);
        _gameBoard.MakeMove(6, 4, CellState.Player1);
        
        
        Assert.False(_gameBoard.AnyMovesAvailable(CellState.Player2));
        Assert.Equal(CellState.Player1, _gameBoard.CalculateWinner());
    }
    
    [Fact]
    public void TestDrawCondition()
    {
        var rowI = 0;
        foreach (var row in _gameBoard.Board)
        {
            foreach (var cell in row)
            {
                if (rowI < 4)
                {
                    cell.CellState = CellState.Player1;
                }
                else
                {
                    cell.CellState = CellState.Player2;
                }
            }

            rowI += 1;
        }

        // Assert that both players have no available moves left
        Assert.False(_gameBoard.AnyMovesAvailable(CellState.Player1));
        Assert.False(_gameBoard.AnyMovesAvailable(CellState.Player2));

        // Assert that the winner is Empty, indicating a draw
        Assert.Equal(CellState.Empty, _gameBoard.CalculateWinner());
    }
    
    [Fact]
    public void TestNotAvailableMovesForPlayer1()
    {
        // Make some moves to set up a scenario
        _gameBoard.MakeMove(2, 3, CellState.Player1);
        _gameBoard.MakeMove(2, 4, CellState.Player2);

        // Get available moves for Player 1
        var availableMoves = _gameBoard.GetAvailableMoves(CellState.Player1);
        List<(int, int)> avaliableMovesTrue = new List<(int, int)>();
        avaliableMovesTrue.Add(new (1,5));
        avaliableMovesTrue.Add(new (2,5));
        avaliableMovesTrue.Add(new (3,5));
        avaliableMovesTrue.Add(new (4,5));
        avaliableMovesTrue.Add(new (5,5));
        // Assert specific cells should be available for Player 1
        int rowCounter = 0;
        foreach (var row in availableMoves)
        {
            int cellCounter = 0;
            foreach (var cell in row)
            {
                if (!avaliableMovesTrue.Contains((rowCounter, cellCounter)))
                {
                    Assert.False(cell);
                }
                cellCounter++;
            }
            rowCounter++;
        }
        Assert.True(availableMoves[1][5]);
        Assert.True(availableMoves[2][5]);
        Assert.True(availableMoves[3][5]);
        Assert.True(availableMoves[4][5]);
        Assert.True(availableMoves[5][5]);
    }

}
