using System;
using System.Linq;
using System.Security.Cryptography;

namespace EPS.Extensions.Unique
{
    public class Unique
    {
        //48-57 are numbers, 65-90 are capital letters, 97-122 are lowercase
        //62 alphanums
        public static readonly int[] AlphaNumerics = {
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80,
            81,
            82, 83, 84, 85, 86, 87, 88, 89, 90, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,
            111,
            112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122
        };
        //37
        public static readonly int[] BadPunctuation = {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
            28, 29, 30, 31, 32,
            34, 39, 96, 127,92,47
        };

        public static readonly int[] Punctuation = {
            33, 35, 36, 37, 38, 40, 41, 42, 43, 44, 45, 46, 58, 59, 60, 61, 62, 63, 64, 91, 93, 94, 95, 123,
            124, 125, 126
        };


        public static string Generate(int length, int numberOfNonAlphanumericCharacters)
        {
            //33-47 are punctuation,91-96, 123-127

            // iterate through our non alphanumeric characters
            int nacNo = 0;

            if (length < 1 || length > 128)
            {
                throw new ArgumentException(nameof(length));
            }

            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            {
                throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
            }

            using var rng = RandomNumberGenerator.Create();

            var byteBuffer = new byte[length];

            rng.GetBytes(byteBuffer);

            var characterBuffer = new char[length];
            var rando = new Random();

            for (var iter = 0; iter < length; iter++)
            {
                var i = byteBuffer[iter];

                if (i > 127 && numberOfNonAlphanumericCharacters == 0 ||
                    BadPunctuation.FirstOrDefault(bp => bp == i) > 0 && numberOfNonAlphanumericCharacters == 0)
                {
                    var n = rando.Next(1, AlphaNumerics.Length);
                    characterBuffer[iter] = (char) AlphaNumerics[n];
                    continue;
                }

                if (i > 127 && nacNo < numberOfNonAlphanumericCharacters ||
                    BadPunctuation.FirstOrDefault(bp => bp == i) > 0 && nacNo < numberOfNonAlphanumericCharacters)
                {
                    var n = rando.Next(1, Punctuation.Length);
                    characterBuffer[iter] = (char) Punctuation[n];
                    nacNo++;
                    continue;
                }

                if (Punctuation.FirstOrDefault(pc => pc == i) > 0 && numberOfNonAlphanumericCharacters == 0)
                {
                    var n = rando.Next(1, AlphaNumerics.Length);
                    nacNo++;
                    characterBuffer[iter] = (char) AlphaNumerics[n];
                    continue;
                }

                if (Punctuation.FirstOrDefault(pc => pc == i) > 0 && nacNo < numberOfNonAlphanumericCharacters)
                {
                    characterBuffer[iter] = (char) Punctuation.FirstOrDefault(pc => pc == i);
                    nacNo++;
                    continue;
                }

                characterBuffer[iter] = (char) AlphaNumerics.FirstOrDefault(an => an == i);

                if (characterBuffer[iter] != '\0') continue;
                {
                    var n = rando.Next(1, AlphaNumerics.Length);
                    characterBuffer[iter] = (char)AlphaNumerics[n];
                }

            }
            return new string(characterBuffer);
        }
    }
}
