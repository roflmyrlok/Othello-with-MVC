using System;
using System.Collections.Generic;

namespace Model
{
    public class GameBoard
    {
        public List<List<Cell>> Board { get; }
        public int Columns { get; }
        public int Rows { get; }

        public GameBoard(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            Board = new List<List<Cell>>();

            InitializeBoard();
            SetupInitialBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < Columns; i++)
            {
                Board.Add(new List<Cell>());
                for (int j = 0; j < Rows; j++)
                {
                    Board[i].Add(new Cell(i, j, CellState.Empty));
                }
            }
        }

        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < Columns && col >= 0 && col < Rows;
        }

        public List<List<bool>> GetAvailableMoves(CellState currentPlayer)
        {
            var availableMoves = new List<List<bool>>();

            for (int i = 0; i < Columns; i++)
            {
                availableMoves.Add(new List<bool>());
                for (int j = 0; j < Rows; j++)
                {
                    availableMoves[i].Add(IsValidMove(i, j, currentPlayer));
                }
            }

            return availableMoves;
        }

        private bool IsValidMove(int row, int col, CellState currentPlayer)
        {
            if (Board[row][col].CellState != CellState.Empty)
                return false;

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int d = 0; d < 8; d++)
            {
                int x = row + dx[d];
                int y = col + dy[d];

                if (!IsValidPosition(x, y) || Board[x][y].CellState == currentPlayer || Board[x][y].CellState == CellState.Empty)
                    continue;

                while (IsValidPosition(x, y) && Board[x][y].CellState != CellState.Empty)
                {
                    if (Board[x][y].CellState == currentPlayer)
                        return true;

                    x += dx[d];
                    y += dy[d];
                }
            }

            return false;
        }

        public bool IsValidMovePublic(int row, int col, CellState player)
        {
            return (!IsValidPosition(row, col) || Board[row][col].CellState != CellState.Empty ||
                    !IsValidMove(row, col, player));
        }
        public void MakeMove(int row, int col, CellState player)
        {
            if (IsValidMovePublic(row, col, player))
            {
                throw new Exception("how?");
            }
            Board[row][col].CellState = player;
            FlipOpponentPieces(row, col, player);
        }

        private void FlipOpponentPieces(int row, int col, CellState player)
        {
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int d = 0; d < 8; d++)
            {
                int x = row + dx[d];
                int y = col + dy[d];

                List<Cell> flippedPieces = new List<Cell>();

                if (!IsValidPosition(x, y) || Board[x][y].CellState == CellState.Empty || Board[x][y].CellState == player)
                    continue;

                while (IsValidPosition(x, y) && Board[x][y].CellState != CellState.Empty)
                {
                    if (Board[x][y].CellState == player)
                    {
                        foreach (var piece in flippedPieces)
                        {
                            piece.CellState = player;
                        }
                        break;
                    }
                    else
                    {
                        flippedPieces.Add(Board[x][y]);
                    }
                    x += dx[d];
                    y += dy[d];
                }
            }
        }

        private void SetupInitialBoard()
        {
            int midRow = Rows / 2;
            int midCol = Columns / 2;
            Board[midRow - 1][midCol - 1].CellState = CellState.Player2;
            Board[midRow - 1][midCol].CellState = CellState.Player1;
            Board[midRow][midCol - 1].CellState = CellState.Player1;
            Board[midRow][midCol].CellState = CellState.Player2;
        }
        
        public bool AnyMovesAvailable(CellState player)
        {
            var availableMoves = GetAvailableMoves(player);

            foreach (var row in availableMoves)
            {
                foreach (var cell in row)
                {
                    if (cell)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public CellState CalculateWinner()
        {
            int player1Count = 0;
            int player2Count = 0;

            // Count the number of cells occupied by each player
            foreach (var row in Board)
            {
                foreach (var cell in row)
                {
                    if (cell.CellState == CellState.Player1)
                    {
                        player1Count++;
                    }
                    else if (cell.CellState == CellState.Player2)
                    {
                        player2Count++;
                    }
                }
            }
            if (player1Count > player2Count)
            {
                return CellState.Player1;
            }
            else if (player2Count > player1Count)
            {
                return CellState.Player2;
            }
            else
            {
                return CellState.Empty; 
            }
        }


    }
}
