using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject
{

    class Puzzle
    {
        // All possible values in a given grid space
        public static readonly string PossibleValues = "123456789";

        // The sudoku grid of the Puzzle object
        public short[,] SudokuGrid { get; set; }

        // Intializes an empty puzzle
        public Puzzle()
        {
            SudokuGrid = new short[9,9];
        }

        // Creates a new puzzle based off of a passed in short[9,9] array
        public Puzzle(short[,] g)
        {
            this.SudokuGrid = g;
        }

        /// <summary>
        /// The puzzle solver, which puts values into the grid spaces.
        /// Finds single value spaces, then puts available numbers into each spot.
        /// </summary>
        public void Solve()
        {
            // First we check for squares with only one possible value.
            for (int row = 0; row < 9; row++)
            {            
                for (int column = 0; column < 9; column++)
                {
                    if (this.SudokuGrid[row, column] == 0)
                    {
                        // An empty string that houses all the values which don't violate the rules
                        string values = String.Empty;
                        var v = this.SudokuGrid[row, column];

                        // For each possible value (1-9), check to see if one or more of them follow the rules of sudoku.
                        // If it does, then append it to the string.
                        foreach (char c in PossibleValues)
                        {                     
                            this.SudokuGrid[row, column] = (short)Char.GetNumericValue(c);

                            if (this.CheckRows() && this.CheckColumns() && this.CheckBoxes())
                            {
                                values += (short)Char.GetNumericValue(c);
                            }
                        }
                        this.SudokuGrid[row, column] = v;
                        // If there was only one possible value for the square, then place that value there.
                        if (values.Length == 1)
                        {
                            this.SudokuGrid[row, column] = Int16.Parse(values);
                        }
                    }
                }
            }
            // Now, we figure out the values which can go in the square and place them there at random
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (this.SudokuGrid[row, column] == 0)
                    {
                        List<short> values = new List<short>();
                        var v = this.SudokuGrid[row, column];

                        // For each possible value (1-9), check to see if one or more of them follow the rules of sudoku.
                        // If so, add it to the list
                        foreach (char c in PossibleValues)
                        {
                            this.SudokuGrid[row, column] = (short)Char.GetNumericValue(c);

                            if (this.CheckRows() && this.CheckColumns() && this.CheckBoxes())
                            {
                                values.Add((short)Char.GetNumericValue(c));
                            }
                        }
                        // "The first value I can think of will go here!" Said the idiot computer.
                        this.SudokuGrid[row, column] = values.FirstOrDefault();
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if all the values in every row are unique.
        /// If so, return true
        /// If not, return false
        /// </summary>
        public bool CheckRows()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    for (int j = column + 1; j < 9; j++)
                    {
                        if (this.SudokuGrid[row, column] == 0)
                        {
                            
                        }
                        else if (this.SudokuGrid[row, column] == this.SudokuGrid[row, j])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks to see if all the values in every column are unique.
        /// If so, return true
        /// If not, return false
        /// </summary>
        public bool CheckColumns()
        {
            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    for (int j = row + 1; j < 9; j++)
                    {
                        if (this.SudokuGrid[row, column] == 0)
                        {

                        }
                        else if (this.SudokuGrid[row, column] == this.SudokuGrid[j, column])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks to see if all the values in every box are unique.
        /// If so, return true
        /// If not, return false
        /// </summary>
        public bool CheckBoxes()
        {
            // Declare strings named 'box'
            string box1 = String.Empty, box2 = String.Empty, box3 = String.Empty, box4 = String.Empty, box5 = String.Empty, box6 = String.Empty, box7 = String.Empty, box8 = String.Empty, box9 = String.Empty;

            // Values from each box space are added to its respective string. Then the elements of each of these strings are checked for uniqueness.
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    box1 += $"{this.SudokuGrid[r, c].ToString()}";
                }
                for (int c = 3; c < 6; c++)
                {
                    box2 += $"{this.SudokuGrid[r, c].ToString()}";
                }
                for (int c = 6; c < 9; c++)
                {
                    box3 += $"{this.SudokuGrid[r, c].ToString()}";
                }              
            }
            for (int r = 3; r < 6; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    box4 += $"{this.SudokuGrid[r, c].ToString()}";
                }
                for (int c = 3; c < 6; c++)
                {
                    box5 += $"{this.SudokuGrid[r, c].ToString()}";
                }
                for (int c = 6; c < 9; c++)
                {
                    box6 += $"{this.SudokuGrid[r, c].ToString()}";
                }
            }
            for (int r = 6; r < 9; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    box7 += $"{this.SudokuGrid[r, c].ToString()}";
                }
                for (int c = 3; c < 6; c++)
                {
                    box8 += $"{this.SudokuGrid[r, c].ToString()}";
                }
                for (int c = 6; c < 9; c++)
                {
                    box9 += $"{this.SudokuGrid[r, c].ToString()}";
                }
            }

            // If the numbers contained in each box string are unique, return true.
            if (IsUnique(box1) && IsUnique(box2) && IsUnique(box3) && IsUnique(box4) && IsUnique(box5) && IsUnique(box6) && IsUnique(box7) && IsUnique(box8) && IsUnique(box9))
            {
                return true;
            }
            else
            {
                return false;
            }
         }

        // Prints the entire Sudoku Grid to the console in a sudoku-esque format
        public void Print()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (column == 2 || column == 5)
                    {
                        if (this.SudokuGrid[row, column] == 0)
                        {
                            Console.Write("? \t");
                        }
                        else
                        {
                            Console.Write($"{this.SudokuGrid[row, column]} \t");
                        }
                    }
                    else
                    {
                        if(this.SudokuGrid[row, column] == 0)
                        {
                            Console.Write("? ");
                        }
                        else
                        {
                            Console.Write($"{this.SudokuGrid[row, column]} ");
                        }
                    }
                }
                Console.WriteLine();
                if (row == 2 || row == 5)
                {
                    Console.WriteLine();
                }
            }
 
        }

        // Produces the count of the amount of filled grid spaces with valid values.
        public int Count()
        {
            int count = 0;

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (this.SudokuGrid[row, column] != 0)
                        count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Takes in a string and checks each char for uniqueness by adding it to a dictionary and checking if the dictionary key already exists.
        /// If not, adds that value to the dictionary; where the key is the char and the value is a 1
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsUnique(string s)
        {
            Dictionary<char, int> d = new Dictionary<char, int>();

            foreach (char c in s)
            {
                if (c == '0')
                {
                    
                }
                else if (d.ContainsKey(c))
                    return false;
                else
                    d.Add(c, 1);
            }
            return true;
        }
    }
}
