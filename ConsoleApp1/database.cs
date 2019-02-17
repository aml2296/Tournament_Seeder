using System;

namespace ConsoleApp1
{
    public class Database
    {
        Iterator it;
        int numberOfDatablocks = 0;
        public string[] player;
        public DatablockHOST[] data;
        private int[] byteOffsets;
        string baseFolderName = @"c:\";
        string path;

        public Database(int size)
        {
            it = new Iterator(size);
            byteOffsets = new int[size];
            player = new string[size];
            for (int i = 0; i < size; i++)
                player[i] = "OPEN_SLOT";
            numberOfDatablocks = size - 1;
            data = new DatablockHOST[size];
            path = System.IO.Path.Combine(baseFolderName, "data");
        }

        public int Position
        {
            get
            {
                return it.it;
            }
            set
            {
                if (value >= it.minValue && value <= it.maxValue)
                    it.it = value;
                else
                {
                    if (value > it.maxValue)
                        it.it = it.maxValue;
                    else
                        it.it = it.minValue;
                }
            }
        }
        public bool Next()
        {
            if (it.it == it.maxValue)
                return false;
            else if (it.it < it.maxValue)
                it++;
            return true;
        }
        public bool Prev()
        {
            if (it.it == it.minValue)
                return false;
            else if (it.it > it.maxValue)
                it--;
            return true;
        }
        public DatablockHOST this[int i]
        {
            get
            {
                if(i >= it.minValue && i <= it.maxValue)
                    return data[i];
            }
        }

        private bool CheckForFolder()
        {
            if (!System.IO.File.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                if (!System.IO.File.Exists(path))
                {
                    Console.WriteLine("ERROR: " + "DATABASE" + "|" + "FOLDER CREATION");
                    return false;
                }
            }
            return true;
        }
        public bool Contains(string str)
        {
            for(int i = 0; i< data.Length;i++ )
            {
                if(data[i].NameOf == str)
                    return true;
            }
            return false;
        }
        public void GatherData()
        {
            if (CheckForFolder())
            {
                string fileName = "source.txt";
                path = System.IO.Path.Combine(path, fileName);
                if (!System.IO.File.Exists(path))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(path))
                    {
                        int offset = 0;
                        for (int i = 0; i < data.Length; i++)
                        {
                            //byte[] dataByte = data[i].toByte();
                            //byteOffsets[i] = dataByte.Length;
                            //offset += dataByte.Length;
                            //fs.Write(dataByte, offset, dataByte.Length);
                        }
                    }
                }
                else
                {

                }
            }
        }


        private int ClacArraySize()
        {
            int returnValue = 0;
            for (int i = 0; i < numberOfDatablocks; i++)
                returnValue += numberOfDatablocks - i;
            return returnValue;
        }
        public void AddDataBlockHost()
        {
            DatablockHOST[] newData = new DatablockHOST[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
                newData[i] = data[i];
            newData[newData.Length - 1] = new DatablockHOST("");
        }
        public class DatablockHOST
        {
            private class Datablock
            {
                ushort sizeOf;
                string otherPlayer;
                ushort win;
                ushort loss;

                public ushort SizeOf
                {
                    get
                    {
                        return sizeOf;
                    }
                }
                public string NameOf
                {
                    get
                    {
                        return otherPlayer;
                    }
                }
                
                public Datablock(string name, ushort startingWINS, ushort startingLOSS)
                {
                    otherPlayer = name;
                    win = startingWINS;
                    loss = startingLOSS;
                    sizeOf = (ushort)toByte().Length;
                }
                public Datablock()
                {
                    otherPlayer = "*BLANK_NODATA*";
                    win = 0;
                    loss = 0;
                    sizeOf = 0;
                }

                public byte[] toByte()
                {
                    string byteString;
                    string[] temp = new string[3];
                    temp[0] = otherPlayer;
                    temp[1] = Convert.ToString((int)win);
                    temp[2] = Convert.ToString((int)loss);

                    byteString = temp[0] + temp[1] + temp[2];
                    return System.Text.Encoding.ASCII.GetBytes(byteString);
                }
            }
            private string hostPlayer;
            private Datablock[] otherPlayers;

            public DatablockHOST(string name)
            {
                hostPlayer = name;
                otherPlayers = new Datablock[1] { new Datablock() };
            }
            public DatablockHOST(string name, ushort playerAmt, byte[] blocks)
            {
                hostPlayer = name;
            }

            public string NameOf
            {
                get
                {
                    return hostPlayer;
                }
            }
            

            private Datablock[] StrToDatablockNODATA(params string[] strArray)
            {
                if (strArray != null)
                {
                    int len = strArray.Length;
                    Datablock[] returnValue = new Datablock[len];

                    for (int i = 0; i < len; i++)
                    {
                        returnValue[i] = new Datablock(strArray[i], 0, 0);
                    }
                    return returnValue;
                }
                else
                    return null;
            }
            public byte[] toByte
            {
                get
                {
                    byte[] returnValue;
                    ushort offset = 0;

                    for (int i = 0; i < otherPlayers.Length; i++)
                        offset += otherPlayers[i].SizeOf;
                    returnValue = new byte[offset];
                    offset = 0;
                    for (int i = 0; i < otherPlayers.Length; i++)
                    {
                        byte[] temp = otherPlayers[i].toByte();
                        for (ushort j = offset; j < offset + otherPlayers[i].SizeOf; j++)
                        {
                            returnValue[j] = temp[j - offset];
                        }
                        offset += (ushort)temp.Length;
                    }
                    return returnValue;
                }
            }
        }
    }
}