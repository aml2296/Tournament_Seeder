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
            string[] players = new string[NUMBERofPLAYERS] { "Terry", "Amy" , "Jerry" ,"Carlos" };
            Datablock[] arrayOfData = new Datablock[NUMBERofPLAYERS];
            int count = 0;
            foreach(string player in players)
            {
                arrayOfData[count] = new Datablock(player,(ushort)count,(ushort)(NUMBERofPLAYERS-count));
                count++;
            }
            Display dis = new Display();
            ConsoleKey myKey;

            //control.AddDataBlockHost("John",arrayOfData);

            byte[] byteArrayFromDatablock = new byte[30];
            do
            {
                /*
                myKey = Console.ReadKey().Key;
                
                switch()
                {
                case ConsoleKey.Enter:
                    count = 0;
                    Console.WriteLine("Converting to Byte");
                    byteArrayFromDatablock = control.ConvertDataToByte();
                    foreach(byte b in byteArrayFromDatablock)
                    {
                        Console.Write(b + "/");
                        count++;
                        if(count%4 == 0)
                            Console.Write("\n");
                    }
                    break;
                case ConsoleKey.S:
                    control.writeToFolder(byteArrayFromDatablock,control.path,0);
                    break;
                case ConsoleKey.R:
                    int k= 5;
                    control.ReadFiles();
                  
                    break;
                default:
                    break;
                }
                Console.WriteLine("----");
                */
                dis.Run();
            }while (count != 1);
        }
    }
}
