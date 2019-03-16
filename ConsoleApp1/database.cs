using System;
using System.IO;
using System.Security.AccessControl;
using System.Runtime.Serialization.Formatters.Binary;

namespace TObase
{
    public class Database
    {
        //Sort datahosts alphabetically, List of starting letters within data
        Iterator it;
        FileStream stream;
        const int dataBufferExtension = 10;
        
        public string[] player;
        public DatablockHOST[] data;

        const string baseFolderName = "TO";
        const string subFolderName = "data";
        const string dataFile = "data.txt";
        public string path;

        public Database(int dataBuffer)
        {
            if(dataBuffer > 0)
            {
                it = new Iterator(dataBuffer);
                player = new string[dataBuffer];
                for (int i = 0; i < dataBuffer; i++)
                player[i] = "OPEN_SLOT";
                data = new DatablockHOST[dataBuffer];
            }
            path = Path.Combine("C:/Users/Aaron/Source/Repos/aml2296/Tournament_Seeder",baseFolderName);
            if(CheckForData())
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

        public bool CheckForData()
        {
            string filePath = Path.Combine(path, subFolderName);
            if (!File.Exists(path))
            {
                DirectorySecurity securityRules = new DirectorySecurity();
                securityRules.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));

                DirectoryInfo di = Directory.CreateDirectory(filePath, securityRules);
               
                filePath = Path.Combine(filePath,dataFile);
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No Data.txt Found, Generating Blank");
                    Console.WriteLine();
                    File.Create(filePath);   
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
        

        public bool writeToFolder(byte[] byteArray, string folderPath, int offset)
        {
            try
            {
                string filePath = Path.Combine(this.path,subFolderName,dataFile);
                using(var f = new FileStream(filePath,FileMode.Open,FileAccess.Write))
                {
                    foreach(byte b in byteArray)
                        f.WriteByte(b);
                    f.Close();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Write to folder caught an Excetion: {0}", ex);
                return false;
            }
        }
        public void ReadFiles()
        {               
             byte[] byteArray;
             uint offset = 0;
             string filePath = Path.Combine(this.path,subFolderName,dataFile);
             using(var r = new FileStream(filePath,FileMode.Open,FileAccess.Read))
             {
                  byteArray = File.ReadAllBytes(filePath);
             }

             uint sizeOfData = (uint)BitConverter.ToUInt32(byteArray,(int)offset);
             offset += sizeof(uint);

             short count = 0;
             do
             {
                 uint sizeOf = (uint)BitConverter.ToUInt32(byteArray,(int)offset);
                 offset += sizeOf;
                 count++;
              }while(offset < sizeOfData - 1);
             
            data = new DatablockHOST[count];
            offset = sizeof(uint);
            for(int i = 0;i < count;i++)
            {
                data[i] = new DatablockHOST(byteArray, offset);
                offset += data[i].SizeOf;
            }
        }
        public void AddDataBlockHost(string name)
        {
            if(data != null)
            {
                DatablockHOST[] newData = data;
                data = new DatablockHOST[newData.Length + 1];
                for (int i = 0; i < data.Length; i++)
                {
                    if(i < newData.Length)
                    {
                         data[i] = newData[i];
                    }
                    else
                    {
                        data[i] = new DatablockHOST(name);
                    }
                }
            }
            else
            {
                data = new DatablockHOST[1];
                data[0] = new DatablockHOST(name);
            }
        }
        public void AddDataBlockHost(string name, params Datablock[] datablocks)
        {
            if(data != null)
            {
                it.First();
                if(data[0] != null)
                {
                    do
                    {
                        Next();
                    }while(data[it.it] != null || it.it != it.minValue);
                    if(it.it == it.minValue)
                    {
                        it.maxValue += 1;
                        it.Last();
                        DatablockHOST[] temp = data;
                        data = new DatablockHOST[temp.Length + 10];
                    }
                }
                data[it.it] = new DatablockHOST(name,datablocks);
            }
        }
        public void AddDataBlockHosts(string name, params DatablockHOST[] hosts)
        {
            if(data != null)
            {
                int count = 0;
                it.it = it.minValue;
                do
                {
                    if(data[it.it] == null)
                        data[it.it] = new DatablockHOST(hosts[count++]);
                    Next();
                }while(it.it != it.minValue);

                if(count < hosts.Length - 1)
                {
                    int hostsLeftToAdd = hosts.Length - count;
                    DatablockHOST[] temp = data;
                    data = new DatablockHOST[temp.Length + hosts.Length - (count + 1)];
                    for(int i =0; i < temp.Length + hostsLeftToAdd; i++)
                    {
                        if(i < temp.Length)
                            data[i] = temp[i];
                        else
                            data[i] = new DatablockHOST(hosts[count++]);
                    }
                }
            }
            else
            {
                data = new DatablockHOST[hosts.Length + dataBufferExtension];
                for(int i = 0; i < data.Length; i++)
                {
                    if(i < hosts.Length)
                        data[i] = new DatablockHOST(hosts[i]);
                    else
                        data[i] = null;
                }
            }
        }
        public byte[] ConvertDataToByte()
        {
            uint sizeOfData = 0;
            uint offset = 0;
            foreach(DatablockHOST host in data)
            {
                if(host != null)
                    sizeOfData += host.SizeOf;
            }
            byte[] returnValue = new byte[sizeOfData + sizeof(uint)];

            byte[] sizeBytes = BitConverter.GetBytes(sizeOfData);
            foreach(byte b in sizeBytes)
            {
                returnValue[offset++] = b;
            }

            foreach(DatablockHOST host in data)
            {
                if(host != null)
                {
                    byte[] byteArray = host.ConvertHostToByte();
                    for(int i = 0; i < host.SizeOf; i++)
                        returnValue[offset + i] = byteArray[i];
                    offset += host.SizeOf;
                }
            }
            return returnValue;
        }
    }
    public class DatablockHOST: Datablock  
        {
            private ushort datablockAMT;
            protected ushort tier = new ushort();
            public Datablock[] otherPlayersDatablocks;

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
            public DatablockHOST(DatablockHOST host)
            {
                nameOfPlayer = host.nameOfPlayer;
                sizeOf = host.SizeOf;
                if(nameOfPlayer.Length > SIZE_OF_NAME)
                    increasedStringValue = (ushort)((nameOfPlayer.Length / (int)SIZE_OF_NAME) + 1);
                else
                    increasedStringValue = 1;

                otherPlayersDatablocks = host.otherPlayersDatablocks;
                datablockAMT = (ushort)otherPlayersDatablocks.Length;
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
            public DatablockHOST(byte[] byteArray, uint offset)
            {
                sizeOf = (uint)BitConverter.ToUInt32(byteArray,(int)offset);
                offset += sizeof(UInt32);

                increasedStringValue = (ushort)BitConverter.ToInt16(byteArray,(int)offset);
                offset += sizeof(ushort);
            
                datablockAMT = (ushort)BitConverter.ToUInt16(byteArray,(int)offset);
                offset += sizeof(ushort);

                char[] nameCharArray = new char[increasedStringValue*SIZE_OF_NAME];
                for(int i = 0; i < increasedStringValue*SIZE_OF_NAME;i++)
                {
                    nameCharArray[i] = BitConverter.ToChar(byteArray,(int)offset);
                    offset += sizeof(char);
                }
                nameOfPlayer = new string(nameCharArray);


                
                otherPlayersDatablocks = new Datablock[datablockAMT];
                for(int i =0; i < datablockAMT; i++)
                {
                    otherPlayersDatablocks[i] = new Datablock(byteArray, (int)offset);
                    offset += otherPlayersDatablocks[i].SizeOf;
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