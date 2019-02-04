using System;

public class database
{
    iterator it;
    int numberOfDatablocks = 0;
    public string[] player;
    public datablock[] data;
    private int[] byteOffsets;
    string baseFolderName = @"c:\";
    string path;

    public database(int size)
	{
        it = new iterator(size);
        byteOffsets = new int[size];
        player = new string[size];
        for (int i = 0; i < size; i++)
            player[i] = "OPEN_SLOT";
        numberOfDatablocks = size-1;
        data = new datablock[calculateArraySize()];
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


    private bool checkForFolder()
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
    public void gatherData()
    {
        if(checkForFolder())
        {
            string fileName = "source.txt";
            path = System.IO.Path.Combine(path, fileName);
            if(!System.IO.File.Exists(path))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(path))
                {
                    int offset = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        byte[] dataByte = data[i].toByte();
                        byteOffsets[i] = dataByte.Length;
                        offset += dataByte.Length;
                        fs.Write(dataByte, offset, dataByte.Length);
                    }
                }
            }
            else
            {

            }
        }
    }

    private int calculateArraySize()
    {
        int returnValue = 0;
        for (int i = 0; i < numberOfDatablocks; i++)
            returnValue += numberOfDatablocks - i;
        return returnValue;
    }

    public class datablock
    {
        string otherPlayer;
        ushort win;
        ushort loss;

        public datablock(string name, ushort startingWINS, ushort startingLOSS)
        {
            otherPlayer = name;
            win = startingWINS;
            loss = startingLOSS;
        }
        public datablock()
        {
            otherPlayer = "*BLANK_NODATA*";
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
}



public class iterator
{
    public int minValue,
        maxValue,
        it;
    public iterator(int min, int max)
    {
        minValue = new int();
        maxValue = new int();
        it = new int();

        minValue = it = min;
        maxValue = max;

        maxValue = max;
    }
    public iterator(int count)
    {
        minValue = new int();
        maxValue = new int();
        it = new int();

        minValue = 0;
        it = 0;
        maxValue = count - 1;
    }
    public static iterator operator++(iterator a)
    {
        a.it++;
        return a;
    }
    public static iterator operator --(iterator a)
    {
        a.it--;
        return a;
    }
}  //Returns an int for position between "minValue" and "maxValue" 
