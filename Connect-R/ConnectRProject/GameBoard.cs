using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace ConnectRProject
{
    [Serializable()]
    public class GameBoard
    {
        public bool GameOn;
        public bool BlackWin;
        public bool RedWin;
        public bool AiOn;
        private char[,] Board { get; set; }
        private int[] Height;
        private int R_VALUE;
        private int COLUMNS;
        private int ROWS;
        private int BoardScore;
        public int Turn;
        private Collection<GameBoard> PossibleMoves;     

        /// <summary>
        /// Initializer for the GameBoard class
        /// </summary>
        /// <param name="m">Number of rows in the board</param>
        /// <param name="n">Number of columns in the board</param>
        /// <param name="r">Number in as row to win</param>
        /// <param name="isAi">Is this game using an AI as the second player</param>
        public GameBoard(int m, int n, int r, bool isAi = false)
        {
            this.ROWS = m;
            this.COLUMNS = n;
            this.Board = new char[m, n];
            this.Height = Enumerable.Repeat(ROWS, COLUMNS).ToArray();
            this.R_VALUE = r;
            this.GameOn = true;
            this.Turn = 0;
            this.PossibleMoves = new Collection<GameBoard>();
            this.BlackWin = false;
            this.RedWin = false;
            this.AiOn = isAi;
        }

        /// <summary>
        /// Plays the game
        /// </summary>
        /// <returns>Returns whether or not the game is still ongoing</returns>
        public bool Play()
        {
            this.Print();

            while (this.GameOn)
            {

                // An even turn denotes the human player 1
                if (this.Turn % 2 == 0)
                {
                    Console.Write("Player 1, please enter the column you'd like to place a piece: ");
                    string inp = Console.ReadLine();
                    int move = Int32.Parse(inp);

                    if (Enumerable.Range(0, COLUMNS + 1).Contains(move))
                    {
                        if (this.NextMove(move, 'R'))
                            this.Turn++;
                    }
                    else
                    {
                        Console.WriteLine("Your input was out of the valid range!");
                    }
                }
                // An odd turn and AI being on denotes the AI playing the odd turns
                else if(this.Turn % 2 != 0 && this.AiOn)
                {
                    int x = MiniMax(this, 3, true);
                    int max = this.PossibleMoves.Max(i => i.BoardScore);
                    var gb = this.PossibleMoves.First(s => s.BoardScore == max);
                    this.Board = gb.Board;
                    this.CheckGameOver();
                    this.Turn++;
                }
                // An odd turn when AI is not on is a second human player
                else
                {
                    Console.Write("\nPlayer 2, please enter the column you'd like to place a piece: ");
                    string ans = Console.ReadLine();
                    int a = Int32.Parse(ans);

                    if (Enumerable.Range(0, COLUMNS + 1).Contains(a))
                    {
                        if (this.NextMove(Int32.Parse(ans), 'B'))
                            this.Turn++;
                    }
                    else
                    {
                        Console.WriteLine("Your input was out of the valid range!");
                    }
                }
                this.Print();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Place the next piece into the board
        /// </summary>
        /// <param name="gb">The gameboard where the piece is going</param>
        /// <param name="input">The specific column the piece is being placed</param>
        /// <returns>If the move was valid, return true</returns>
        public bool NextMove(GameBoard gb, int input)
        {
            input--;

            if (Height[input] == 0)
            {
                return false;
            }

            // Place piece in column with respect to the int given
            gb.Board[(Height[input] - 1), input] = gb.Turn % 2 == 0 ? 'R' : 'B';
            Height[input]--;
            gb.Turn++;

            CheckGameOverState();

            return true;
        }

        /// <summary>
        /// For use with human players, places a piece of Char into a spot of Int
        /// </summary>
        /// <param name="input">Column that the piece is going in</param>
        /// <param name="piece">Char that is being placed there</param>
        /// <returns>True if the move is valid, else false</returns>
        public bool NextMove(int input, char piece)
        {
            input--;

            if (Height[input] == 0)
            {
                Console.WriteLine("You can't place anymore pieces here!");
                return false;
            }

            // Place piece in column with respect to the int given
            this.Board[(Height[input] - 1), input] = piece;
            Height[input]--;

            CheckGameOver();

            return true;
        }

        /// <summary>
        /// Checks for a game over, then prints out the winning board
        /// </summary>
        /// <returns>True if there exists a row, column, or diagonal with R_VALUE pieces in a row</returns>
        public bool CheckGameOver()
        {
            //Vertical check
            for (int c = 0; c < COLUMNS; c++)
            {
                string vert = String.Empty;

                for (int r = 0; r < ROWS; r++)
                {
                    vert += this.Board[r, c];
                }

                if(this.CheckForWin(vert))
                    return true;
            }

            //Horizontal Check
            for (int r = 0; r < ROWS; r++)
            {
                string horiz = String.Empty;

                for (int c = 0; c < COLUMNS; c++)
                {
                    horiz += this.Board[r, c];
                }

                if(this.CheckForWin(horiz))
                    return true;
            }

            //Positive Slope Check
            for (int line = 1; line <= (ROWS + COLUMNS - 1); line++)
            {
                String diag = String.Empty;
                    
                int start_col = Math.Max(0, line - ROWS);
                int count = Math.Min(Math.Min(line, (COLUMNS - start_col)), ROWS);

                for (int j = 0; j < count; j++)
                    diag += this.Board[(Math.Min(ROWS, line) - j - 1), (start_col + j)];

                if(this.CheckForWin(diag))
                    return true;
            }

            //Negative Slope Check
            for (int line = 1; line <= (ROWS + COLUMNS - 1); line++)
            {
                String diag = String.Empty;

                int start_col = Math.Max(0, line - ROWS);
                int count = Math.Min(Math.Min(line, (COLUMNS - start_col)), ROWS);

                var temp = this.Board;
                temp = FlipArray(temp);

                for (int j = 0; j < count; j++)
                    diag += temp[(Math.Min(ROWS, line) - j - 1), (start_col + j)];

                if(this.CheckForWin(diag))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks for game over same as the previous function, however this one is specifically meant
        /// for the AI's use in building a tree
        /// </summary>
        /// <returns>True if there exists a row, column, or diagonal with R_VALUE pieces in a row</returns>
        public bool CheckGameOverState()
        {
            //Vertical check
            for (int c = 0; c < COLUMNS; c++)
            {
                string vert = String.Empty;

                for (int r = 0; r < ROWS; r++)
                {
                    vert += this.Board[r, c];
                }

                if (this.CheckForWinState(vert))
                    return true;
            }

            //Horizontal Check
            for (int r = 0; r < ROWS; r++)
            {
                string horiz = String.Empty;

                for (int c = 0; c < COLUMNS; c++)
                {
                    horiz += this.Board[r, c];
                }

                if (this.CheckForWinState(horiz))
                    return true;
            }

            //Diagonal-Right Check
            for (int line = 1; line <= (ROWS + COLUMNS - 1); line++)
            {
                String diag = String.Empty;

                int start_col = Math.Max(0, line - ROWS);
                int count = Math.Min(Math.Min(line, (COLUMNS - start_col)), ROWS);

                for (int j = 0; j < count; j++)
                    diag += this.Board[(Math.Min(ROWS, line) - j - 1), (start_col + j)];

                if (this.CheckForWinState(diag))
                    return true;
            }

            //Diagonal-Left Check
            for (int line = 1; line <= (ROWS + COLUMNS - 1); line++)
            {
                String diag = String.Empty;

                int start_col = Math.Max(0, line - ROWS);
                int count = Math.Min(Math.Min(line, (COLUMNS - start_col)), ROWS);

                var temp = this.Board;
                temp = FlipArray(temp);

                for (int j = 0; j < count; j++)
                    diag += temp[(Math.Min(ROWS, line) - j - 1), (start_col + j)];

                if (this.CheckForWinState(diag))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Looks at the board and scores each line based on the evaluation criteria.
        /// </summary>
        /// <param name="gb">Gameboard being passed in</param>
        public void ScoreBoard(GameBoard gb)
        {
            //Vertical check
            for (int c = 0; c < COLUMNS; c++)
            {
                string vert = String.Empty;

                for (int r = 0; r < ROWS; r++)
                {
                    vert += gb.Board[r, c];
                }

                ScoreLine(gb, vert);                               
            }

            //Horizontal Check
            for (int r = 0; r < ROWS; r++)
            {
                string horiz = String.Empty;

                for (int c = 0; c < COLUMNS; c++)
                {
                    horiz += gb.Board[r, c];
                }

                ScoreLine(gb, horiz);
            }

            //Diagonal-Right Check
            for (int line = 1; line <= (ROWS + COLUMNS - 1); line++)
            {
                String diag = String.Empty;

                int start_col = Math.Max(0, line - ROWS);
                int count = Math.Min(Math.Min(line, (COLUMNS - start_col)), ROWS);

                for (int j = 0; j < count; j++)
                    diag += gb.Board[(Math.Min(ROWS, line) - j - 1), (start_col + j)];
                  
                ScoreLine(gb, diag);
            }

            //Diagonal-Left Check
            for (int line = 1; line <= (ROWS + COLUMNS - 1); line++)
            {
                String diag = String.Empty;

                int start_col = Math.Max(0, line - ROWS);
                int count = Math.Min(Math.Min(line, (COLUMNS - start_col)), ROWS);

                var temp = gb.Board;
                temp = FlipArray(temp);

                for (int j = 0; j < count; j++)
                    diag += temp[(Math.Min(ROWS, line) - j - 1), (start_col + j)];

                ScoreLine(gb, diag);
            }
        }

        /// <summary>
        /// Finds all possible moves for a given gameboard and adds them as children
        /// </summary>
        /// <param name="gb">The gameboard we want to find the possible moves for</param>
        public void AddMoves(GameBoard gb)
        {
            Collection<GameBoard> temp = new Collection<GameBoard>();

            for (int m = 1; m < COLUMNS; m++)
            {
                GameBoard tempBoard = new GameBoard(ROWS, COLUMNS, R_VALUE);
                tempBoard = DeepCopy(gb);

                if (tempBoard.NextMove(tempBoard, m))
                {
                    temp.Add(tempBoard);
                }
            }

            gb.PossibleMoves = temp;
        }

        /// <summary>
        /// Initiates the minimax algorithm operation
        /// </summary>
        /// <param name="gb">The current game state we are utilizing</param>
        /// <param name="depth">The depth we want to generate the tree to</param>
        /// <param name="max">Whether the current player is the MAX player or MIN player</param>
        /// <returns>Return the repsective score of the gameboard</returns>
        public int MiniMax(GameBoard gb, int depth, bool max)
        {
            AddMoves(gb);

            if (depth == 0 || gb.CheckGameOverState())
            {
                ScoreBoard(gb);
                return gb.BoardScore;
            }

            if (max)
            {
               int bestScore = Int32.MinValue;

                for(int i = 0; i < gb.PossibleMoves.Count; i++)
                {
                    var v = MiniMax(gb.PossibleMoves[i], depth - 1, false);
                    bestScore = Math.Max(bestScore, v);
                }
                return bestScore;
            }
            else
            {
               int bestScore = Int32.MinValue;

                for (int i = 0; i < gb.PossibleMoves.Count; i++)
                {
                    var v = MiniMax(gb.PossibleMoves[i], depth - 1, true);
                    bestScore = Math.Min(bestScore, v);
                }
                return bestScore;
            }
        }

        /// <summary>
        /// Whether or not a gameboard node is a leaf node
        /// </summary>
        /// <returns>True if the node is a leaf node</returns>
        public bool IsLeafState()
        {
            if (this.PossibleMoves.Count == 0)
            {
                return true;
            }

            return false;
        }

        // Prints the current gameboard
        public void Print()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (this.Board[i, j] == '\0')
                    {
                        Console.Write("* ");
                    }
                    else
                    {
                        Console.Write(this.Board[i, j] + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Check each line in the board for a win
        /// </summary>
        /// <param name="s">The line as a string to be checked</param>
        /// <returns>Return true if there is a win</returns>
        public bool CheckForWin(string s)
        {
            string reds = String.Empty;
            string blacks = String.Empty;

            for (int x = 0; x < R_VALUE; x++)
            {
                reds += "R";
            }

            for (int x = 0; x < R_VALUE; x++)
            {
                blacks += "B";
            }

            Regex redCheck = new Regex(reds);
            Regex blackCheck = new Regex(blacks);

            if (redCheck.Matches(s).Count >= 1)
            {
                GameOn = false;
                RedWin = true;
                BlackWin = false;
                Console.WriteLine("Red Wins!");
                return true;
            }
            if (blackCheck.Matches(s).Count >= 1)
            {
                GameOn = false;
                BlackWin = true;
                RedWin = false;
                Console.WriteLine("Black Wins!");
                return true;
            }

            return false;
        }

        // Same as the CheckForWin function, but for the use of the AI
        public bool CheckForWinState(string s)
        {
            string reds = String.Empty;
            string blacks = String.Empty;

            for (int x = 0; x < R_VALUE; x++)
            {
                reds += "R";
            }

            for (int x = 0; x < R_VALUE; x++)
            {
                blacks += "B";
            }

            Regex redCheck = new Regex(reds);
            Regex blackCheck = new Regex(blacks);

            if (redCheck.Matches(s).Count >= 1)
            {
                return true;
            }
            if (blackCheck.Matches(s).Count >= 1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Scores the given line according to the evaluation criteria
        /// </summary>
        /// <param name="gb">The gameboard we're using</param>
        /// <param name="s">The string to be scored</param>
        public void ScoreLine(GameBoard gb, string s)
        {
            string reds = String.Empty;
            string blacks = String.Empty;

            if (gb.CheckGameOverState() && RedWin)
            {
                gb.BoardScore -= (1000 / R_VALUE);
            }

            if (gb.CheckGameOverState() && BlackWin)
            {
                gb.BoardScore += (1000 / R_VALUE);
            }

            for (int x = 0; x < R_VALUE; x++)
            {
                reds += "R";
                Regex redCheck = new Regex(reds);

                if (redCheck.Matches(s).Count >= 1)
                {
                    gb.BoardScore += (100 / R_VALUE);
                }
            }

            for (int x = 0; x < R_VALUE; x++)
            {
                blacks += "B";
                Regex blackCheck = new Regex(blacks);

                if (blackCheck.Matches(s).Count >= 1)
                {
                    gb.BoardScore += (100 / R_VALUE);
                }
            }

        }

        // Deep copy for one GameBoard tree to another
        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        // Method to flip an array like a mirror, used to look at the negative slope of a gameboard
        public char[,] FlipArray(char[,] arrayToFlip)
        {
            int rows = ROWS;
            int columns = COLUMNS;
            char[,] flippedArray = new char[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    flippedArray[i, j] = arrayToFlip[(rows - 1) - i, j];
                }
            }
            return flippedArray;
        }
    }

}