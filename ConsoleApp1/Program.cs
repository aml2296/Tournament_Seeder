using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputSTRING;
            ushort playerTotal = 5;

            Tournament_Seeder baseSeed = new Tournament_Seeder();
            baseSeed.BuildTournament("test", 5);
            string[] list = baseSeed.listOfPlayers();
            for (int i = 0; i < playerTotal; i++)
                Console.WriteLine(list[i]);
            do
            {
                if (baseSeed.checkIfFull())
                {
                    Console.WriteLine("UpdateData? Hit <Enter>");
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                        baseSeed.updateData();
                }
                else
                {
                    Console.WriteLine("Input player names!");
                    inputSTRING = Console.ReadLine().Split();
                    baseSeed.addTournamentPLAYER(inputSTRING);
                }

            } while (Console.ReadKey().Key != ConsoleKey.X);
        }
    }
}
