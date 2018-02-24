using System;

namespace MineSweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            const int gameBoardWidth = 10;
            const int gameBoardHeight = 10;
            const int mines = 10;

            int? width = null;
            int? height = null;

            Tuple<int, int> firstMovePosition;
            Tuple<int, int> openTilesAdjacentedWithFlag;

            //cell
            Cell currentCell;

            //create GameBoard object
            GameBoard game = new GameBoard(gameBoardWidth, gameBoardHeight, mines);

            //setting width and hight
            game.board.GetSize(ref width, ref height);

            //create new GameSolver object
            GameSolver solver = new GameSolver(width, height, mines);

            Console.WriteLine("Start Game");
            //Display hidden board
            solver.userBoard.DisplayBoard();
            Console.WriteLine();

            //first random move - its position
            firstMovePosition = solver.FirstMove();

            //initialization of new board with mines and adjacents numbers
            game.Open(ref solver.userBoard, firstMovePosition);

            //display board after first move
            Console.WriteLine("Opened position: x = " + firstMovePosition.Item1

+ " y= " + firstMovePosition.Item2);
            solver.userBoard.DisplayBoard();
            Console.WriteLine();

            do
            {
                //add flags to obvious tiles with mines
                solver.SearchRevealedTilesAndFlagObviousMines();

                //assign new position of tile to open
                openTilesAdjacentedWithFlag =

solver.SearchRevealedTilesAndCheckAdjacentFlagNumber();

                Console.WriteLine("Opened position: x = " +

openTilesAdjacentedWithFlag.Item1 + " y= " + openTilesAdjacentedWithFlag.Item2);

                //board update and return current value of chosen Cell
                currentCell = game.Open(ref solver.userBoard,

openTilesAdjacentedWithFlag);

                //check if currentCell is a mine then I loose :(
                if (currentCell == Cell.MINE)
                {
                    Console.WriteLine("GAME OVER ");
                    solver.userBoard.DisplayBoard();
                    break;
                }

                //check if win :)
                if (solver.checkIfFinish())
                {
                    solver.userBoard.DisplayBoard();
                    break;
                }

                solver.userBoard.DisplayBoard();

                Console.WriteLine();
                //Console.ReadKey();


            } while (true);

            //Display game board to check if the result is correct
            Console.WriteLine();
            Console.WriteLine("Revealed Board");
            game.board.DisplayBoard();
            Console.ReadKey();

        }

    }
}
