using System;
using System.Collections.Generic;

namespace MineSweeper
{
    class GameSolver
    {
        #region Fields

        //random number
        Random rand = new Random();

        //game settings
        public int? width = null;
        public int? height = null;
        public int minesNumbr;

        //Click location
        int x;
        int y;

        //userBoard
        public Board userBoard;

        //amount of max opened tiles on board to win
        int maxOpenedTiles;
        public List<Tuple<int, int>> closedTilesList = new List<Tuple<int, int>>();

        #endregion

        #region Constructor
        public GameSolver(int? width, int? height, int minesNumbr)
        {
            //get and set information about board size and mines number
            this.width = width ?? default(int);
            this.height = height ?? default(int);
            this.minesNumbr = minesNumbr;
            maxOpenedTiles = (width ?? default(int)) * (height ?? default(int)) - minesNumbr;

            //creat user board
            userBoard = new Board(width ?? default(int), height ?? default(int), new Tile(Cell.CLOSED));

        }
        #endregion

        #region Public methods

        /// <summary>
        /// Draw the first coordinates to open
        /// </summary>
        /// <returns>Position on first tile to open</returns>
        public Tuple<int, int> FirstMove()
        {
            x = rand.Next(0, width ?? default(int));
            y = rand.Next(0, height ?? default(int));

            return new Tuple<int, int>(x, y);
        }

