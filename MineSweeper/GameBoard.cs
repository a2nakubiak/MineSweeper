using System;

namespace MineSweeper
{
    class GameBoard
    {
        #region Fields

        //check if it is the first move
        bool isFirstMove = true;

        //random number
        Random rand = new Random();

        //board size
        int width;
        int height;

        //game board
        public Board board;
        int mineNumber;

        #endregion

        #region Constructor
        public GameBoard(int width, int height, int mines)
        {
            this.width = width;
            this.height = height;
            this.mineNumber = mines;

            //create board objec with empty tiles.cell
            board = new Board(width, height, new Tile(Cell.EMPTY));
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Open given tile
        /// Change closed status of userdBoard to Empty or Numerical
        /// Recursive function - open all empty cells adjacent to given empty cell
        /// </summary>
        /// <param name="userBoard">The board on which it will open the tile</param>
        /// <param name="position">Tile coordinates to open</param>
        /// <returns>State of open tile </returns>
        public Cell Open(ref Board userBoard, Tuple<int, int> position)
        {
            int x = position.Item1;
            int y = position.Item2;

            if (isFirstMove)
            {
                isFirstMove = false;
                GenerateMineField(width, height, mineNumber, position);
            }

            if (board.getBoard()[x][y].cell == Cell.EMPTY)
            {
                for (int i = -1; i <= 1; i++)
                {
                    //check the horizontal range of board
                    if (x + i >= 0 && x + i < width)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            //check the vertical range of board and tile state
                            if (y + j >= 0 && y + j < height && userBoard.getBoard()[x + i][y + j].cell == Cell.CLOSED)
                            {
                                //check if empty cell if so open function call itself again with the position of adjacent empty cell
                                //open numerical cell adjacent to empty cell 
                                if (board.getBoard()[x + i][y + j].cell == Cell.EMPTY)
                                {
                                    userBoard.getBoard()[x][y] = board.getBoard()[x][y];
                                    Tuple<int, int> newPosition = new Tuple<int, int>(x + i, y + j);

                                    Open(ref userBoard, newPosition);
                                }
                                else
                                {
                                    userBoard.getBoard()[x + i][y + j] = board.getBoard()[x + i][y + j];

                                }

                            }
                        }
                    }
                }
            }

            //if cell is the mine, return mine and the end of the game
            else if (board.getBoard()[x][y].cell == Cell.MINE)
            {
                userBoard.getBoard()[x][y] = board.getBoard()[x][y]; ;
                return userBoard.getBoard()[x][y].cell;
            }
            else
            {
                //open numerical cell
                userBoard.getBoard()[x][y] = board.getBoard()[x][y];
            }

            return userBoard.getBoard()[x][y].cell;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generete mines outside the first move area
        /// Guarantees that the player will not lose in the first round
        /// </summary>
        /// <param name="width">Width board</param>
        /// <param name="height">Height board</param>
        /// <param name="count">Number of mines</param>
        /// <param name="position">Location of move (x,y)</param>
        private void GenerateMineField(int width, int height, int count, Tuple<int, int> position)
        {
            int x, y;
            int xPosition = position.Item1;
            int yPosition = position.Item2;

            for (int i = 0; i < count; i++)
            {
                x = rand.Next(0, width);
                y = rand.Next(0, height);

                //check area of first move
                if (!(xPosition - 1 <= x && xPosition + 1 >= x && yPosition - 1 <= y && yPosition + 1 >= y))
                {
                    //check if mine is in board if not add
                    if (board.getBoard()[x][y].cell == Cell.MINE)
                    {
                        i--;
                    }
                    else
                    {
                        board.getBoard()[x][y].cell = Cell.MINE;

                        SetAdjacentCells(ref board, x, y);
                    }
                }
                else i--;

            }

        }

        /// <summary>
        /// Set numerical values of tiles based on number of adjacent mines 
        /// </summary>
        /// <param name="board">Board game</param>
        /// <param name="x">Horizontal mine position</param>
        /// <param name="y">Vertical mine position</param>
        private void SetAdjacentCells(ref Board board, int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                //check the horizontal range of adjacent cell
                if (x + i >= 0 && x + i < width)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //check the verticle range of adjacent cell if cell state is not a mine
                        if (y + j >= 0 && y + j < height && board.getBoard()[x + i][y + j].cell != Cell.MINE)
                        {

                            board.getBoard()[x + i][y + j].cell += 1;

                        }
                    }

                }
            }
        }

        #endregion

    }
}
