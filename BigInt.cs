using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1
{
    public class BigInt
    {
        private bool Negative;
        private int[] number;
        private static int BASE = 10000;
        private static int LEN = 4;

        public BigInt(String val)
        {
            val = signumStringToUnsignedString(val);
            if (val.Length == 0)
                throw new ArgumentException("Zero length BigNumber");
            val = skipLeadingZeros(val);
            if (val.Length == 0)
            {
                number = new int[] { 0 };
                Negative = false;
                return;
            }
            number = new int[(val.Length - 1) / LEN + 1];
            for (int i = val.Length, j = 0; i > 0; i -= LEN, j++)
            {
                if (i >= LEN)
                {
                    number[j] = Convert.ToInt32(val.Substring(i - LEN, LEN));
                }
                else
                {
                    number[j] = Convert.ToInt32(val.Substring(0, i));
                }
            }
            return;
        }

        private BigInt(int[] number, bool isNegative)
        {
            this.number = number;
            this.Negative = isNegative;
            if (this.isZero())
                this.Negative = false;
        }

        public static BigInt subtractMod(BigInt a, BigInt b, BigInt p)
        {
            return mod(subtract(a, b), p);
        }

        public static BigInt addMod(BigInt a, BigInt b, BigInt p)
        {
            return mod(add(mod(a, p), mod(b, p)), p);
        }

        public static BigInt divideMod(BigInt a, BigInt b, BigInt p)
        {
            BigInt[] A = new BigInt[] { b };
            BigInt[] B = new BigInt[] { a };
            BigInt[] M = new BigInt[] { p };
            return solve(A, B, M, 1).Item1;
        }

        private String signumStringToUnsignedString(String val)
        {
            if (val.Length == 0)
                throw new ArgumentException("Zero length BigInteger");
            if (val[0] == '+')
            {
                Negative = false;
                return val.Substring(1);
            }
            else if (val[0] == '-')
            {
                Negative = true;
                return val.Substring(1);
            }
            else
            {
                Negative = false;
                return val;
            }
        }

        private String skipLeadingZeros(String val)
        {
            int cursor = 0;
            while (cursor < val.Length &&
                    Convert.ToInt32(val[cursor]) == 0)
            {
                cursor++;
            }
            return val.Substring(cursor);
        }

        private int intAtDigits(int i)
        {
            if (i < number.Length)
            {
                return number[i];
            }
            else
            {
                return 0;
            }
        }

        private static int[] addRes(BigInt a, BigInt b)
        {
            int[] sumOfaAndb = new int[Math.Max(a.number.Length, b.number.Length) + 1];
            int carry = 0;
            for (int i = 0; i < Math.Max(a.number.Length, b.number.Length) + 1; ++i)
            {
                sumOfaAndb[i] += carry + a.intAtDigits(i) + b.intAtDigits(i);
                carry = sumOfaAndb[i] >= BASE ? 1 : 0;
                if (carry == 1)
                {
                    sumOfaAndb[i] -= BASE;
                }
            }
            return deleteLeadingZerosOfArray(sumOfaAndb);
        }

        private static int[] subtractRes(BigInt firstNumberBiggerAbsThanSecond, BigInt second)
        {
            int[] minusOfaAndb = new int[Math.Max(firstNumberBiggerAbsThanSecond.number.Length, second.number.Length)];
            int carry = 0;
            for (int i = 0; i < Math.Max(firstNumberBiggerAbsThanSecond.number.Length, second.number.Length); ++i)
            {
                minusOfaAndb[i] += -carry + firstNumberBiggerAbsThanSecond.intAtDigits(i) - second.intAtDigits(i);
                carry = minusOfaAndb[i] < 0 ? 1 : 0;
                if (carry == 1)
                {
                    minusOfaAndb[i] += BASE;
                }
            }
            return deleteLeadingZerosOfArray(minusOfaAndb);
        }

        private static int[] deleteLeadingZerosOfArray(int[] array)
        {
            int indexOfSkipZeros = array.Length - 1;
            while (indexOfSkipZeros > 0)
            {
                if (array[indexOfSkipZeros] == 0)
                {
                    indexOfSkipZeros--;
                }
                else
                {
                    break;
                }
            }
            int[] newArray = new int[indexOfSkipZeros + 1];
            Array.Copy(array, 0, newArray, 0, indexOfSkipZeros + 1);
            return newArray;

        }

        private static int[] mult(BigInt a, BigInt b)
        {
            int[] c = new int[a.number.Length + b.number.Length];
            for (int i = 0; i < a.number.Length; ++i)
                for (int j = 0, carry = 0; (j < b.number.Length || carry != 0); ++j)
                {
                    long cur = c[i + j] + a.number[i] * (j < (int)b.number.Length ? b.number[j] : 0) + carry;
                    c[i + j] = (int)(cur % BASE);
                    carry = (int)(cur / BASE);
                }
            return deleteLeadingZerosOfArray(c);
        }

        private void LevelUp()
        {
            int[] a = new int[number.Length + 1];
            Array.Copy(number, 0, a, 0, number.Length);
            for (int i = number.Length; i >= 1; i--)
                a[i] = number[i - 1];
            if (a[number.Length] == 0) Array.Copy(a, 0, number, 0, number.Length);
            else number = a;
        }


        public static BigInt divide(BigInt a, BigInt b)
        {
            if (b.isZero())
                throw new ArgumentException("divide to zero");
            int[] res = new int[a.number.Length];
            BigInt curValue = new BigInt("0");
            for (int i = a.number.Length - 1; i >= 0; i--)
            {
                curValue.LevelUp();
                curValue.number[0] = a.number[i];
                int x = 0;
                int le = 0, r = BASE;
                while (le <= r)
                {
                    int m = (le + r) >> 1;
                    BigInt cur = multiply(b, valueOf(m));
                    if (lessEqualAbs(cur, curValue))
                    {
                        x = m;
                        le = m + 1;
                    }
                    else
                        r = m - 1;
                }
                res[i] = x;
                curValue = subtract(curValue, multiply(b, valueOf(x)));
            }
            if (less(a, BigInt.ZERO) && !equal(curValue, ZERO))
            {
                return subtract(new BigInt(deleteLeadingZerosOfArray(res), a.Negative == b.Negative ? false : true), valueOf(1));
            }
            return new BigInt(deleteLeadingZerosOfArray(res), a.Negative == b.Negative ? false : true);
        }

        private static BigInt ZERO = new BigInt("0");

        public static BigInt mod(BigInt a, BigInt b)
        {
            if (!greater(b, new BigInt("0")))
                throw new ArgumentException("b must be >0");
            int[] res = new int[a.number.Length];
            BigInt curValue = new BigInt("0");
            for (int i = a.number.Length - 1; i >= 0; i--)
            {
                curValue.LevelUp();
                curValue.number[0] = a.number[i];
                // подбираем максимальное число x, такое что b * x <= curValue
                int x = 0;
                int le = 0, r = BASE;
                while (le <= r)
                {
                    int m = (le + r) >> 1;
                    BigInt cur = multiply(b, valueOf(m));
                    if (lessEqualAbs(cur, curValue))
                    {
                        x = m;
                        le = m + 1;
                    }
                    else
                        r = m - 1;
                }
                res[i] = x;

                curValue = subtract(curValue, multiply(b, valueOf(x)));
            }
            if (less(a, BigInt.ZERO) && !equal(curValue, ZERO))
            {
                return subtract(b, curValue);
            }
            else
                return curValue;
        }

        private static bool equalAbs(BigInt a, BigInt b)
        {
            if (a.number.Length != b.number.Length)
                return false;
            for (int i = 0; i < a.number.Length; i++)
            {
                if (a.number[i] != b.number[i])
                    return false;
            }
            return true;
        }

        public static bool equal(BigInt a, BigInt b)
        {
            return equalAbs(a, b) && a.Negative == b.Negative;
        }

        private static bool lessEqualAbs(BigInt a, BigInt b)
        {
            if (a.number.Length != b.number.Length)
                return a.number.Length < b.number.Length;
            for (int i = a.number.Length - 1; i >= 0; i--)
            {
                if (a.number[i] != b.number[i])
                    return a.number[i] < b.number[i];
            }
            return true;
        }

        private static bool lessAbs(BigInt a, BigInt b)
        {
            if (a.number.Length != b.number.Length)
                return a.number.Length < b.number.Length;
            for (int i = a.number.Length - 1; i >= 0; i--)
            {
                if (a.number[i] != b.number[i])
                    return a.number[i] < b.number[i];
            }
            return false;
        }

        public static bool less(BigInt a, BigInt b)
        {
            if (a.Negative == b.Negative)
            {
                if (a.Negative)
                    return !lessEqualAbs(a, b);
                else
                    return lessAbs(a, b);
            }
            else if (a.Negative)
                return true;
            else
                return false;
        }

        public static bool greater(BigInt a, BigInt b)
        {
            return !less(a, b) && !equal(a, b);
        }

        private BigInt(int len, bool isNeg)
        {
            this.number = new int[len];
            this.Negative = isNeg;
        }

        public BigInt sqrt()
        {
            if (less(this, ZERO))
                throw new ArgumentException("BigNumber must be >=0");
            int pos = (number.Length + 1) / 2;
            BigInt cur = new BigInt(pos, false);
            pos--;
            while (pos >= 0)
            {
                int l = 0, r = BASE;
                int curDigit = 0;
                while (l <= r) // подбираем текущую цифру
                {
                    int m = (l + r) >> 1;
                    cur.number[pos] = m;
                    if (BigInt.lessEqualAbs(BigInt.multiply(cur, cur), this))
                    {
                        curDigit = m;
                        l = m + 1;
                    }
                    else
                        r = m - 1;
                }
                cur.number[pos] = curDigit;
                pos--;
            }
            Array.Copy(cur.number, 0, this.number, 0, cur.number.Length);
            return this;
        }



        public static BigInt pow(BigInt a, BigInt n)
        {
            if (n.isZero())
                return BigInt.valueOf(1);

            BigInt c = mod(n, valueOf(2));
            if (equal(valueOf(1), c))
                return multiply(pow(a, subtract(n, valueOf(1))), a);
            else
            {
                BigInt b = pow(a, divide(n, valueOf(2)));
                return multiply(b, b);
            }
        }

        public static BigInt powByMod(BigInt x, BigInt y, BigInt N)
        {
            if (y.isZero())
                return new BigInt("1");
            BigInt z = BigInt.powByMod(x, divide(y, new BigInt("2")), N);
            if (mod(y, valueOf(2)).isZero())
                return mod(multiply(z, z), N);
            else
                return mod(multiply(multiply(z, z), x), N);
        }

        public static BigInt powByModFast(BigInt x, BigInt y, BigInt N)
        {
            if (y.isZero())
                return new BigInt("1");
            string binary = Convert.ToString(Int32.Parse(y.toString()), 2);
            binary = new string(binary.ToCharArray().Reverse().ToArray());
            List<BigInt> temp = new List<BigInt>();
            BigInt last = x;
            temp.Add(last);
            for (int i = 1; i < binary.Length; i++)
            {
                last = mod((multiply(last, last)), N);
                temp.Add(last);
            }
            BigInt res = new BigInt("1");
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1') res = multiply(res, temp[i]);
            }
            res = mod(res, N);
            return res;
        }

        public static string MillerRabin(BigInt x)
        {
            BigInt xMinusOne = subtract(x, new BigInt("1"));
            BigInt two = new BigInt("2");
            int i = 0;
            BigInt m = xMinusOne;
            while (true)
            {
                if (BigInt.subtract(m, BigInt.multiply(two, BigInt.divide(m, two))).toString() != "0") break;
                m = divide(m, two);
                i++;
            }
            Random r = new Random();
            BigInt a;
            if (xMinusOne.toString().Length >= 10) a = new BigInt((r.Next(2, 1000000000)).ToString());
            else a = new BigInt((r.Next(2, Int32.Parse(subtract(xMinusOne, new BigInt("1")).toString()))).ToString());
            BigInt b = powByModFast(a, m, x);
            BigInt one = new BigInt("1");
            BigInt counter = new BigInt("0");
            BigInt iBigInt = new BigInt(i.ToString());
            if (equal(b, one)) return "probably prime";
            while (less(counter, iBigInt))
            {
                if (equal(b, xMinusOne)) return "probably prime";
                b = powByModFast(b, two, x);
                counter = add(counter, one);
            }
            return "composite";
        }

        public static void Billie(BigInt n)
        {
            Console.WriteLine("Billie Primarly test:");
            if (MillerRabin(n) != "probably prime") { Console.WriteLine("Miller-Rabin Falled"); return; }
            Console.WriteLine(Vn(n, FindD(n)));
        }

        public static bool BillieBool(BigInt n)
        {
            if (MillerRabin(n) != "probably prime")  return false;
            //return Vn(n, FindD(n));
            return true;
        }

        public static bool Vn(BigInt n, BigInt D)
        {
            BigInt P = new BigInt("1");
            BigInt Q = divide(subtract(P, D), new BigInt("4"));
            Console.WriteLine("Q:" + Q.toString());
            BigInt U = new BigInt("1");
            BigInt V = new BigInt("1");
            BigInt k = new BigInt("1");
            BigInt one = new BigInt("1");
            BigInt two = new BigInt("2");
            BigInt nPlusOne = add(one, n);
            string bin = nPlusOne.ToBinary();
            Console.WriteLine("n+1 bin: " + bin);
            BigInt U_temp = new BigInt(U.toString());
            BigInt V_temp = new BigInt(V.toString());
            int i = 0;
            while (i < bin.Length)
            {
                //if ((bin[i] == '1') && (i == 0))
                //{
                //    U = U2k(U_temp, V_temp);
                //    V = V2k(V_temp, Q, k);
                //}
                if ((bin[i] == '1') && (i != 0))
                {
                    U = U2k(U_temp, V_temp);
                    V = V2k(V_temp, Q, k);
                    U_temp = new BigInt(U.toString());
                    V_temp = new BigInt(V.toString());
                    //Console.WriteLine("V:" + V.toString());
                    //Console.WriteLine("U" + U.toString());
                    k = multiply(two, k);
                    U = U2kPlusOne(U_temp, V_temp, P);
                    V = V2kPlusOne(U_temp, V_temp, P, D);
                    k = add(k, one);
                };
                if ((bin[i] == '0'))
                {
                    U = U2k(U_temp, V_temp);
                    V = V2k(V_temp, Q, k);
                    k = multiply(two, k);
                };
                //Console.WriteLine("V:" + V.toString());
                //Console.WriteLine("U" + U.toString());
                i++;
                U_temp = new BigInt(U.toString());
                V_temp = new BigInt(V.toString());
            }
            Console.WriteLine("k: " + k.toString());
            Console.WriteLine("Vn+1 mod n: " + mod(V, n).toString());
            BigInt compare = multiply(two, Q);
            while (less(compare, new BigInt("0")))
            {
                compare = add(compare, n);
            }
            Console.WriteLine("Result:");
            return equal(mod(V, n), compare);
        }

        public static int jacobiSymbol(int k, int n)
        {
            if (k < 0 || n % 2 == 0)
            {
                throw new ArgumentException("Invalid value. k = " + k + ", n = " + n);
            }
            k %= n;
            int jacobi = 1;
            while (k > 0)
            {
                while (k % 2 == 0)
                {
                    k /= 2;
                    int r = n % 8;
                    if (r == 3 || r == 5)
                    {
                        jacobi = -jacobi;
                    }
                }
                int temp = n;
                n = k;
                k = temp;
                if (k % 4 == 3 && n % 4 == 3)
                {
                    jacobi = -jacobi;
                }
                k %= n;
            }
            if (n == 1)
            {
                return jacobi;
            }
            return 0;
        }

        public static BigInt GCD(BigInt a, BigInt b)
        {
            if (b.isZero()) return a;
            else return GCD(b, mod(a, b));
        }

        public static BigInt LCM(BigInt a, BigInt b)
        {
            return divide(multiply(a , b), GCD(a, b));
        }

        public static BigInt PrimeGenerate(int min, int max)
        {
            Random rnd = new Random();
            while(true)
            {
                byte [] 
                int value = rnd.NextBytes()
                 Console.WriteLine("value: " + value);
                if ((value % 2 == 0) || (value <= 3)) continue;
                BigInt res = new BigInt(value.ToString());
                if (BillieBool(res)) return res;
            }
        }

        public static void ExtendedGCD(in BigInt a, in BigInt b, out BigInt x, out BigInt y)
        {
            BigInt one = new BigInt("1");
            BigInt zero = new BigInt("0");
            List<BigInt> xs = new List<BigInt>() { one, zero };
            List<BigInt> ys = new List<BigInt>() { zero, one };
            List<BigInt> qs = new List<BigInt>();
            BigInt aTemp = a;
            BigInt resedueTemp;
            BigInt resedue = b;
            while (true)
            {
                resedueTemp = mod(aTemp, resedue);
                if (resedueTemp.isZero()) break;
                qs.Add(divide(subtract(aTemp, resedueTemp), resedue));
                aTemp = resedue;
                resedue = resedueTemp;
            }
            BigInt xTemp;
            BigInt yTemp;
            for (int i = 2; i < qs.Count() + 2; i++)
            {
                xTemp = subtract(xs[i - 2], multiply(xs[i - 1], qs[i - 2]));
                xs.Add(xTemp);
                yTemp = subtract(ys[i - 2], multiply(ys[i - 1], qs[i - 2]));
                ys.Add(yTemp);
            }
            x = xs.LastOrDefault();
            y = ys.LastOrDefault();
        }

        public static BigInt FindD (BigInt n)
        {
            BigInt temp = new BigInt("5");
            BigInt two = new BigInt("2");
            BigInt minusOne = new BigInt("-1");
            while (true)
            {
                if (BigInt.greater(temp, two))
                {
                    if (jacobiSymbol(Int32.Parse(temp.toString()), Int32.Parse(n.toString())) == -1) return temp;
                    temp = add(temp, two);
                    temp = multiply(temp, minusOne);
                }
                else
                {
                    temp = multiply(temp, minusOne);
                    temp = add(temp, two);
                }
            }
        }

        public static BigInt U2k (BigInt Uk, BigInt Vk)
        {
            return multiply(Uk, Vk);
        }

        public static BigInt V2k(BigInt Vk, BigInt Q, BigInt k)
        {
            BigInt two = new BigInt("2");
            return subtract(multiply(Vk, Vk), multiply(two, pow(Q, k)));
        }

        public static BigInt U2kPlusOne(BigInt U2k, BigInt V2k, BigInt P)
        {
            BigInt two = new BigInt("2");
            return divide(add(multiply(P, U2k), V2k), two);
        }

        public static BigInt V2kPlusOne(BigInt U2k, BigInt V2k, BigInt P, BigInt D)
        {
            BigInt two = new BigInt("2");
            return divide(add(multiply(D, U2k), multiply(P,V2k)), two);
        }

        public static BigInt add(BigInt a, BigInt b)
        {
            if (a.Negative == b.Negative)
                return new BigInt(addRes(a, b), a.Negative);
            else if (a.Negative)
            {
                if (BigInt.lessEqualAbs(a, b))
                    return new BigInt(subtractRes(b, a), b.Negative);
                else
                    return new BigInt(subtractRes(a, b), a.Negative);
            }
            else
            {
                if (BigInt.lessEqualAbs(a, b))
                    return new BigInt(subtractRes(b, a), b.Negative);
                else
                    return new BigInt(subtractRes(a, b), a.Negative);
            }
        }

        public static BigInt subtract(BigInt a, BigInt b)
        {
            if (a.Negative == !b.Negative)
                return new BigInt(addRes(a, b), a.Negative);
            else if (a.Negative)
            {
                if (BigInt.lessEqualAbs(a, b))
                    return new BigInt(subtractRes(b, a), !b.Negative);
                else
                    return new BigInt(subtractRes(a, b), a.Negative);
            }
            else
            {
                if (BigInt.lessEqualAbs(a, b))
                    return new BigInt(subtractRes(b, a), !b.Negative);
                else
                    return new BigInt(subtractRes(a, b), a.Negative);
            }
        }

        public static BigInt multiply(BigInt a, BigInt b)
        {
            return new BigInt(mult(a, b), a.Negative == b.Negative ? false : true);
        }
        public static BigInt multiplyByMod(BigInt a, BigInt b, BigInt p)
        {
            return mod(multiply(mod(a, p), mod(b, p)), p);
        }



        public bool isZero()
        {
            return number.Length == 1 && number[0] == 0;
        }

        public String toString()
        {
            StringBuilder magSting = new StringBuilder();
            magSting.Append(number[number.Length - 1]);
            for (int i = number.Length - 2; i >= 0; i--)
            {
                for (int j = 0; j < LEN - (number[i]).ToString().Length; j++)
                {
                    magSting.Append("0");
                }
                magSting.Append(number[i]);
            }
            return (Negative ? "-" : "") + magSting.ToString();
        }

        public static BigInt valueOf(int n)
        {
            return new BigInt((n).ToString());
        }

        public BigInt(BigInt old)
        {
            this.number = new int[old.number.Length];
            this.Negative = old.Negative;
            for (int i = 0; i < old.number.Length; ++i)
            {
                this.number[i] = old.number[i];
            }
        }
        //алгоритм Гарнера
        public static (BigInt, BigInt) solve(BigInt[] A, BigInt[] B, BigInt[] M, int n)
        {
            BigInt x = new BigInt("0");
            BigInt mod = BigInt.valueOf(1);

            BigInt r1, r2, x1, x2, r, q, t, b;

            for (int i = 0; i < n; ++i)
            {

                r1 = BigInt.multiply(A[i], mod);
                r2 = M[i];
                x1 = BigInt.valueOf(1);
                x2 = BigInt.valueOf(0);
                r = BigInt.valueOf(1);

                while (!equal(r, ZERO))
                {
                    q = divide(r1, r2);
                    t = subtract(x1, multiply(q, x2));
                    x1 = new BigInt(x2);
                    x2 = new BigInt(t);
                    r = subtract(r1, multiply(q, r2));
                    r1 = new BigInt(r2);
                    r2 = new BigInt(r);
                }

                b = subtract(B[i], multiply(A[i], x));
                if (!equal(BigInt.mod(b, r1), ZERO))
                {
                    throw new ArgumentOutOfRangeException("No solution");
                }
                x = add(x, divide(multiply(mod, multiply(b, x1)), r1));
                mod = divide(multiply(mod, M[i]), r1);

            }

            if (less(x, ZERO) || greater(x, mod) || equal(x, mod))
            {
                x = subtract(x, multiply(mod, divide(x, mod)));
            }
            return  (x, mod);
        }

    public bool equals(Object o)
        {
            if (this == o) return true;
            if (o == null) return false;
            BigInt bigNumber = (BigInt)o;
            return Negative == bigNumber.Negative &&
                    Array.Equals(number, bigNumber.number);
        }

    }
}
