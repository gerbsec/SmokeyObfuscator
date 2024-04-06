using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Numerics;

namespace ProtoRandom
{
    public class ProtoRandom
    {
        /// <summary>
        /// Complexity of the selection of the random value.
        /// </summary>
        public int Complexity { get; set; }

        /// <summary>
        /// Create a new instance of ProtoRandom.
        /// </summary>
        /// <param name="complexity">Complexity of selection of the random value.</param>
        public ProtoRandom(int complexity = 100)
        {
            Complexity = complexity;
        }

        /// <summary>
        /// Generate a random byte array of a specific size.
        /// </summary>
        /// <param name="size">Size of the byte array.</param>
        /// <returns>A random byte array.</returns>
        public byte[] GetRandomByteArray(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] random = new byte[size];
            rng.GetBytes(random);
            return random;
        }

        /// <summary>
        /// Generate a random byte array of a specific size.
        /// </summary>
        /// <param name="size">Size of the byte array.</param>
        /// <returns>A random byte array.</returns>
        public byte[] GetRandomBytes(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] random = new byte[size];
            rng.GetBytes(random);
            return random;
        }

        /// <summary>
        /// Generate a random byte array with a size between min and max values.
        /// </summary>
        /// <param name="min">Minimum size of the byte array.</param>
        /// <param name="max">Maximum size of the byte array.</param>
        /// <returns>A random byte array.</returns>
        public byte[] GetRandomByteArray(int min, int max)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] random = new byte[GetRandomInt32(min, max)];
            rng.GetBytes(random);
            return random;
        }

        /// <summary>
        /// Generate a random byte array with a size between min and max values.
        /// </summary>
        /// <param name="min">Minimum size of the byte array.</param>
        /// <param name="max">Maximum size of the byte array.</param>
        /// <returns>A random byte array.</returns>
        public byte[] GetRandomBytes(int min, int max)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] random = new byte[GetRandomInt32(min, max)];
            rng.GetBytes(random);
            return random;
        }

        /// <summary>
        /// Generate a random integer (Int32).
        /// </summary>
        /// <returns>A random integer (Int32).</returns>
        public int GetRandomInt32()
        {
            List<int[]> arrays = new List<int[]>();

            for (int i = 0; i < Complexity; i++)
            {
                int[] values = new int[Complexity];

                for (int j = 0; j < Complexity; j++)
                {
                    values[j] = GetBasicRandomInt32();
                }

                arrays.Add(values);
            }

            return arrays[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
        }

        /// <summary>
        /// Generate a random integer (Int32).
        /// </summary>
        /// <param name="max">Maximum value of the integer.</param>
        /// <returns>A random integer (Int32).</returns>
        public int GetRandomInt32(int max)
        {
            return GetRandomInt32() % (max + 1);
        }

        /// <summary>
        /// Generate a random integer (Int32).
        /// </summary>
        /// <param name="min">Minimum value of the integer.</param>
        /// <param name="max">Maximum value of the integer.</param>
        /// <returns>A random integer (Int32).</returns>
        public int GetRandomInt32(int min, int max)
        {
            return GetRandomInt32() % (max - min + 1) + min;
        }

        /// <summary>
        /// Generate a random boolean.
        /// </summary>
        /// <returns>A random boolean.</returns>
        public bool GetRandomBoolean()
        {
            if (GetRandomInt32(1) == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Generate a random string with specific length and set of characters.
        /// </summary>
        /// <param name="chars">Set of characters.</param>
        /// <param name="length">Length of the random string.</param>
        /// <returns>A random string.</returns>
        public string GetRandomString(char[] chars, int length)
        {
            string value = "";

            for (int i = 0; i < length; i++)
            {
                value += chars[GetRandomInt32(0, chars.Length - 1)];
            }

            return value;
        }

        /// <summary>
        /// Generate a random string with specific length and set of characters.
        /// </summary>
        /// <param name="chars">Set of characters.</param>
        /// <param name="length">Length of the random string.</param>
        /// <returns>A random string.</returns>
        public string GetRandomString(string chars, int length)
        {
            string value = "";

            for (int i = 0; i < length; i++)
            {
                value += chars[GetRandomInt32(0, chars.Length - 1)];
            }

            return value;
        }

        /// <summary>
        /// Generate a random string with length from min to max value and specific set of characters.
        /// </summary>
        /// <param name="chars">Set of characters.</param>
        /// <param name="min">Minimum length of the string.</param>
        /// <param name="max">Maximum length of the string.</param>
        /// <returns>A random string.</returns>
        public string GetRandomString(char[] chars, int min, int max)
        {
            return GetRandomString(chars, GetRandomInt32(min, max));
        }

        /// <summary>
        /// Generate a random string with length from min to max value and specific set of characters.
        /// </summary>
        /// <param name="chars">Set of characters.</param>
        /// <param name="min">Minimum length of the string.</param>
        /// <param name="max">Maximum length of the string.</param>
        /// <returns>A random string.</returns>
        public string GetRandomString(string chars, int min, int max)
        {
            return GetRandomString(chars, GetRandomInt32(min, max));
        }

        /// <summary>
        /// Generate a random unsigned integer (UInt32).
        /// </summary>
        /// <returns>A random unsigned integer (UInt32).</returns>
        public uint GetRandomUInt32()
        {
            List<uint[]> arrays = new List<uint[]>();

            for (int i = 0; i < Complexity; i++)
            {
                uint[] values = new uint[Complexity];

                for (int j = 0; j < Complexity; j++)
                {
                    values[j] = GetBasicRandomUInt32();
                }

                arrays.Add(values);
            }

            return arrays[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
        }

        /// <summary>
        /// Generate a random unsigned integer (UInt32) with a maximum value.
        /// </summary>
        /// <param name="max">Maximum value of the uint.</param>
        /// <returns>A random unsigned integer (UInt32).</returns>

        /// <summary>
        /// Generate a random short (Int16).
        /// </summary>
        /// <returns>A random short (Int16).</returns>
        public short GetRandomInt16()
        {
            List<short[]> arrays = new List<short[]>();

            for (int i = 0; i < Complexity; i++)
            {
                short[] values = new short[Complexity];

                for (int j = 0; j < Complexity; j++)
                {
                    values[j] = GetBasicRandomInt16();
                }

                arrays.Add(values);
            }

            return arrays[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
        }

        /// <summary>
        /// Generate a random short (Int16) between 0 and a maximum value.
        /// </summary>
        /// <param name="max">Maximum value of the short.</param>
        /// <returns>A random short (Int16).</returns>

        /// <summary>
        /// Generate a random unsigned short (UInt16).
        /// </summary>
        /// <returns>A random unsigned short (UInt16).</returns>
        public ushort GetRandomUInt16()
        {
            List<ushort[]> arrays = new List<ushort[]>();

            for (int i = 0; i < Complexity; i++)
            {
                ushort[] values = new ushort[Complexity];

                for (int j = 0; j < Complexity; j++)
                {
                    values[j] = GetBasicRandomUInt16();
                }

                arrays.Add(values);
            }

            return arrays[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
        }

        /// <summary>
        /// Generate a random unsigned short (UInt16) between 0 and a maximum value.
        /// </summary>
        /// <param name="max">Maximum value of the ushort.</param>
        /// <returns>A random unsigned short (UInt16).</returns>

        /// <summary>
        /// Generate a random long (Int64).
        /// </summary>
        /// <returns></returns>
        public long GetRandomInt64()
        {
            List<long[]> arrays = new List<long[]>();

            for (int i = 0; i < Complexity; i++)
            {
                long[] values = new long[Complexity];

                for (int j = 0; j < Complexity; j++)
                {
                    values[j] = GetBasicRandomInt64();
                }

                arrays.Add(values);
            }

            return arrays[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
        }

        /// <summary>
        /// Generate a random long (Int64) between 0 and a maximum value.
        /// </summary>
        /// <param name="max">Maximum value of the long.</param>
        /// <returns>A random long (Int64).</returns>
        public long GetRandomInt64(long max)
        {
            return GetRandomInt64() % (max + 1);
        }

        /// <summary>
        /// Generate a random long (Int64) between a minimum and a maximum value.
        /// </summary>
        /// <param name="min">Minimum value of the long.</param>
        /// <param name="max">Maximum value of the long.</param>
        /// <returns>A random long (Int64).</returns>
        public long GetRandomInt64(long min, long max)
        {
            return GetRandomInt64() % (max - min + 1) + min;
        }

        /// <summary>
        /// Generate a random unsigned long (UInt64).
        /// </summary>
        /// <returns>A random unsigned long (UInt64).</returns>
        public ulong GetRandomUInt64()
        {
            List<ulong[]> arrays = new List<ulong[]>();

            for (int i = 0; i < Complexity; i++)
            {
                ulong[] values = new ulong[Complexity];

                for (int j = 0; j < Complexity; j++)
                {
                    values[j] = GetBasicRandomUInt64();
                }

                arrays.Add(values);
            }

            return arrays[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
        }

        /// <summary>
        /// Generate a random unsigned long (UInt64) between 0 and a maximum value.
        /// </summary>
        /// <param name="max">Maximum value of the ulong.</param>
        /// <returns>A random unsigned long (UInt64).</returns>

        /// <summary>
        /// Generate a random double.
        /// </summary>
        /// <returns>A random double.</returns>
        public double GetRandomDouble()
        {
            List<double[]> arrays = new List<double[]>();

            for (int i = 0; i < Complexity; i++)
            {
                double[] values = new double[Complexity];

                for (int j = 0; j < Complexity; j++)
                {
                    values[j] = GetBasicRandomDouble();
                }

                arrays.Add(values);
            }

            return arrays[GetBasicRandomInt32() % Complexity][GetBasicRandomInt32() % Complexity];
        }

        /// <summary>
        /// Generate a random double between 0 and a maximum value.
        /// </summary>
        /// <param name="max">Maximum value of the double.</param>
        /// <returns>A random double.</returns>
        public double GetRandomDouble(double max)
        {
            return Modulo(GetRandomDouble(), max + 1);
        }

        /// <summary>
        /// Generate a random double between a minimum and maximum value.
        /// </summary>
        /// <param name="min">Minimum value of the double.</param>
        /// <param name="max">Maximum value of the double.</param>
        /// <returns>A random double.</returns>
        public double GetRandomDouble(double min, double max)
        {
            return Modulo(GetRandomDouble(), max - min + 1) + min;
        }

        #region Basic Operations

        /// <summary>
        /// Get the modulo of two double values.
        /// </summary>
        /// <param name="a">First double value.</param>
        /// <param name="b">Second double value</param>
        /// <returns>Modulo of two double values.</returns>
        private double Modulo(double a, double b)
        {
            double closest, residue;

            if (b == Math.Floor(b))
            {
                return (a % b);
            }
            else
            {
                closest = Math.Round(a / b);
                residue = Math.Abs(a - closest * b);

                if (residue < Math.Pow(10.0, -14))
                {
                    return 0.0;
                }
                else
                {
                    return residue * Math.Sign(a);
                }
            }
        }

        /// <summary>
        /// Get the modulo of two BigInteger values.
        /// </summary>
        /// <param name="a">First BigInteger value.</param>
        /// <param name="b">Second BigInteger value.</param>
        /// <returns>Modulo of two BigInteger values.</returns>

        /// <summary>
        /// Get a basic random integer (Int32) value.
        /// </summary>
        /// <returns>A basic random integer (Int32) value.</returns>
        private int GetBasicRandomInt32()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            int value = BitConverter.ToInt32(randomNumber, 0);

            if (value < 0)
            {
                value *= -1;
            }

            return value;
        }

        /// <summary>
        /// Get a basic random unsigned integer (IInt32) value.
        /// </summary>
        /// <returns>A basic random unsigned integer (UInt32) value.</returns>
        private uint GetBasicRandomUInt32()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            uint value = BitConverter.ToUInt32(randomNumber, 0);
            return value;
        }

        /// <summary>
        /// Get a basic random long (Int64) value.
        /// </summary>
        /// <returns>A basic random long (Int64) value.</returns>
        private long GetBasicRandomInt64()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[8];
            rng.GetBytes(randomNumber);
            long value = BitConverter.ToInt64(randomNumber, 0);

            if (value < 0)
            {
                value *= -1;
            }

            return value;
        }

        /// <summary>
        /// Get a basic random unsigned long (UInt64) value.
        /// </summary>
        /// <returns>A basic random unsigned long (UInt64) value.</returns>
        private ulong GetBasicRandomUInt64()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[8];
            rng.GetBytes(randomNumber);
            ulong value = BitConverter.ToUInt64(randomNumber, 0);
            return value;
        }

        /// <summary>
        /// Get a basic random short (Int16) value.
        /// </summary>
        /// <returns>A basic random short (Int16) value.</returns>
        private short GetBasicRandomInt16()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[2];
            rng.GetBytes(randomNumber);
            short value = BitConverter.ToInt16(randomNumber, 0);

            if (value < 0)
            {
                value *= -1;
            }

            return value;
        }

        /// <summary>
        /// Get a basic random unsigned short (UInt16) value.
        /// </summary>
        /// <returns>A basic random unsigned short (UInt16) value.</returns>
        private ushort GetBasicRandomUInt16()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[2];
            rng.GetBytes(randomNumber);
            ushort value = BitConverter.ToUInt16(randomNumber, 0);
            return value;
        }

        /// <summary>
        /// Get a basic random double value.
        /// </summary>
        /// <returns>A basic random double value.</returns>
        private double GetBasicRandomDouble()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[8];
            rng.GetBytes(randomNumber);
            double value = BitConverter.ToDouble(randomNumber, 0);

            if (value < 0)
            {
                value *= -1;
            }

            if (value.ToString().Contains("E"))
            {
                value = double.Parse(value.ToString().Split('E')[0]);
            }

            return value;
        }

        #endregion
    }
}