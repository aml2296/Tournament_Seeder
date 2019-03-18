using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AML
{
    public class AlphaSort //Sorts Comparable Data
    {
        public static T[] Sort<T>(ref T[] sortValues) where T : IComparable
        {
            try
            {
                T holder = sortValues[0];
                int position = 0;
                int length = sortValues.Length;


                for (int index = 0; index < length - 1; index++)
                {
                    if (sortValues[index + 1].CompareTo(holder) < 0)
                    {
                        holder = sortValues[index + 1];
                        RestackBackToFront(ref sortValues, holder);
                    }
                    else
                    {
                        if (sortValues[index].CompareTo(sortValues[index + 1]) > 0)
                        {
                            position = index;
                            do
                            {
                                Swap(ref sortValues[index], ref sortValues[index + 1]);
                                index--;
                            } while (sortValues[index].CompareTo(sortValues[index + 1]) > 0);
                            index = position;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return sortValues;
        }
        public static T[] RestackBackToFront<T>(ref T[] restackValues, T moveToFront)
        {
            try
            {
                T tempOne;
                int end = Array.IndexOf(restackValues, moveToFront);

                for (int index = 0; index < end; index++)
                {
                    tempOne = restackValues[end];
                    restackValues[end] = restackValues[index];
                    restackValues[index] = tempOne;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return restackValues;
        }
        public static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }
    }
    public class CharList //Tracks a Tier of all lowercase Char's (a-z)
    {
        List<List<int>> CharL;

        public CharList()
        {
            for (int i = 0; i < 26; i++)
                CharL.Add(new List<int>());
        }

        private List<int> this[int i]
        {
            get
            {
                if (i - 'a' >= 0 && i - 'a' <= 'z' - 'a')
                    return CharL[i - 'a'];
                else if (i - 'a' < 0)
                    return CharL[0];
                else
                    return CharL[26];
            }
        }
        public void Add(char c, int tier)
        {
            this[(int)Char.ToLowerInvariant(c)].Add(tier);
        }
        public List<int> FindIndexList(char index)
        {
            return this[(int)index];
        }
        public byte[] ToByte()
        {
            //Returns a Byte Array made up of the elements contained in class
            //  |SizeOf->|--|Char|Tier|--|Char|Tier|--|Char|Tier|--...ect
            //  Each element is written with two int components
            //  The first being its char data and the second being its Tier
            const int TUPLE_ELEMENT_SIZE = 10;
            var elementsInList = new Tuple<int, int>[TUPLE_ELEMENT_SIZE];
            int[] arrayCount = new int[3] { 0, 0, 0 };

            foreach (List<int> charInList in CharL) //Converts the List of Lists into a Tuple Array
            {
                if (CharL[arrayCount[0]].Count > 0)
                {
                    foreach (int tier in charInList)
                    {
                        if (arrayCount[2] != 0 && arrayCount[2] % (TUPLE_ELEMENT_SIZE - 1) == 0 )
                        {
                            //Extends the array of elements if on the last index of array
                            var temp = new Tuple<int, int>[elementsInList.Length + TUPLE_ELEMENT_SIZE];
                            for(int i = 0; i < elementsInList.Length; i++)
                            {
                                temp[i] = elementsInList[i];
                            }
                            elementsInList = temp;
                        }
                        elementsInList[arrayCount[2]] = new Tuple<int, int>(arrayCount[0], tier);
                        arrayCount[1]++;
                        arrayCount[2]++;
                    }
                }
                arrayCount[0]++;
            }


            byte[] returnValue = new byte[elementsInList.Length * (sizeof(int)) * 2];
            arrayCount[2] = 0;


            foreach (Tuple<int,int> T in elementsInList)
            {
                byte[] byteConv = BitConverter.GetBytes(T.Item1);
                foreach(byte b in byteConv)
                    returnValue[arrayCount[2]++] = b;
                byteConv = BitConverter.GetBytes(T.Item2);
                foreach (byte b in byteConv)
                    returnValue[arrayCount[2]++] = b;
            }

            return returnValue;
        }
    }
}
