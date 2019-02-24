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
            string[] players = new string[4] { "Terry", "Amy" , "Jerry" ,"Carlos" };
            Datablock[] arrayOfData = new Datablock[players.Length];
            int count = 0;
            foreach(string player in players)
            {
                arrayOfData[count++] = new Datablock(player,(ushort)count,(ushort)count);
            }
            Display dis = new Display();
            ConsoleKey myKey;
            byte[] byteArrayFromDatablock = new byte[50];

            DatablockHOST hostData = new DatablockHOST("Host",arrayOfData);

            do
            {
                myKey = Console.ReadKey().Key;
                switch(myKey)
                {
                case ConsoleKey.Enter:
                    count = 0;
                    Console.WriteLine("Converting to Byte");
                    byteArrayFromDatablock = hostData.ConvertHostToByte();
                    foreach(byte b in byteArrayFromDatablock)
                    {
                        Console.Write(b + "/");
                        count++;
                        if(count%4 == 0)
                            Console.Write("\n");
                    }
                    break;
                case ConsoleKey.Backspace:
                    Console.WriteLine("Converting to Data");
                    DatablockHOST test = new DatablockHOST(byteArrayFromDatablock,0);
                    break;  
                default:
                    break;
                }
                Console.WriteLine("----");
            }while (myKey != ConsoleKey.Escape);
        }
    }
}