        /// <summary>
        /// Search revealed tiles and flag obvious mines
        /// </summary>
        public void SearchRevealedTilesAndFlagObviousMines()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //enter if it is opened numerical tile
                    if (userBoard.getBoard()[i][j].cell != Cell.CLOSED &&
                        userBoard.getBoard()[i][j].cell != Cell.EMPTY &&
                        userBoard.getBoard()[i][j].cell != Cell.FLAG &&
                         userBoard.getBoard()[i][j].cell != Cell.MINE)
                    {
                        //Open function that flag the tile where is mine
                        FlagAdjacentHiddenTiles(userBoard.getBoard()[i][j], i, j);
                    }
                }
            }
        }

        /// <summary>
        /// Search revealed tiles and check adjacent flag number
        /// </summary>
        /// <returns>New position of tile to open</returns>
        public Tuple<int, int> SearchRevealedTilesAndCheckAdjacentFlagNumber()
        {
            int newX = -1;
            int newY = -1;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (userBoard.getBoard()[i][j].cell != Cell.CLOSED &&
                        userBoard.getBoard()[i][j].cell != Cell.EMPTY &&
                        userBoard.getBoard()[i][j].cell != Cell.FLAG &&
                         userBoard.getBoard()[i][j].cell != Cell.MINE)
                    {
                        //check if number of surrounding flags is equal to number on cell.
                        if (userBoard.getBoard()[i][j].flagsAround.Count == (int)userBoard.getBoard()[i][j].cell)
                        {
                            //open tiles without flags
                            bool hasAdjacents = OpenClosedTilesSurroundedByFlags(ref newX, ref newY, i, j);
                            if (hasAdjacents)
                            {
                                return new Tuple<int, int>(newX, newY);
                            }
                            else continue;

                        }
                    }
                }
            }

            if (ClosedTilesCounter() < maxOpenedTiles)
            {
                int choice = rand.Next(closedTilesList.Count);
                int x = closedTilesList[choice].Item1;
                int y = closedTilesList[choice].Item2;
                return new Tuple<int, int>(x, y);
            }
            return new Tuple<int, int>(-1, -1);
        }

        /// <summary>
        /// Check if all possible tiles are opened if so I win :)
        /// </summary>
        /// <returns>true if all possible tiles are opened</returns>
        public bool checkIfFinish()
        {
            if (maxOpenedTiles <= OpenedTilesCounter())
            {
                Console.WriteLine("Win!");
                return true;
            }
            return false;
        }
        #endregion


        #region Private methods

        /// <summary>
        /// Flag adjacent hidden tiles based on the number of given tile
        /// </summary>
        /// <param name="tile">Numerical tile</param>
        /// <param name="x">Horizontal position of tile</param>
        /// <param name="y">Vertical position of tile</param>
        private void FlagAdjacentHiddenTiles(Tile tile, int x, int y)
        {
            List<Tuple<int, int>> flagLocationList = new List<Tuple<int, int>>();
            int adjacentCounter = 0;

            for (int i = -1; i <= 1; i++)
            {
                if (x + i >= 0 && x + i < width)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //count number of surrounding closed or flag tiles
                        if (y + j >= 0 && y + j < height && (userBoard.getBoard()[x + i][y + j].cell == Cell.CLOSED ||
                            userBoard.getBoard()[x + i][y + j].cell == Cell.FLAG))
                        {
                            flagLocationList.Add(new Tuple<int, int>(x + i, y + j));
                            adjacentCounter++;
                        }
                    }
                }
            }

            //check if number of surrounding closed or flag tiles is equal to numerical value of tile
            //if so all these tiles marks as flag
            if (adjacentCounter == (int)tile.cell)
            {
                for (int i = 0; i < flagLocationList.Count; i++)
                {
                    userBoard.getBoard()[flagLocationList[i].Item1][flagLocationList[i].Item2].cell = Cell.FLAG;
                }
            }
            UpdateAdjacent();

        }

        /// <summary>
        /// Open closed tiles surrounded by flags
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool OpenClosedTilesSurroundedByFlags(ref int newX, ref int newY, int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (x + i >= 0 && x + i < width)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //check all closed tiles
                        if (y + j >= 0 && y + j < height && (userBoard.getBoard()[x + i][y + j].cell == Cell.CLOSED &&
                            userBoard.getBoard()[x + i][y + j].cell != Cell.FLAG))
                        {
                            // return first tile to open
                            newX = x + i;
                            newY = y + j;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Update information about hidden and flagged adjacent tiles
        /// </summary>
        private void UpdateAdjacent()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    userBoard.getBoard()[i][j].hiddenAround.Clear();
                    userBoard.getBoard()[i][j].flagsAround.Clear();

                    if (userBoard.getBoard()[i][j].cell != Cell.CLOSED &&
                        userBoard.getBoard()[i][j].cell != Cell.EMPTY &&
                        userBoard.getBoard()[i][j].cell != Cell.FLAG &&
                         userBoard.getBoard()[i][j].cell != Cell.MINE)
                    {
                        for (int ii = -1; ii <= 1; ii++)
                        {
                            if (i + ii >= 0 && i + ii < width)
                            {
                                for (int jj = -1; jj <= 1; jj++)
                                {
                                    if (j + jj >= 0 && j + jj < height)
                                    {
                                        if (userBoard.getBoard()[i + ii][j + jj].cell == Cell.FLAG)
                                            userBoard.getBoard()[i][j].flagsAround.Add(new Tuple<int, int>(ii, jj));
                                        else if (userBoard.getBoard()[i + ii][j + jj].cell == Cell.CLOSED)
                                            userBoard.getBoard()[i][j].hiddenAround.Add(new Tuple<int, int>(ii, jj));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Count the number of closed tiles
        /// </summary>
        /// <returns>number of closed tiles on board</returns>
        private int ClosedTilesCounter()
        {
            int counter = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (userBoard.getBoard()[i][j].cell == Cell.CLOSED)
                    {
                        counter++;
                        closedTilesList.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return counter;
        }

        /// <summary>
        /// Count the number of opened tiles
        /// </summary>
        /// <returns>number of opened tiles on board</returns>
        private int OpenedTilesCounter()
        {
            int counter = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (userBoard.getBoard()[i][j].cell != Cell.CLOSED && userBoard.getBoard()[i][j].cell != Cell.FLAG)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }

        #endregion
    }
}
