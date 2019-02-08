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
        compare,
        data
    }

    class Program
    {
        static void Main(string[] args)
        {
            Tournament_Seeder baseSeed = new Tournament_Seeder();
            Display dis = new Display();
            mod programMode = mod.main;
            dis.BuildMainMenu();
            do
            {
                switch(programMode)
                {
                    case mod.main:
                        if (dis.getMode != mod.main)
                            dis.BuildMainMenu();
                        break;
                    case mod.bracket:
                        break;
                    case mod.data:
                        break;
                    case mod.compare:
                        break;
                }
                dis.update();
            } while (Console.ReadKey().Key != ConsoleKey.X);
        }
    }
}
