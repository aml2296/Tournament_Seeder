using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp1
{
    public class Database
    {
        Iterator it;
        
        public string[] player;
        public DatablockHOST[] data;

        string baseFolderName = @"c:\";
        string path;

        public Database(int size)
        {
            it = new Iterator(size);
            player = new string[size];
            for (int i = 0; i < size; i++)
                player[i] = "OPEN_SLOT";
            data = new DatablockHOST[size];
            path = Path.Combine(baseFolderName, "data");
            if(!CheckForFolder())
            {

            }
        }

        public int Position
        {
           get
            {
                return it.it;
            }
        }
        public bool Next()
        {
            if (it.it == it.maxValue)
                it.it = 0;
            else if (it.it < it.maxValue)
                it++;
            return true;
        }
        public bool Prev()
        {
            if (it.it == it.minValue)
                it.it = it.maxValue;
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
                return null;
            }
        }

        private bool CheckForFolder()
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
                if (!File.Exists(path))
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

        public void AddDataBlockHost()
        {
            DatablockHOST[] newData = new DatablockHOST[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
                newData[i] = data[i];
            newData[newData.Length - 1] = new DatablockHOST("");
        }


    }
        public class DatablockHOST : Datablock  
        {
            private ushort datablockAMT = new ushort();
            private Datablock[] otherPlayersDatablocks;

            public DatablockHOST(string name)
            {
                this.nameOfPlayer = name;
                this.datablockAMT = 0;
                this.otherPlayersDatablocks = new Datablock[1] { new Datablock() };

                sizeOf = 0;
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(char)*(UInt32)SIZE_OF_NAME*(UInt32)increasedStringValue;
                sizeOf += otherPlayersDatablocks[0].SizeOf;
            }
            public DatablockHOST(string name, params Datablock[] playersToAdd)
            {
                this.nameOfPlayer = name;
                this.datablockAMT = (ushort)playersToAdd.Length;
                this.sizeOf = 0;
                this.otherPlayersDatablocks = new Datablock[playersToAdd.Length];
                for(int i =0; i< datablockAMT; i++)
                {
                   this.otherPlayersDatablocks[i] = new Datablock(playersToAdd[i]);
                   this.sizeOf += playersToAdd[i].SizeOf;
                }
                if(nameOfPlayer.Length > SIZE_OF_NAME)
                    increasedStringValue = (ushort)((nameOfPlayer.Length / (int)SIZE_OF_NAME) + 1);
                else
                    increasedStringValue = 1;

                sizeOf += sizeof(UInt32);
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(char)*(UInt32)SIZE_OF_NAME*(UInt32)increasedStringValue;
            }
            public DatablockHOST(byte[] byteArray, int offset)
            {
                sizeOf = BitConverter.ToUInt32(byteArray,offset);
                offset += sizeof(UInt32);

                increasedStringValue = (ushort)BitConverter.ToInt16(byteArray,offset);
                offset += sizeof(ushort);
            
                datablockAMT = (ushort)BitConverter.ToUInt16(byteArray,offset);
                offset += sizeof(ushort);

                char[] nameCharArray = new char[increasedStringValue*SIZE_OF_NAME];
                for(int i = 0; i < increasedStringValue*SIZE_OF_NAME;i++)
                {
                    nameCharArray[i] = BitConverter.ToChar(byteArray,offset);
                    offset += sizeof(char);
                }
                nameOfPlayer = new string(nameCharArray);


                
                otherPlayersDatablocks = new Datablock[datablockAMT];
                for(int i =0; i < datablockAMT; i++)
                {
                    otherPlayersDatablocks[i] = new Datablock(byteArray, offset);
                    offset += (int)otherPlayersDatablocks[i].SizeOf;
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
            public byte[] ConvertHostToByte()
            {
                byte[] returnValue;
                UInt32 offset = 0;

                returnValue = new byte[this.SizeOf];
                byte[] temp = new byte[sizeof(UInt32)];
                

                temp = BitConverter.GetBytes(sizeOf);
                for(int i = 0; i < temp.Length; i++)
                {
                    returnValue[i + offset] = temp[i];     
                }
                offset += sizeof(UInt32);
            
            
                temp = BitConverter.GetBytes(increasedStringValue);    
                for(int i = 0; i < temp.Length; i++)
                {
                    returnValue[i + offset] = temp[i];     
                }
                offset += sizeof(ushort);


                temp = BitConverter.GetBytes(datablockAMT);    
                for(int i = 0; i < temp.Length; i++)
                {
                    returnValue[i + offset] = temp[i];     
                }
                offset += sizeof(ushort);


                if(nameOfPlayer.Length != SIZE_OF_NAME*increasedStringValue)
                    nameOfPlayer = new string(ConvertStrtoCharArray(nameOfPlayer));
                for(int i = 0; i < SIZE_OF_NAME*increasedStringValue; i++)
                {
                    temp = BitConverter.GetBytes(nameOfPlayer[i]);
                    for(int j = 0; j < sizeof(char); j++)
                        returnValue[j+offset+(i*sizeof(char))] = temp[j];
                }
                offset += sizeof(char) * (UInt32)SIZE_OF_NAME * (UInt32)increasedStringValue;


                for (int i = 0; i < otherPlayersDatablocks.Length; i++)
                {
                    temp = otherPlayersDatablocks[i].ConvertBlockToByte();
                    for (UInt32 j = offset; j < offset + otherPlayersDatablocks[i].SizeOf; j++)
                    {
                        returnValue[j] = temp[j - offset];
                    }
                    offset += otherPlayersDatablocks[i].SizeOf;
                }
                return returnValue;
            }
        }
        public class Datablock
        {
            protected UInt32 sizeOf = new UInt32();
            public string nameOfPlayer;
            protected ushort win = new ushort();
            protected ushort loss = new ushort();
            protected ushort increasedStringValue = new ushort();
            protected const short SIZE_OF_NAME = 10;

            public UInt32 SizeOf
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
                    return nameOfPlayer;
                }
            }
                
            public Datablock(string name, ushort startingWINS, ushort startingLOSS)
            {
                nameOfPlayer = name;
                win = startingWINS;
                loss = startingLOSS;
                if(nameOfPlayer.Length > SIZE_OF_NAME)
                    increasedStringValue = (ushort)((nameOfPlayer.Length / (int)SIZE_OF_NAME) + 1);
                else
                    increasedStringValue = 1;


                sizeOf += sizeof(ushort);
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(UInt32);
                sizeOf += sizeof(char)*(UInt32)SIZE_OF_NAME*(UInt32)increasedStringValue;
            }
            public Datablock(Datablock block)
            {
                nameOfPlayer = block.nameOfPlayer;
                win = block.win;
                loss = block.loss;
                sizeOf = block.SizeOf;
                if(nameOfPlayer.Length > SIZE_OF_NAME)
                    increasedStringValue = (ushort)((nameOfPlayer.Length / (int)SIZE_OF_NAME) + 1);
                else
                    increasedStringValue = 1;
            }
            public Datablock(byte[] byteArray, int offset)
            {
                sizeOf = (uint)BitConverter.ToUInt32(byteArray,offset);
                offset += sizeof(UInt32);

                increasedStringValue = (ushort)BitConverter.ToInt16(byteArray,offset);
                offset += sizeof(ushort);
                
                char[] nameCharArray = new char[increasedStringValue*SIZE_OF_NAME];
                for(int i = 0; i < increasedStringValue*SIZE_OF_NAME;i++)
                {
                    nameCharArray[i] = BitConverter.ToChar(byteArray,offset);
                    offset += sizeof(char);
                }
                nameOfPlayer = new string(nameCharArray);


                win = (ushort)BitConverter.ToUInt16(byteArray, offset);
                offset += sizeof(ushort);

                loss = (ushort)BitConverter.ToUInt16(byteArray, offset);
            }
            public Datablock()
            {
                nameOfPlayer = "*BLANK_NODATA*";
                win = 0;
                loss = 0;
                increasedStringValue = 2;
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(ushort);
                sizeOf += sizeof(UInt32);
                sizeOf += sizeof(char)*(UInt32)SIZE_OF_NAME*(UInt32)increasedStringValue;
            }

            public byte[] ConvertBlockToByte()
            {
                byte[] returnValue = new byte[sizeOf];
                UInt32 offset = 0;
                byte[] temp = new byte[sizeof(UInt32)];
               
 
                temp = BitConverter.GetBytes(sizeOf);
                for(int i = 0; i < temp.Length; i++)
                {
                    returnValue[i + offset] = temp[i];     
                }
                offset += sizeof(UInt32);
            
            
                temp = BitConverter.GetBytes(increasedStringValue);    
                for(int i = 0; i < temp.Length; i++)
                {
                    returnValue[i + offset] = temp[i];     
                }
                offset += sizeof(ushort);
            
            
                if(nameOfPlayer.Length != SIZE_OF_NAME*increasedStringValue)
                    nameOfPlayer = new string(ConvertStrtoCharArray(nameOfPlayer));
                for(int i = 0; i < SIZE_OF_NAME*increasedStringValue; i++)
                {
                    temp = BitConverter.GetBytes(nameOfPlayer[i]);
                    for(int j = 0; j < sizeof(char); j++)
                        returnValue[j+offset+(i*sizeof(char))] = temp[j];
                }
                offset += sizeof(char) * (UInt32)SIZE_OF_NAME * (UInt32)increasedStringValue;


                temp = BitConverter.GetBytes(win);
                for(int i = 0; i < temp.Length; i++)
                {
                    returnValue[i + offset] = temp[i];     
                }
                offset += sizeof(ushort);


                temp = BitConverter.GetBytes(loss);
                for(int i = 0; i < temp.Length; i++)
                {
                    returnValue[i + offset] = temp[i];     
                }
                offset += sizeof(ushort);
                return returnValue;
            }
            public char[] ConvertStrtoCharArray(string str)
            {
                char[] returnValue = new char[SIZE_OF_NAME*increasedStringValue];
                int count = 0;
                foreach(char character in str)
                    returnValue[count++] = character;
                for(int i = count; i < SIZE_OF_NAME*increasedStringValue;i++)
                    returnValue[i] = ' ';
                return returnValue;
            }
        }
}