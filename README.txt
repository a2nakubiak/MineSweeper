====== MineSweeper Game Description ======

MineSweeper Game consists of 5 classes:
1. Tile - keeps the state of one tile
2. Board - contains basic information about size of a board, mines' number and function to fill and display any board.
3. GameBoard - creats a board to a game, sets the mines' positions and opens tiles on user board
4. GameSolver - sloves the game and contains algorithms of strategy
5. Program - manages the game

The Program Class creates a GameBoard object and a GameSolver object.The GameBoard object is a base of the game and its Constructor takes parameters of Width, Hight and minesNumber (to game was more flexible). The GameSolver receives this information thanks to references. Thanks to this solution, two objects are separated. The Solver has to take informatiom about the board from the GameBoard.

====== Strategy ======
First move is chosen when on a board there are no mines, so player doesn't lose on the first move. The position of the first move is saved and in surrounding area of this position algorithm does not generate mines.After each move, board is displayed and based on the opened tiles algorithm makes a decision where to open a new tile.

====== Algorithm ======
1. Search the opened numerical tiles, i.e. 1-8.
2. Count the number of closed neighbors of any given tile
3. If number of hidden neighboring tiles is equal to the number in the current tile then, it means all the adjacent tiles are mines so must set a flag
4. Based on the number of flagged tiles and the number in the current tile check if the numbers are equal. If for the current tile exists any adjacent hidden tile it means, there must not be a mine - we can open it.
5. If the above solution doesn't find the new position to open then, the algorithm draws one closed tile on the board and opens it - This is the one moment when player (solver) can lose the game due to randomness of the event. 
6. Algorithm is performed until solver chooses a mine tile or opens all possible tiles.
