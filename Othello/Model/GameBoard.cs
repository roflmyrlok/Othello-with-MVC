using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<List<bool>> GetAvailableMoves()
        {
            var availableMoves = new List<List<bool>>();

            for (int i = 0; i < Columns; i++)
            {
                availableMoves.Add(new List<bool>());
                for (int j = 0; j < Rows; j++)
                {
                    availableMoves[i].Add(false);
                }
            }

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    // Check if the cell is empty
                    if (Board[i][j].CellState == CellState.Empty)
                    {
                        // Check if any neighboring cell contains a player's piece
                        if (IsNeighborOccupied(i, j))
                        {
                            availableMoves[i][j] = true;
                        }
                    }
                }
            }

            return availableMoves;
        }

        private bool IsNeighborOccupied(int x, int y)
        {
            // Check the eight neighboring cells
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int newX = x + dx;
                    int newY = y + dy;

                    // Skip if out of bounds or the current cell
                    if (newX < 0 || newX >= Columns || newY < 0 || newY >= Rows || (dx == 0 && dy == 0))
                    {
                        continue;
                    }

                    // Check if the neighboring cell is occupied by a player's piece
                    if (Board[newX][newY].CellState != CellState.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
