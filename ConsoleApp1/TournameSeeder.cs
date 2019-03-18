using System;
using System.Collections.Generic;
using AML;

namespace TObase
{
    public class Tournament_Seeder
    {
        string name;
        CharList charList;
        public Database dataB;
        ushort tournamentSIZE = 2;
        ushort tournamentFILL = 0;

        public void BuildTournament(string tournamentName, ushort playerAmt)
        {
            name = tournamentName;
            dataB = new Database(playerAmt);
            tournamentSIZE = playerAmt;
        }
        private string[] ListOfPlayers()
        {
            if (dataB != null)
            {
                //dataB.Position = 0;
                string[] returnStr = new string[dataB.player.Length];
                for (int i = 0; i < dataB.player.Length; i++)
                    returnStr[i] = dataB.player[i];
                return returnStr;
            }
            else
                Console.WriteLine("ERROR: " + "Tournament_Seeder" + "|" + "NO DATAB");
            return null;
        }
        public bool AddTournamentPLAYER(params string[] names)//Returns True on successful addition of all players
        //  False on running out of room for all of names[]
        //  and on a null dataB
        {
            if (dataB != null)
            {
                if (names != null)
                {
                    dataB.player[dataB.Position] = names[0];
                    for (int i = 1; i < names.Length; i++)
                    {
                        if (dataB.Next())
                            dataB.player[dataB.Position] = names[i];
                        else
                            return false;
                    }
                    dataB.Next();
                    tournamentFILL += (ushort)names.Length;
                    return true;
                }
                else
                    Console.WriteLine("Error: " + "Tournament_Seeder" + "|" + "NO NAMES");
            }
            else
                Console.WriteLine("ERROR: " + "Tournament_Seeder" + "|" + "NO DATAB");
            return false;
        }
        public bool CheckIfFull()
        {
            if (tournamentFILL < tournamentSIZE)
                return false;
            else
                return true;
        }
        public void UpdateData()
        {
            
        }


        private Datablock CheckInLibrary(string name)
        {
           
            return null;
        }
        public bool LoadLibrary()
        {
            return dataB.ReadCharLToFile(ref charList);
        }
        public bool SaveLibrary()
        {
            return dataB.WriteCharLToFile(charList);
        }
    }


    public class Iterator
    {
        public int minValue,
            maxValue,
            it;
        public Iterator(int min, int max)
        {
            minValue = new int();
            maxValue = new int();
            it = new int();

            minValue = it = min;
            maxValue = max;

            maxValue = max;
        }
        public Iterator(int count)
        {
            minValue = new int();
            maxValue = new int();
            it = new int();

            minValue = 0;
            it = 0;
            maxValue = count - 1;
        }
        public static Iterator operator ++(Iterator a)
        {
            if (a.it == a.maxValue)
                a.it = a.minValue;
            else
                a.it++;
            return a;
        }
        public static Iterator operator --(Iterator a)
        {
            if (a.it == a.minValue)
                a.it = a.maxValue;
            else
                a.it--;
            return a;
        }
        public int First()
        {    
            return it = minValue;
        }
        public int Last()
        {
            return it = maxValue;
        }
    }  //Returns an int for position between "minValue" and "maxValue" 

}