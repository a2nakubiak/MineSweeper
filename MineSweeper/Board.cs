using System;
using System.Collections.Generic;

namespace MineSweeper
{
    class Board
    {
        #region Fields

        //random number
        Random rand = new Random();

        //board size
        private int width;
        private int height;
        private List<List<Tile>> board = new List<List<Tile>>();

        #endregion

        #region Construtcor
        public Board(int width, int height, Tile tile)
        {
            this.width = width;
            this.height = height;

            //board initialization
            //fill board base on the taken arguments
            FillBoard(tile);
        }
        #endregion

        /// <summary>
        /// Allow GetSize outside of this class
        /// </summary>
        /// <param name="width">Horizontal board size</param>
        /// <param name="height">Vertical board size</param>
        #region Public methods
        public void GetSize(ref int? width, ref int? height)
        {
            width = this.width;
            height = this.height;
        }

        /// <summary>
        /// Allow GetBoard outside of this class
        /// </summary>
        /// <returns>2dimensional board of tiles</returns>
        public List<List<Tile>> getBoard()
        {
            return this.board;
        }

        /// <summary>
        /// Display board on the console output
        /// </summary>
        public void DisplayBoard()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (board[i][j].cell != Cell.CLOSED)
                    {
                        if (board[i][j].cell == Cell.EMPTY)
                            Console.Write(".");
                        else if (board[i][j].cell == Cell.FLAG)
                            Console.Write("F");
                        else if (board[i][j].cell == Cell.MINE)
                            Console.Write("M");
                        else Console.Write((int)board[i][j].cell);
                    }
                    else
                    {
                        Console.Write("C");
                    }

                }
                Console.WriteLine();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Fill board base on the taken arguments
        /// Board initialization
        /// </summary>
        /// <param name="tile">Type of board filling</param>
        private void FillBoard(Tile tile)
        {
            for (int i = 0; i < width; i++)
            {
                List<Tile> sublist = new List<Tile>();
                for (int j = 0; j < height; j++)
                {
                    sublist.Add(new Tile(tile.cell));
                }
                board.Add(sublist);
            }
        }

        #endregion
    }
}
