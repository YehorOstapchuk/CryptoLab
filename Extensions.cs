using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1
{
    public static class Extensions
    {
        public static List<string> base64Table = new List<string>() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                                                     "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                                                     "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "+",  "/" };
        public static string ToBinary(this BigInt x)
        {
            BigInt num = new BigInt(x.toString());
            BigInt zero = new BigInt("0");
            BigInt two = new BigInt("2");
            string res = "";
            BigInt temp;
            while (num.toString() != "0")
            {
                temp = BigInt.mod(num, two);
                num = BigInt.subtract(num, temp);
                num = BigInt.divide(num, two);
                res += temp.toString();
            }
            return new string(res.ToCharArray().Reverse().ToArray());
        }

        public static string ToBiase64(this BigInt x)
        {
            BigInt num = new BigInt(x.toString());
            BigInt zero = new BigInt("0");
            BigInt two = new BigInt("64");
            string res = "";
            BigInt temp;
            while (num.toString() != "0")
            {
                temp = BigInt.mod(num, two);
                num = BigInt.subtract(num, temp);
                num = BigInt.divide(num, two);
                res += base64Table[Int32.Parse(temp.toString())];
            }
            return new string(res.ToCharArray().Reverse().ToArray());
        }

        public static string ToEight(this BigInt x)
        {
            BigInt num = new BigInt(x.toString());
            BigInt zero = new BigInt("0");
            BigInt two = new BigInt("8");
            string res = "";
            BigInt temp;
            while (num.toString() != "0")
            {
                temp = BigInt.mod(num, two);
                num = BigInt.subtract(num, temp);
                num = BigInt.divide(num, two);
                res += temp.toString();
            }
            return new string(res.ToCharArray().Reverse().ToArray());
        }
    }
}
