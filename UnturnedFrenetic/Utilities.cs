using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnturnedFrenetic
{
    public class Utilities
    {
        /// <summary>
        /// A UTF-8 without BOM encoding.
        /// </summary>
        public static Encoding encoding = new UTF8Encoding(false);

        /// <summary>
        /// A static random object for all non-determistic objects to use.
        /// </summary>
        public static Random UtilRandom = new Random();

        public static ushort BytesToUshort(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static float BytesToFloat(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes, 0);
        }

        public static double BytesToDouble(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes, 0);
        }

        public static byte[] UshortToBytes(ushort ush)
        {
            return BitConverter.GetBytes(ush);
        }

        public static byte[] FloatToBytes(float flt)
        {
            return BitConverter.GetBytes(flt);
        }

        public static byte[] DoubleToBytes(double flt)
        {
            return BitConverter.GetBytes(flt);
        }

        public static int BytesToInt(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        public static long BytesToLong(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes, 0);
        }

        public static byte[] IntToBytes(int intty)
        {
            return BitConverter.GetBytes(intty);
        }

        public static byte[] LongToBytes(long intty)
        {
            return BitConverter.GetBytes(intty);
        }

        public static byte[] BytesPartial(byte[] full, int start, int length)
        {
            byte[] data = new byte[length];
            Array.Copy(full, start, data, 0, length);
            return data;
        }

        public static float StepTowards(float start, float target, float amount)
        {
            if (start < target - amount)
            {
                return start + amount;
            }
            else if (start > target + amount)
            {
                return start - amount;
            }
            else
            {
                return target;
            }
        }

        public static bool IsCloseTo(float one, float target, float amount)
        {
            return one > target ? one - amount < target : one + amount > target;
        }

        /// <summary>
        /// Converts a string to a float. Returns 0 if the string is not a valid float.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The converted float.</returns>
        public static float StringToFloat(string input)
        {
            float output;
            if (float.TryParse(input, out output))
            {
                return output;
            }
            else
            {
                return 0f;
            }
        }
        
        /// <summary>
        /// Converts a string to a double. Returns 0 if the string is not a valid double.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The converted double.</returns>
        public static double StringToDouble(string input)
        {
            double output;
            if (double.TryParse(input, out output))
            {
                return output;
            }
            else
            {
                return 0f;
            }
        }

        /// <summary>
        /// Converts a string to a ushort. Returns 0 if the string is not a valid ushort.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The converted ushort.</returns>
        public static ushort StringToUShort(string input)
        {
            ushort output;
            if (ushort.TryParse(input, out output))
            {
                return output;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Converts a string to a uint. Returns 0 if the string is not a valid uint.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The converted uint.</returns>
        public static uint StringToUInt(string input)
        {
            uint output;
            if (uint.TryParse(input, out output))
            {
                return output;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Converts a string to a int. Returns 0 if the string is not a valid int.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The converted int.</returns>
        public static int StringToInt(string input)
        {
            int output;
            if (int.TryParse(input, out output))
            {
                return output;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns a string representation of the specified time.
        /// </summary>
        /// <returns>The time as a string.</returns>
        public static string DateTimeToString(DateTime dt)
        {
            string utcoffset = "";
            DateTime UTC = dt.ToUniversalTime();
            if (dt.CompareTo(UTC) < 0)
            {
                TimeSpan span = UTC.Subtract(dt);
                utcoffset = "-" + Pad(((int)Math.Floor(span.TotalHours)).ToString(), '0', 2) + ":" + Pad(span.Minutes.ToString(), '0', 2);
            }
            else
            {
                TimeSpan span = dt.Subtract(UTC);
                utcoffset = "+" + Pad(((int)Math.Floor(span.TotalHours)).ToString(), '0', 2) + ":" + Pad(span.Minutes.ToString(), '0', 2);
            }
            return Pad(dt.Year.ToString(), '0', 4) + "/" + Pad(dt.Month.ToString(), '0', 2) + "/" +
                    Pad(dt.Day.ToString(), '0', 2) + " " + Pad(dt.Hour.ToString(), '0', 2) + ":" +
                    Pad(dt.Minute.ToString(), '0', 2) + ":" + Pad(dt.Second.ToString(), '0', 2) + " UTC" + utcoffset;
        }

        /// <summary>
        /// Pads a string to a specified length with a specified input, on a specified side.
        /// </summary>
        /// <param name="input">The original string.</param>
        /// <param name="padding">The symbol to pad with.</param>
        /// <param name="length">How far to pad it to.</param>
        /// <param name="left">Whether to pad left (true), or right (false).</param>
        /// <returns>The padded string.</returns>
        public static string Pad(string input, char padding, int length, bool left = true)
        {
            int targetlength = length - input.Length;
            StringBuilder pad = new StringBuilder(targetlength <= 0 ? 1 : targetlength);
            for (int i = 0; i < targetlength; i++)
            {
                pad.Append(padding);
            }
            if (left)
            {
                return pad + input;
            }
            else
            {
                return input + pad;
            }
        }

        /// <summary>
        /// Returns a peice of text copied a specified number of times.
        /// </summary>
        /// <param name="text">What text to copy.</param>
        /// <param name="times">How many times to copy it.</param>
        /// <returns>.</returns>
        public static string CopyText(string text, int times)
        {
            StringBuilder toret = new StringBuilder(text.Length * times);
            for (int i = 0; i < times; i++)
            {
                toret.Append(text);
            }
            return toret.ToString();
        }

        /// <summary>
        /// Returns the number of times a character occurs in a string.
        /// </summary>
        /// <param name="input">The string containing the character.</param>
        /// <param name="countme">The character which the string contains.</param>
        /// <returns>How many times the character occurs.</returns>
        public static int CountCharacter(string input, char countme)
        {
            int count = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == countme)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Combines a list of strings into a single string, separated by spaces.
        /// </summary>
        /// <param name="input">The list of strings to combine.</param>
        /// <param name="start">The index to start from.</param>
        /// <returns>The combined string.</returns>
        public static string Concat(List<string> input, int start = 0)
        {
            StringBuilder output = new StringBuilder();
            for (int i = start; i < input.Count; i++)
            {
                output.Append(input[i]).Append(" ");
            }
            return (output.Length > 0 ? output.ToString().Substring(0, output.Length - 1) : "");
        }

        /// <summary>
        /// If raw string data is input by a user, call this function to clean it for tag-safety.
        /// </summary>
        /// <param name="input">The raw string.</param>
        /// <returns>A cleaned string.</returns>
        public static string CleanStringInput(string input)
        {
            // No nulls!
            return input.Replace('\0', ' ');
        }

        /// <summary>
        /// Used to identify if an input character is a valid color symbol (generally the character that follows a '^'), for use by RenderColoredText
        /// </summary>
        /// <param name="c"><paramref name="c"/>The character to check.</param>
        /// <returns>whether the character is a valid color symbol.</returns>
        public static bool IsColorSymbol(char c)
        {
            return ((c >= '0' && c <= '9') /* 0123456789 */ ||
                    (c >= 'a' && c <= 'b') /* ab */ ||
                    (c >= 'd' && c <= 'f') /* def */ ||
                    (c >= 'h' && c <= 'l') /* hijkl */ ||
                    (c >= 'n' && c <= 'u') /* nopqrstu */ ||
                    (c >= 'R' && c <= 'T') /* RST */ ||
                    (c >= '#' && c <= '&') /* #$%& */ || // 35 - 38
                    (c >= '(' && c <= '*') /* ()* */ || // 40 - 42
                    (c == 'A') ||
                    (c == 'O') ||
                    (c == '-') || // 45
                    (c == '!') || // 33
                    (c == '@') // 64
                    );
        }

        public static double PI180 = Math.PI / 180;
        
        /// <summary>
        /// Validates a username as correctly formatted.
        /// </summary>
        /// <param name="str">The username to validate.</param>
        /// <returns>Whether the username is valid.</returns>
        public static bool ValidateUsername(string str)
        {
            if (str == null)
            {
                return false;
            }
            // Length = 4-15
            if (str.Length < 4 || str.Length > 15)
            {
                return false;
            }
            // Starts A-Z
            if (!(str[0] >= 'a' && str[0] <= 'z') && !(str[0] >= 'A' && str[0] <= 'Z'))
            {
                return false;
            }
            // All symbols are A-Z, 0-9, _
            for (int i = 0; i < str.Length; i++)
            {
                if (!(str[i] >= 'a' && str[i] <= 'z') && !(str[i] >= 'A' && str[i] <= 'Z')
                    && !(str[i] >= '0' && str[i] <= '9') && !(str[i] == '_'))
                {
                    return false;
                }
            }
            // Valid if all tests above passed
            return true;
        }

        /// <summary>
        /// Calculates a Halton Sequence result.
        /// </summary>
        /// <param name="basen">Should be prime.</param>
        static float HaltonSequence(int index, int basen)
        {
            if (basen <= 1)
            {
                return 0;
            }
            float res = 0;
            float f = 1;
            int i = index;
            while (i > 0)
            {
                f = f / basen;
                res = res + f * (i % basen);
                i = (int)Math.Floor((float)i / basen);
            }
            return res;
        }
    }

    public class IntHolder
    {
        public int Value = 0;
    }
}
