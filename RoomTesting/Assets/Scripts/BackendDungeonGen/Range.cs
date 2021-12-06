using System;

namespace BackendDungeonGen
{
    public class Range
    {
        public static string[] stringRange(string[] array, int lowerBound, int upperBound)
        {
            if (upperBound == -1)
            {
                upperBound = array.Length;
            }

            string[] returnArray = new string[(upperBound - lowerBound)];

            for (int i = 0; i < array.Length; i++)
            {
                if (i == upperBound)
                {
                    break;
                }

                if (i >= lowerBound)
                {
                    returnArray[(i - lowerBound)] = array[i];
                }
            }

            return returnArray;
        }

        public static char[] charRange(char[] array, int lowerBound, int upperBound)
        {
            if (upperBound == -1)
            {
                upperBound = array.Length;
            }

            char[] returnArray = new char[(upperBound - lowerBound)];

            for (int i = 0; i < array.Length; i++)
            {
                if (i == upperBound)
                {
                    break;
                }

                if (i >= lowerBound)
                {
                    returnArray[(i - lowerBound)] = array[i];
                }
            }

            return returnArray;
        }
    }
}