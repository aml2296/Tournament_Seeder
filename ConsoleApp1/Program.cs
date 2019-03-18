using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TObase
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
            const int NUMBERofPLAYERS = 4;

            Database control = new Database(NUMBERofPLAYERS);
            string[] players = new string[NUMBERofPLAYERS] { "Terry", "Amy", "Jerry", "Carlos" };
            Datablock[] arrayOfData = new Datablock[NUMBERofPLAYERS];
            int count = 0;
            foreach (string player in players)
            {
                arrayOfData[count] = new Datablock(player, (ushort)count, (ushort)(NUMBERofPLAYERS - count), 0);
                count++;
            }
            Display dis = new Display();
            ConsoleKey myKey;

            //control.AddDataBlockHost("John", 0, arrayOfData);
            Tournament_Seeder TO = new Tournament_Seeder();
            TO.BuildTournament("Test", 10);
            TO.AddTournamentPLAYER(players);


            byte[] byteArrayFromDatablock = new byte[30];
            do
            {
                myKey = Console.ReadKey().Key;

                switch (myKey)
                {
                    case ConsoleKey.Enter:
                        count = 0;
                        Console.WriteLine("Converting to Byte");
                        byteArrayFromDatablock = control.ConvertDataToByte();
                        foreach (byte b in byteArrayFromDatablock)
                        {
                            Console.Write(b + "/");
                            count++;
                            if (count % 4 == 0)
                                Console.Write("\n");
                        }
                        break;
                    case ConsoleKey.S:
                        control.WriteToFolder(byteArrayFromDatablock, control.path, 0);
                        break;
                    case ConsoleKey.R:
                        control.ReadFiles();
                        break;
                    default:
                        break;
                }
                Console.WriteLine("----");
            } while (myKey != ConsoleKey.Escape);
        }
    }
}
