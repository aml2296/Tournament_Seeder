using System;


public class Tournament_Seeder
{
    string name;
    database dataB;
    ushort tournamentSIZE = 2;
    ushort tournamentFILL = 0;

    public void BuildTournament(string tournamentName, ushort playerAmt)
    {
        name = tournamentName;
        dataB = new database(playerAmt);
        tournamentSIZE = playerAmt;
    }
    public string[] listOfPlayers()
    {
        if (dataB != null)
        {
            dataB.Position = 0;
            string[] returnStr = new string[dataB.player.Length];
            for (int i = 0; i < dataB.player.Length; i++)
                returnStr[i] = dataB.player[i];
            return returnStr;
        }
        else
            Console.WriteLine("ERROR: " + "Tournament_Seeder" + "|" + "NO DATAB");
        return null;
    }
                                      // Returns array of player names
    public bool addTournamentPLAYER(params string[] names)
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
                                                                //Returns True on successful addition of all players
                                                                //  False on running out of room for all of names[]
                                                                //  and on a null dataB
    public bool checkIfFull()
    {
        if (tournamentFILL < tournamentSIZE)
            return false;
        else
            return true;
    }
    public void updateData()
    {
        dataB.gatherData();
    }
}
