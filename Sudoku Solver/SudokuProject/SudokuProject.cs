using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject
{
    public class SudokuProject
    {
        public static void Main()
        {
            short[,] puzzle1 = new short[9, 9];
            short[,] puzzle2 = new short[9, 9];
            short[,] puzzle3 = new short[9, 9];

            #region Puzzle 1 Value Declaration

            puzzle1[0, 2] = 4;
            puzzle1[0, 6] = 2;
                puzzle1[1, 0] = 1;
                puzzle1[1, 1] = 7;
                puzzle1[1, 5] = 3;
            puzzle1[2, 0] = 5;
            puzzle1[2, 1] = 8;
            puzzle1[2, 8] = 6;
                puzzle1[3, 0] = 4;
                puzzle1[3, 3] = 3;
                puzzle1[3, 7] = 5;
            puzzle1[4, 4] = 8;
            puzzle1[4, 5] = 2;
            puzzle1[4, 6] = 3;
            puzzle1[4, 7] = 4;
                puzzle1[5, 0] = 2;
                puzzle1[5, 1] = 5;
                puzzle1[5, 6] = 1;
                puzzle1[5, 7] = 7;
            puzzle1[6, 6] = 4;
                puzzle1[7, 2] = 5;
                puzzle1[7, 3] = 6;
                puzzle1[7, 4] = 4;
                puzzle1[7, 5] = 7;
            puzzle1[8, 6] = 7;
            puzzle1[8, 8] = 1;

            #endregion

            #region Puzzle 2 Value Declaration

            puzzle2[0, 1] = 1;
                puzzle2[1, 2] = 4;
                puzzle2[1, 3] = 2;
                puzzle2[1, 7] = 5;
                puzzle2[1, 8] = 1;
            puzzle2[2, 4] = 9;
            puzzle2[2, 5] = 1;
            puzzle2[2, 8] = 7;
                puzzle2[3, 1] = 9;
                puzzle2[3, 3] = 4;
                puzzle2[3, 5] = 3;
                puzzle2[3, 7] = 2;
            puzzle2[4, 0] = 7;
            puzzle2[4, 4] = 1;
            puzzle2[4, 5] = 5;
            puzzle2[4, 7] = 3;
                puzzle2[6, 1] = 5;
                puzzle2[6, 3] = 9;
                puzzle2[6, 5] = 2;
                puzzle2[6, 6] = 1;
                puzzle2[6, 7] = 4;
            puzzle2[7, 0] = 4;
            puzzle2[7, 2] = 2;
            puzzle2[7, 6] = 6;
            puzzle2[7, 7] = 8;
                puzzle2[8, 4] = 8;
                puzzle2[8, 8] = 5;

            #endregion

            #region Puzzle 3 Value Declaration

            puzzle3[1, 0] = 8;
            puzzle3[1, 3] = 1;
            puzzle3[1, 4] = 2;
            puzzle3[1, 5] = 4;
            puzzle3[1, 6] = 9;
                puzzle3[2, 0] = 9;
                puzzle3[2, 1] = 1;
                puzzle3[2, 2] = 2;
                puzzle3[2, 4] = 8;
            puzzle3[3, 0] = 1;
            puzzle3[3, 4] = 6;
            puzzle3[3, 5] = 3;
            puzzle3[3, 6] = 8;
            puzzle3[3, 7] = 9;
                puzzle3[4, 7] = 1;
                puzzle3[4, 8] = 7;
            puzzle3[5, 2] = 7;
            puzzle3[5, 3] = 5;
            puzzle3[5, 6] = 3;
            puzzle3[5, 7] = 2;
                puzzle3[6, 1] = 3;
                puzzle3[6, 5] = 6;
                puzzle3[6, 6] = 5;
                puzzle3[6, 8] = 8;
            puzzle3[7, 0] = 4;
            puzzle3[7, 7] = 6;
                puzzle3[8, 1] = 9;

            #endregion

            // Load in one of the puzzles.
            //Puzzle myPuzzle = new Puzzle(puzzle1);
            //Puzzle myPuzzle = new Puzzle(puzzle2);
            Puzzle myPuzzle = new Puzzle(puzzle3);

            // Prints out the initial puzzle
            Console.WriteLine("The Initial Puzzle: \n");
                myPuzzle.Print();
            Console.WriteLine("\n=====================\n");

            //Solves the puzzle and prints the "solution"
            Console.WriteLine("The PC's Solution: \n");
                myPuzzle.Solve();
                myPuzzle.Print();

            // The number of grid spaces filled
            Console.WriteLine($"\nThe number of spaces filled is: {myPuzzle.Count()}/81.\n");
        }
        
    }
}
