using System;

namespace Lab1
{
    class Program
    {
        private static bool end = false;
        private static int choice;
        static void Main(string[] args)
        {
            while (true)
            {
                printOperations();
                getChoice();
                if (end) return;
                calculate();
            }
        }
        static void printOperations()
        {
            //Console.WriteLine(BigInt.jacobiSymbol(13, 45));
            Console.WriteLine("Choose the operation: ");
            Console.WriteLine("1 - +");
            Console.WriteLine("2 - -");
            Console.WriteLine("3 - *");
            Console.WriteLine("4 - /");
            Console.WriteLine("5 - ^");
            Console.WriteLine("6 - [sqrt(x)]");
            Console.WriteLine("7 - <");
            Console.WriteLine("8 - >");
            Console.WriteLine("9 - ==");
            Console.WriteLine("10 - + (mod)");
            Console.WriteLine("11 - - (mod)");
            Console.WriteLine("12 - * (mod)");
            Console.WriteLine("13 - / (mod)");
            Console.WriteLine("14 - ^ (mod)");
            Console.WriteLine("15 - aiX=bi(mod pi)");
            Console.WriteLine("16 - ^ (mod) fast");
            Console.WriteLine("17 - Miller-Rabin Test");
            Console.WriteLine("18 - Billie Test");
            Console.WriteLine("19 - RSA");
            Console.WriteLine("20 - exit");
        }
        static void getChoice()
        {
            while (true)
            {
                Console.WriteLine("\nEnter operation:");
                choice = Convert.ToInt32(Console.ReadLine());
                if (choice < 1 || choice > 20)
                {
                    Console.WriteLine("Incorrect input. Try again.");
                    continue;
                }
                if (choice == 20)
                {
                    end = true;
                }
                break;
            }
        }
        static void calculate()
        {
            BigInt a, b, m;
            int n;
            switch (choice)
            {
                case 1:
                    Console.WriteLine("1 - +");
                    a = getNumber("First ");
                    // BigInt.Billie(a);
                    // Console.WriteLine((BigInt.Billie(a).toString()));
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + "+" + b.toString() + "=" + BigInt.add(a, b).toString());
                    break;
                case 2:
                    Console.WriteLine("2 - -");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + "-" + b.toString() + "=" + BigInt.subtract(a, b).toString());
                    break;
                case 3:
                    Console.WriteLine("3 - *");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + "*" + b.toString() + "=" + BigInt.multiply(a, b).toString());
                    break;
                case 4:
                    Console.WriteLine("4 - /");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + "/" + b.toString() + "=" + BigInt.divide(a, b).toString());
                    Console.WriteLine("Ostacha: " + BigInt.subtract(a, BigInt.multiply(b, BigInt.divide(a, b))).toString());
                    break;
                case 5:
                    Console.WriteLine("5 - ^");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + "^" + b.toString() + "=" + BigInt.pow(a, b).toString());
                    break;
                case 6:
                    Console.WriteLine("6 - [sqrt(x)]");
                    a = getNumber("Number >0 ");
                    Console.WriteLine("sqrt =" + a.sqrt().toString());
                    break;
                case 7:
                    Console.WriteLine("7 - <");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + "<" + b.toString() + "=" + BigInt.less(a, b));
                    break;
                case 8:
                    Console.WriteLine("8 - >");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + ">" + b.toString() + "=" + BigInt.greater(a, b));
                    break;
                case 9:
                    Console.WriteLine("9 - ==");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    Console.WriteLine(a.toString() + "==" + b.toString() + "=" + BigInt.equal(a, b));
                    break;
                case 10:
                    Console.WriteLine("10 - + (mod)");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    m = getNumber("mod ");
                    Console.WriteLine(a.toString() + "+" + b.toString() + "mod" + m.toString()+ "=" + BigInt.addMod(a, b, m).toString());
                    break;
                case 11:
                    Console.WriteLine("11 - - (mod)");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    m = getNumber("mod ");
                    Console.WriteLine(a.toString() + "-" + b.toString() + "mod" + m.toString() + "=" + BigInt.subtractMod(a, b, m).toString());
                    break;
                case 12:
                    Console.WriteLine("12 - * (mod)");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    m = getNumber("mod ");
                    Console.WriteLine(a.toString() + "*" + b.toString() + "mod" + m.toString() + "=" + BigInt.multiplyByMod(a, b, m).toString());
                    break;
                case 13:
                    Console.WriteLine("13 - /(mod)");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    m = getNumber("mod ");
                    Console.WriteLine(a.toString() + "/" + b.toString() + "mod" + m.toString() + "=" + BigInt.divideMod(a, b, m).toString());
                    Console.WriteLine("Ostacha: " + BigInt.subtractMod(a, BigInt.multiply(b, BigInt.divide(a, b)), m).toString());
                    break;
                case 14:
                    Console.WriteLine("14 - ^ (mod)");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    m = getNumber("mod ");
                    Console.WriteLine(a.toString() + "^" + b.toString() + "mod" + m.toString() + "=" + BigInt.powByMod(a, b, m).toString());
                    break;
                case 15:
                    Console.WriteLine("15 - aiX=bi(mod pi)");
                    n = getUInt("Count of equations");
                    BigInt[] A = new BigInt[n];
                    BigInt[] B = new BigInt[n];
                    BigInt[] M = new BigInt[n];
                    for (int i = 0; i < n; i++)
                    {
                        A[i] = getNumber("A[" + i + "]");
                        B[i] = getNumber("B[" + i + "]");
                        M[i] = getNumber("P[" + i + "]");
                    }
                    for (int i = 0; i < n; i++)
                        Console.WriteLine(A[i].toString() + "x =" + B[i].toString() + "mod" + M[i].toString());
                    (BigInt, BigInt) res = BigInt.solve(A, B, M, n);
                    Console.WriteLine("x=" + res.Item1.toString() + "mod" + res.Item2.toString());
                    break;
                case 16:
                    Console.WriteLine("16 - ^ (mod) fast");
                    a = getNumber("First ");
                    b = getNumber("Second ");
                    m = getNumber("mod ");
                    Console.WriteLine(a.toString() + "^" + b.toString() + "mod" + m.toString() + "=" + BigInt.powByModFast(a, b, m).toString());
                    break;
                case 17:
                    Console.WriteLine("17 - Miller Rabin");
                    a = getNumber("Number ");
                    Console.WriteLine(BigInt.MillerRabin(a));
                    break;
                case 18:
                    //Console.WriteLine("18 - Billie");
                    //Console.WriteLine("Enter the word");
                    //string word = Console.ReadLine();
                    //RSA test = new RSA(8, 1, 6);
                    //test.Encryption(word);
                    //Console.WriteLine(test.Decryption());
                    break;
                case 19:
                    Console.WriteLine("Enter the word");
                    string word = Console.ReadLine();
                    long ellapledTicks = DateTime.Now.Ticks;
                    RSA test = new RSA(8, 32, 64);
                    test.Encryption(word);
                    ellapledTicks = DateTime.Now.Ticks - ellapledTicks;

                    TimeSpan elapsedSpan = new TimeSpan(ellapledTicks);
                    
                    Console.WriteLine(test.Decryption());
                    Console.WriteLine("Time: " + elapsedSpan.TotalMilliseconds);
                    break;
                case 20:
                    Console.WriteLine("19 - exit");
                    end = true;
                    break;
            }
        }
        static BigInt getNumber(String name)
        {
            Console.WriteLine(name + " operand:");
            String s = Console.ReadLine();
            return new BigInt(s);
        }
        static int getUInt(String name)
        {
            Console.WriteLine(name + " operand:");
            String s = Console.ReadLine();
            return Convert.ToInt32(s);
        }
    }
}
