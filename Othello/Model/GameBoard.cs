using System;
using System.Collections.Generic;

namespace Model
{
    public class GameBoard
    {
        //column{row}
        public List<List<Cell>> Board;
        public int Columns { get; }
        public int Rows { get; }

        public GameBoard(int columns, int rows)
        {
            Board = new List<List<Cell>>();
            Rows = rows;
            Columns = columns;
            for (int i = 0; i < columns; i++)
            {
                Board.Add(new List<Cell>());
                for (int j = 0; j < rows; j++)
                {
                    Board[i].Add(new Cell(i, j, CellState.Empty));
                }
            }
        }

        public List<List<bool>> GetAvailableMoves(CellState cellState)
        {
            var availableMoves = new List<List<bool>>();

            // Initialize the availableMoves matrix
            for (int i = 0; i < Columns; i++)
            {
                availableMoves.Add(new List<bool>());
                for (int j = 0; j < Rows; j++)
                {
                    availableMoves[i].Add(false);
                }
            }

            // Iterate over each cell on the board
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Cell cell = Board[i][j];

                    // Check if the cell is empty
                    if (cell.CellState == CellState.Empty)
                    {
                        // Check if placing a piece in this cell flips any opponent pieces
                        if (IsValidMove(i, j, cellState))
                        {
                            availableMoves[i][j] = true; // Mark the cell as available
                        }
                    }
                }
            }

            return availableMoves;
        }

        // Helper method to check if placing a piece in a cell flips any opponent pieces
        private bool IsValidMove(int col, int row, CellState cellState)
        {
            // Check in all eight directions
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int d = 0; d < 8; d++)
            {
                int x = row + dx[d];
                int y = col + dy[d];

                // Skip if the adjacent cell is out of bounds or empty
                if (x < 0 || x >= Columns || y < 0 || y >= Rows || Board[x][y].CellState == CellState.Empty)
                    continue;

                // If the adjacent cell contains an opponent's piece, continue searching in this direction
                if (Board[x][y].CellState != cellState)
                {
                    while (x >= 0 && x < Columns && y >= 0 && y < Rows && Board[x][y].CellState != CellState.Empty)
                    {
                        // If we find our own piece, this move is valid
                        if (Board[x][y].CellState == cellState)
                            return true;
                        x += dx[d];
                        y += dy[d];
                    }
                }
            }

            return false;
        }
        public void MakeMove(int row, int col, CellState player)
        {
            if (Board[row][col].CellState != CellState.Empty)
            {
                throw new InvalidOperationException("Invalid move: Cell is not empty.");
            }

            bool validMove = false;

            // Check in all eight directions
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int d = 0; d < 8; d++)
            {
                int x = row + dx[d];
                int y = col + dy[d];

                // Skip if the adjacent cell is out of bounds or empty
                if (x < 0 || x >= Columns || y < 0 || y >= Rows || Board[x][y].CellState == CellState.Empty || Board[x][y].CellState == player)
                    continue;

                // If the adjacent cell contains an opponent's piece, continue searching in this direction
                if (Board[x][y].CellState != player)
                {
                    while (x >= 0 && x < Columns && y >= 0 && y < Rows && Board[x][y].CellState != CellState.Empty)
                    {
                        // If we find our own piece, this move is valid
                        if (Board[x][y].CellState == player)
                        {
                            validMove = true;
                            break;
                        }
                        x += dx[d];
                        y += dy[d];
                    }
                }
            }

            if (!validMove)
            {
                throw new InvalidOperationException("Invalid move: Move does not flip any opponent pieces.");
            }

            // Place the player's piece on the board
            Board[row][col].CellState = player;

            // Flip opponent pieces
            FlipOpponentPieces(row, col, player);
        }

        // Helper method to flip opponent pieces
        private void FlipOpponentPieces(int row, int col, CellState player)
        {
            // Check in all eight directions
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int d = 0; d < 8; d++)
            {
                int x = row + dx[d];
                int y = col + dy[d];

                List<Cell> flippedPieces = new List<Cell>();

                // Skip if the adjacent cell is out of bounds or empty or contains player's own piece
                if (x < 0 || x >= Columns || y < 0 || y >= Rows || Board[x][y].CellState == CellState.Empty || Board[x][y].CellState == player)
                    continue;

                // If the adjacent cell contains an opponent's piece, continue searching in this direction
                if (Board[x][y].CellState != player)
                {
                    while (x >= 0 && x < Columns && y >= 0 && y < Rows && Board[x][y].CellState != CellState.Empty)
                    {
                        // If we find our own piece, flip the opponent pieces in between
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
        }
    }
}
