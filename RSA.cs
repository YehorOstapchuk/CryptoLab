using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class RSA
    {
        BigInt p;
        BigInt q;
        BigInt mod;
        BigInt e;
        BigInt A;
        BigInt d;
        string word;
        List<BigInt> wordNumbersEncrypted;

        BigInt pSign;
        BigInt qSign;
        BigInt modSign;
        BigInt eSign;
        BigInt ASign;
        BigInt dSign;
        BigInt SignEncrypted;


        public RSA (int max, int powMin, int powMax)
        {
            SignInitialize(max,powMin, powMax);
            max = (int)Math.Pow(2, max);
            powMin = (int)Math.Pow(2, powMin);
            powMax = (int)Math.Pow(2, powMax);
            do
            {
                p = BigInt.PrimeGenerate(powMin, powMax);
                q = BigInt.PrimeGenerate(powMin, powMax);
                mod = BigInt.multiply(p, q);
            }
            while ((BigInt.less(mod, new BigInt(max.ToString()))) || (BigInt.equal(p,q)));

            Console.WriteLine("P: " + p.toString());
            Console.WriteLine("Q: " + q.toString());
            Console.WriteLine("mod: " + mod.toString());

            A = Karmichael(p, q);
            Console.WriteLine("A: " + A.toString());
            do
            {
                do
                {
                    e = BigInt.PrimeGenerate(0, Int32.Parse(A.toString()));
                }
                while ((BigInt.mod(A, e)).isZero());
                BigInt temp;
                BigInt.ExtendedGCD(A, e, out temp, out d);
            }
            while (BigInt.less(d, new BigInt("0")));
            Console.WriteLine("E: " + e.toString());
            Console.WriteLine("D: " + d.toString());
        }

        public void SignInitialize(int max, int powMin, int powMax)
        {
            Console.WriteLine("------------------------------------------------");
            max = (int)Math.Pow(2, max);
            powMin = (int)Math.Pow(2, powMin);
            powMax = (int)Math.Pow(2, powMax);
            do
            {
                pSign = BigInt.PrimeGenerate(powMin, powMax);
                Console.WriteLine("P Sign: " + pSign.toString());
                qSign = BigInt.PrimeGenerate(powMin, powMax);
                modSign = BigInt.multiply(pSign, qSign);
            }
            while ((BigInt.less(modSign, new BigInt(max.ToString()))) || (BigInt.equal(pSign, qSign)));

            Console.WriteLine("P Sign: " + pSign.toString());
            Console.WriteLine("Q Sign: " + qSign.toString());
            Console.WriteLine("mod Sign: " + modSign.toString());

            ASign = Karmichael(pSign, qSign);
            Console.WriteLine("A Sign: " + ASign.toString());
            do
            {
                do
                {
                    eSign = BigInt.PrimeGenerate(0, Int32.Parse(ASign.toString()));
                }
                while ((BigInt.mod(ASign, eSign)).isZero());
                BigInt temp;
                BigInt.ExtendedGCD(ASign, eSign, out temp, out dSign);
            }
            while (BigInt.less(dSign, new BigInt("0")));
            Console.WriteLine("E Sign: " + eSign.toString());
            Console.WriteLine("D Sign: " + dSign.toString());
            Console.WriteLine("------------------------------------------------");
        }

        public BigInt Hash (string word, BigInt mod)
        {
            BigInt sum = new BigInt("0");
            for (int i = 0; i < word.Length; i++)
            {
                sum = BigInt.add(sum, BigInt.multiply(new BigInt(((int)word[i]).ToString()), BigInt.pow(new BigInt("2"), new BigInt(i.ToString()))));
            }
            return sum;
        }

        public BigInt Karmichael (BigInt p, BigInt q)
        {
            BigInt one = new BigInt("1");
            return BigInt.LCM(BigInt.subtract(p, one), BigInt.subtract(q, one));
        }

        public void Encryption(string tempWord)
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Bob recieves the word " + tempWord + " and starting to work with it.");
            word = tempWord;
            List<BigInt> wordNumbers = new List<BigInt>();
            Console.WriteLine("Word's Codes:");
            for (int i = 0; i < word.Length; i++)
            {
                wordNumbers.Add(new BigInt (((int)word[i]).ToString()));
                Console.Write(wordNumbers[i].toString() + " ");
            }
            Console.WriteLine();
            wordNumbersEncrypted = new List<BigInt>();

            Console.WriteLine("Encrypted Word's Codes:");

            for (int i = 0; i < wordNumbers.Count; i++)
            {
                wordNumbersEncrypted.Add(BigInt.powByMod(wordNumbers[i], e, mod));
                Console.Write(wordNumbersEncrypted[i].toString() + " ");
            }
            SignEncrypted = BigInt.powByMod(Hash(word, modSign), dSign, modSign);
            Console.WriteLine();
            Console.WriteLine("Bob is sending numbers to Alice.");
            Console.WriteLine("------------------------------------------------");
        }

        public string Decryption()
        {
            Console.WriteLine("Alice recieves the numbers and starting to work with it.");
            Console.WriteLine("Decrypted Word's Codes:");
            List<BigInt> wordNumbers = new List<BigInt>();
            BigInt one = new BigInt("1");
            for (int i = 0; i < word.Length; i++)
            {
                //wordNumbers.Add(BigInt.powByMod(wordNumbersEncrypted[i], d, mod));
                wordNumbers.Add(ChineseTheorem(BigInt.mod(BigInt.pow(wordNumbersEncrypted[i], BigInt.mod(d, BigInt.subtract(p, one))), p), BigInt.mod(BigInt.pow(wordNumbersEncrypted[i], BigInt.mod(d, BigInt.subtract(q, one))), q), p, q));

                //Console.Write("Chinese: " + ChineseTheorem(BigInt.mod(BigInt.pow(wordNumbersEncrypted[i], BigInt.mod(d, BigInt.subtract(p, one))), p), BigInt.mod(BigInt.pow(wordNumbersEncrypted[i], BigInt.mod(d, BigInt.subtract(q, one))), q), p, q).toString());
                Console.Write(wordNumbers[i].toString() + " ");
            }
            Console.WriteLine();
            string decryptedWord = "";
            for (int i = 0; i < word.Length; i++)
            {
                decryptedWord += (char)Int32.Parse(wordNumbers[i].toString());
            }
            Console.WriteLine("Decrypted Word: " + decryptedWord);
           Console.WriteLine("Sign: " + BigInt.powByMod(SignEncrypted, eSign, modSign).toString());
            Console.WriteLine("Hash: " + BigInt.mod(Hash(word, modSign), modSign).toString());
            Console.WriteLine("------------------------------------------------");
            return decryptedWord;
        }

        public BigInt ChineseTheorem (BigInt r1, BigInt r2, BigInt mod1, BigInt mod2)
        {
           // Console.WriteLine("r1:" + r1.toString());
            BigInt M = BigInt.multiply(mod1, mod2);
            BigInt m1 = BigInt.divide(M, mod1);
            BigInt m2 = BigInt.divide(M, mod2);
            BigInt y1 = new BigInt("1");
            BigInt y2 = new BigInt("1");
            BigInt one = new BigInt("1");
            for (BigInt i = one; BigInt.less(i, mod1); i = BigInt.add(i, one))
            {
                if (BigInt.equal(BigInt.multiplyByMod(m1, i, mod1), one)) { y1 = i; break; }
            }

            for (BigInt i = one; BigInt.less(i, mod2); i = BigInt.add(i, one))
            {
                if (BigInt.equal(BigInt.multiplyByMod(m2, i, mod2), one)) { y2 = i; break; }
            }

            return BigInt.mod(BigInt.add(BigInt.multiply(m1, BigInt.multiply(r1, y1)), BigInt.multiply(m2, BigInt.multiply(r2, y2))), mod);
        }
    }
}
