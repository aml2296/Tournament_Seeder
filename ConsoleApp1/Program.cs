using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    enum Mod
    {
        main,
        bracket,
        compare,
        data,
        EXIT
    }

    class Program
    {
        static void Main(string[] args)
        {
            Tournament_Seeder host = new Tournament_Seeder();
            string[] players = new string[5] { "Terry", "Jim", "Frank", "Rose", "Amy" };
            Display dis = new Display();

            do
            {
                host.BuildTournament("tournament 1", 5);
                host.addTournamentPLAYER(players);

                byte[] temp = host.dataB.data[0].toByte;
                for(int i = 0; i < temp.Length; i++)
                    Console.WriteLine(temp[i]);
            }while (Console.ReadKey().Key != ConsoleKey.X);
        }
    }
}
