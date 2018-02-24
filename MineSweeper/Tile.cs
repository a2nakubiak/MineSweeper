using System;
using System.Collections.Generic;

namespace MineSweeper
{
    /// <summary>
    /// Cell describe state of the particular tile
    /// </summary>
    public enum Cell
    {
        EMPTY,
        M1,
        M2,
        M3,
        M4,
        M5,
        M6,
        M7,
        M8,
        FLAG,
        MINE,
        CLOSED,
    };

    class Tile
    {
        #region Fields
        public Cell cell;

        //create list adjacenting tiles
        public List<Tuple<int, int>> flagsAround = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> hiddenAround = new List<Tuple<int, int>>();

        #endregion

        public Tile(Cell cell)
        {
            //assign the state of tile to cell variable
            this.cell = cell;
        }

    }
}
