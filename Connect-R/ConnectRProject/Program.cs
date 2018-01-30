using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectRProject
{
    class Program
    {
        static void Main()
        {
            // Change these values to alter the game
            int turn = 0;
            int rows = 6;
            int columns = 7;
            int r_val = 4;

            GameBoard board = new GameBoard(rows, columns, r_val, true);
    
            Console.Write("Is Player 1 going first? Y/N?");
            string player1Goes = Console.ReadLine();
            board.Turn = player1Goes == "Y" ? 0 : 1;

            while (board.Play())
            {
            }
                  
        }
    }
}
