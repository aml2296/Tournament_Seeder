using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    enum mod
    {
        main,
        bracket,
        graph,
        data
    }

    class Program
    {
        static void Main(string[] args)
        {
            Tournament_Seeder baseSeed = new Tournament_Seeder();
            display dis = new display();
            mod programMode = mod.main;
            do
            {
                switch(programMode)
                {
                    case mod.main:
                        break;
                    case mod.bracket:
                        break;
                    case mod.data:
                        break;
                    case mod.graph:
                        break;
                }
                dis.start();
            } while (Console.ReadKey().Key != ConsoleKey.X);
        }
    }
}
