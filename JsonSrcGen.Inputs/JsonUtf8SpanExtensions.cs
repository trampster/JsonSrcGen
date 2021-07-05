using System;
using System.Text;
using System.Globalization;
using System.Runtime.CompilerServices;

#nullable enable
namespace JsonSrcGen
{
    internal static class JsonUtf8SpanExtensions
    {
        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out bool value)
        {
            json = json.SkipWhitespace();
            switch (json[0])
            {
                case (byte)'t':
                    value = true;
                    return json.Slice(4);
                case (byte)'f':
                    value = false;
                    return json.Slice(5);
            }
            throw new InvalidJsonException("Expected 'true' or 'false'", Encoding.UTF8.GetString(json));
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out bool? value)
        {
            json = json.SkipWhitespace();
            switch (json[0])
            {
                case (byte)'t':
                    value = true;
                    return json.Slice(4);
                case (byte)'n':
                    value = null;
                    return json.Slice(4);
            }
            value = false;
            return json.Slice(5);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out short value)
        {
            json = json.SkipWhitespace();
            int sign = 1;
            int startIndex = 0;
            if (json[0] == '-')
            {
                sign = -1;
                startIndex = 1;
            }
            int afterIntIndex = 0;
            int soFar = 0;

            for (int index = startIndex; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
         				int digit = character - '0';
                        soFar *= 10;
                        soFar += digit; 
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = (short)(sign * soFar);

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out long? value)
        {
            json = json.SkipWhitespace();
            int sign = 1;
            int startIndex = 0;
            switch (json[0])
            {
                case (byte)'-':
                    sign = -1;
                    startIndex = 1;
                    break;
                case (byte)'n':
                    value = null;
                    return json.Slice(4);
            }
            int afterIntIndex = 0;
            value = 0;
            for (int index = startIndex; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    long digit = ((long)character) - 48;
                    value *= 10;
                    value += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = sign * value;

            return json.Slice(afterIntIndex);
        }

        private static readonly StringBuilder _sbNumber = new StringBuilder();

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out decimal? value)
        {
            json = json.SkipWhitespace();

            if (json[0] == 'n')
            {
                value = null;
                return json.Slice(4);
            }

            var result = json.Read(out decimal newValue);
            value = newValue;
            return result;
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out decimal value)
        {
            _sbNumber.Clear();
            int index = 0;
            int sign = 1;
            byte character = (byte)' ';
            value = 0;

            if (json[index] == '-')
            {
                sign = -1;
                index++;
            }

            for (; index < json.Length; index++)
            {
                character = json[index];

                if (!CheckNumber(json, index, character))
                    break;

                _sbNumber.Append((char)character);
            }

            if (decimal.TryParse(_sbNumber.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                value *= sign;
                return json.Slice(index);
            }

            throw new Exception("Error parse decimal");
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out float? value)
        {
            json = json.SkipWhitespace();

            if (json[0] == 'n')
            {
                value = null;
                return json.Slice(4);
            }

            var result = json.Read(out float newValue);
            value = newValue;
            return result;
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out float value)
        {
            _sbNumber.Clear();
            int index = 0;
            int sign = 1;
            byte character = (byte)' ';
            value = 0;

            if (json[index] == '-')
            {
                sign = -1;
                index++;
            }

            for (; index < json.Length; index++)
            {
                character = json[index];

                if (!CheckNumber(json, index, character))
                    break;

                _sbNumber.Append(character);
            }

            if (float.TryParse(_sbNumber.ToString(), out value))
            {
                value *= sign;
                return json.Slice(index);
            }

            throw new Exception("Error parse float");
        }


        private static bool CheckNumber(ReadOnlySpan<byte> json, int index, byte character)
        {
            int nextIndex = (index + 1);
            nextIndex = nextIndex < json.Length ? nextIndex : 0;
            byte nextChar = json[nextIndex];

            if (!(character >= '0' && character <= '9') && (nextIndex == 0 || !(nextChar >= '0' && nextChar <= '9')))
            {
                return false;
            }

            return true;
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out long value)
        {
            json = json.SkipWhitespace();
            int sign = 1;
            int startIndex = 0;
            if (json[0] == '-')
            {
                sign = -1;
                startIndex = 1;
            }
            int afterIntIndex = 0;
            value = 0;
            for (int index = startIndex; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    long digit = ((long)character) - 48;
                    value *= 10;
                    value += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = sign * value;

            return json.Slice(afterIntIndex);
        }


        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out int? value)
        {
            json = json.SkipWhitespace();
            int sign = 1;
            int startIndex = 0;

            switch (json[0])
            {
                case (byte)'-':
                    sign = -1;
                    startIndex = 1;
                    break;
                case (byte)'n':
                    value = null;
                    return json.Slice(4);
            }

            int afterIntIndex = 0;
            int soFar = 0;

            for (int index = startIndex; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    int digit = ((int)character) - 48;
                    soFar *= 10;
                    soFar += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = sign * soFar;

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out int value)
        {
            json = json.SkipWhitespace();
            int sign = 1;
            int startIndex = 0;
            if (json[0] == '-')
            {
                sign = -1;
                startIndex = 1;
            }
            int afterIntIndex = 0;
            int soFar = 0;

            for (int index = startIndex; index < json.Length; index++)
            {
                var character = json[index];

                if (character >= '0' && character <= '9')
                {
                    int digit = ((int)character) - 48;
                    soFar *= 10;
                    soFar += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = sign * soFar;

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out double? value)
        {
            json = json.SkipWhitespace();

            int index = 0;
            int sign = 1;
            byte character = (byte)' ';

            switch (json[0])
            {
                case (byte)'-':
                    sign = -1;
                    index++;
                    break;
                case (byte)'n':
                    value = null;
                    return json.Slice(4);
            }

            double wholePart = 0;
            for (; index < json.Length; index++)
            {
                character = json[index];
                if (!(character >= '0' && character <= '9')) break;
                wholePart = (wholePart * 10) + character - '0';
            }

            double fractionalPart = 0;
            if (character == '.')
            {
                long fractionalValue = 0;
                int factionalLength = 0;
                for (index = index + 1; index < json.Length; index++)
                {
                    character = json[index];
                    if (!(character >= '0' && character <= '9')) break;
                    fractionalValue = (fractionalValue * 10) + character - '0';
                    factionalLength++;
                }
                double divisor = Math.Pow(10, factionalLength);
                fractionalPart = fractionalValue / divisor;
            }

            int exponentPart = 0;
            if (character == 'E' || character == 'e')
            {
                index++;
                character = json[index];
                int exponentSign = 1;
                if (character == '-')
                {
                    index++;
                    exponentSign = -1;
                }
                else if (character == '+')
                {
                    index++;
                }

                for (; index < json.Length; index++)
                {
                    character = json[index];
                    if (!(character >= '0' && character <= '9')) break;
                    exponentPart = (exponentPart * 10) + character - '0';
                }

                exponentPart *= exponentSign;
            }
            else
            {
                value = sign * (wholePart + fractionalPart);
                return json.Slice(index);
            }
            value = sign * (wholePart + fractionalPart) * Math.Pow(10, exponentPart);
            return json.Slice(index);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out double value)
        {
            json = json.SkipWhitespace();

            int index = 0;
            int sign = 1;
            byte character = (byte)' ';

            if (json[index] == '-')
            {
                sign = -1;
                index++;
            }

            double wholePart = 0;
            for (; index < json.Length; index++)
            {
                character = json[index];
                if (!(character >= '0' && character <= '9')) break;
                wholePart = (wholePart * 10) + character - '0';
            }

            double fractionalPart = 0;
            if (character == '.')
            {
                long fractionalValue = 0;
                int factionalLength = 0;
                for (index = index + 1; index < json.Length; index++)
                {
                    character = json[index];
                    if (!(character >= '0' && character <= '9')) break;
                    fractionalValue = (fractionalValue * 10) + character - '0';
                    factionalLength++;
                }
                double divisor = Math.Pow(10, factionalLength);
                fractionalPart = fractionalValue / divisor;
            }

            int exponentPart = 0;
            if (character == 'E' || character == 'e')
            {
                index++;
                character = json[index];
                int exponentSign = 1;
                if (character == '-')
                {
                    index++;
                    exponentSign = -1;
                }
                else if (character == '+')
                {
                    index++;
                }

                for (; index < json.Length; index++)
                {
                    character = json[index];
                    if (!(character >= '0' && character <= '9')) break;
                    exponentPart = (exponentPart * 10) + character - '0';
                }

                exponentPart *= exponentSign;
            }
            else
            {
                value = sign * (wholePart + fractionalPart);
                return json.Slice(index);
            }
            value = sign * (wholePart + fractionalPart) * Math.Pow(10, exponentPart);
            return json.Slice(index);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out byte value)
        {
            json = json.SkipWhitespace();
            int afterIntIndex = 0;
            value = 0;
            for (int index = 0; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    byte digit = (byte)(((byte)character) - 48);
                    value *= 10;
                    value += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out ushort value)
        {
            json = json.SkipWhitespace();
            int afterIntIndex = 0;
            int soFar = 0;
            for (int index = 0; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    int digit = ((int)character) - 48;
                    soFar *= 10;
                    soFar += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = (ushort)soFar;

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out ulong? value)
        {
            json = json.SkipWhitespace();
            if (json[0] == 'n')
            {
                value = null;
                return json.Slice(4);
            }
            int afterIntIndex = 0;
            value = 0;
            for (int index = 0; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    ulong digit = ((ulong)character) - 48;
                    value *= 10;
                    value += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out ulong value)
        {
            json = json.SkipWhitespace();
            int afterIntIndex = 0;
            value = 0;
            for (int index = 0; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    ulong digit = ((ulong)character) - 48;
                    value *= 10;
                    value += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out uint? value)
        {
            json = json.SkipWhitespace();
            if (json[0] == 'n')
            {
                value = null;
                return json.Slice(4);
            }
            int afterIntIndex = 0;
            int soFar = 0;
            for (int index = 0; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    int digit = ((int)character) - 48;
                    soFar *= 10;
                    soFar += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = (uint)soFar;

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out uint value)
        {
            json = json.SkipWhitespace();
            int afterIntIndex = 0;
            int soFar = 0;
            for (int index = 0; index < json.Length; index++)
            {
                var character = json[index];
                if (character >= '0' && character <= '9')
                {
                    int digit = ((int)character) - 48;
                    soFar *= 10;
                    soFar += digit;
                }
                else
                {
                    afterIntIndex = index;
                    break;
                }
            }

            value = (uint)soFar;

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out string? value)
        {
            json = json.SkipWhitespace();
            if (json[0] == 'n')
            {
                value = null;
                return json.Slice(3);
            }
            for (int index = 1; index < json.Length; index++)
            {
                switch (json[index])
                {
                    case (byte)'\\':
                        json = ReadEscapedString(json, index, out value);
                        return json;
                    case (byte)'\"':
                        value = Encoding.UTF8.GetString(json.Slice(0, index));
                        return json.Slice(index + 1);
                }
            }
            throw new InvalidJsonException("Failed to find end of string", Encoding.UTF8.GetString(json));
        }

        public static ReadOnlySpan<byte> Read(this ReadOnlySpan<byte> json, out char value)
        {
            json = json.SkipWhitespace();
            if (json[0] != '\"')
            {
                throw new InvalidJsonException($"Expected Char value to start with \" but instead got {json[0]}", Encoding.UTF8.GetString(json));
            }
            switch (json[1])
            {
                case (byte)'\\':
                    json = ReadEscapedChar(json.Slice(2), out value);
                    return json.Slice(1);
                default:
                    json = ReadUTF8Char(json.Slice(1), out value);
                    return json.Slice(1);
            }
        }

        static ReadOnlySpan<byte> ReadUTF8Char(this ReadOnlySpan<byte> json, out char value)
        {
            var enc = Encoding.UTF8;

            if (json[0] <= 0x7F) //Single byte character
            {
                value = (char)json[0];
                return json.Slice(1);
            }

            byte first = json[0];
            var remainingBytes =
                ((first & 240) == 240) ? 3 : (
                ((first & 224) == 224) ? 2 : (
                ((first & 192) == 192) ? 1 : -1
            ));

            Span<char> twoCharSpan = stackalloc char[2];
            int length = enc.GetChars(json.Slice(0, remainingBytes + 1), twoCharSpan);
            if(length > 1)
            {
                throw new InvalidJsonException("Tried to deserialize to a char but value requires two chars");
            }
            value = twoCharSpan[0];
            return json.Slice(length);
        }

        static ReadOnlySpan<byte> ReadEscapedChar(this ReadOnlySpan<byte> json, out char value)
        {
            byte character = json[0];
            switch (character)
            {
                case (byte)'\"':
                case (byte)'\\':
                case (byte)'/':
                    value = (char)character;
                    break;
                case (byte)'b':
                    value = '\b';
                    break;
                case (byte)'f':
                    value = '\f';
                    break;
                case (byte)'n':
                    value = '\n';
                    break;
                case (byte)'r':
                    value = '\r';
                    break;
                case (byte)'t':
                    value = '\t';
                    break;
                case (byte)'u':
                    value = FromHex(json, 1);
                    return json.Slice(5);
                default:
                    throw new InvalidJsonException($"Unrecognized escape sequence", Encoding.UTF8.GetString(json));
            }
            return json.Slice(1);
        }

        [ThreadStatic]
        static StringBuilder? _builder;

        static ReadOnlySpan<byte> ReadEscapedString(this ReadOnlySpan<byte> json, int firstEscapeCharacterIndex, out string value)
        {
            var builder = _builder;
            if (builder == null)
            {
                builder = new StringBuilder();
                _builder = builder;
            }
            builder.Clear();
            int index = firstEscapeCharacterIndex;
            while (true)
            {
                byte character = json[index];

                if (character == '\\')
                {
                    builder.Append(Encoding.UTF8.GetString(json.Slice(0, index))); //append
                    //escape character
                    index++;
                    character = json[index];
                    switch (character)
                    {
                        case (byte)'\"':
                        case (byte)'\\':
                        case (byte)'/':
                            builder.Append((char)character);
                            break;
                        case (byte)'b':
                            builder.Append('\b');
                            break;
                        case (byte)'f':
                            builder.Append('\f');
                            break;
                        case (byte)'n':
                            builder.Append('\n');
                            break;
                        case (byte)'r':
                            builder.Append('\r');
                            break;
                        case (byte)'t':
                            builder.Append('\t');
                            break;
                        case (byte)'u':
                            index++;
                            builder.Append(FromHex(json, index));
                            index += 3;
                            break;
                    }

                    json = json.Slice(index + 1);
                    index = 0;
                    continue;
                }
                else if (character == '\"')
                {
                    //end of string value
                    builder.Append(Encoding.UTF8.GetString(json.Slice(0, index)));
                    value = builder.ToString();
                    return json.Slice(index + 1);
                }
                index++;

            }
            //we got to the end of the span without finding the end of string character
            throw new InvalidJsonException("Missing end of string value", Encoding.UTF8.GetString(json));
        }

        static char FromHex(this ReadOnlySpan<byte> json, int internalStart)
        {
            int value =
                (FromHexChar(json[internalStart]) << 12) +
                (FromHexChar(json[internalStart + 1]) << 8) +
                (FromHexChar(json[internalStart + 2]) << 4) +
                (FromHexChar(json[internalStart + 3]));
            return (char)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> SkipWhitespace(this ReadOnlySpan<byte> json)
        {
            for (int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                switch (value)
                {
                    case (byte)' ':
                    case (byte)'\t':
                    case (byte)'\n':
                    case (byte)'\r':
                        continue;
                }
                return json.Slice(index);
            }
            throw new InvalidJsonException($"Unexpected end of json while skipping whitespace", Encoding.UTF8.GetString(json));
        }

        public static ReadOnlySpan<byte> SkipWhitespaceTo(this ReadOnlySpan<byte> json, char to)
        {
            for (int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                switch (value)
                {
                    case (byte)' ':
                    case (byte)'\t':
                    case (byte)'\n':
                    case (byte)'\r':
                        continue;
                }
                if (value == to)
                {
                    index++;
                    return json.Slice(index);
                }
                throw new InvalidJsonException($"Unexpected character! expected '{to}' but got '{value}'", Encoding.UTF8.GetString(json));
            }
            throw new InvalidJsonException($"Unexpected end of json while looking for '{to}'", Encoding.UTF8.GetString(json));
        }

        public static ReadOnlySpan<byte> SkipWhitespaceTo(this ReadOnlySpan<byte> json, char to1, char to2, out char found)
        {
            for (int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                switch (value)
                {
                    case (byte)' ':
                    case (byte)'\t':
                    case (byte)'\n':
                    case (byte)'\r':
                        continue;
                }
                if (value == to1)
                {
                    index++;
                    found = to1;
                    return json.Slice(index);
                }
                if (value == to2)
                {
                    index++;
                    found = to2;
                    return json.Slice(index);
                }
                throw new InvalidJsonException($"Unexpected character! expected '{to1}' or '{to2}' but got '{value}'", Encoding.UTF8.GetString(json));
            }
            throw new InvalidJsonException($"Unexpected end of json while looking for '{to1}' or {to2}", Encoding.UTF8.GetString(json));
        }

        public static ReadOnlySpan<byte> SkipProperty(this ReadOnlySpan<byte> json)
        {
            int numberOfOpenBrackets = 0;

            bool inString = false;

            for (int index = 0; index < json.Length; index++)
            {
                var character = json[index];
                switch (character)
                {
                    case (byte)'{':
                        numberOfOpenBrackets++;
                        break;
                    case (byte)'}':
                        if (numberOfOpenBrackets == 0)
                        {
                            //end of class that the property is in
                            return json.Slice(index);
                        }
                        numberOfOpenBrackets--;
                        break;
                    case (byte)'\"':
                        inString = !inString;
                        break;
                    case (byte)',':
                        if (numberOfOpenBrackets == 0 && !inString)
                        {
                            return json.Slice(index);
                        }
                        break;
                }
            }
            throw new InvalidJsonException("Failed to find end of property", Encoding.UTF8.GetString(json));
        }

        public static ReadOnlySpan<byte> ReadTo(this ReadOnlySpan<byte> json, char to)
        {
            for (int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                if (value == to)
                {
                    return json.Slice(0, index);
                }
            }
            throw new InvalidJsonException($"Unexpected end of json while looking for '{to}'", Encoding.UTF8.GetString(json));
        }

        public static bool EqualsBytes(this ReadOnlySpan<byte> json, ReadOnlySpan<byte> other)
        {
            if (json.Length != other.Length)
            {
                return false;
            }
            for (int index = 0; index < json.Length; index++)
            {
                if (json[index] != other[index])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool EqualsString(this ReadOnlySpan<byte> json, string other)
        {
            if (json.Length != other.Length)
            {
                return false;
            }
            for (int index = 0; index < json.Length; index++)
            {
                if (json[index] != other[index])
                {
                    return false;
                }
            }
            return true;
        }

        public static ReadOnlySpan<byte> ReadDateTimeOffset(this ReadOnlySpan<byte> json, out DateTimeOffset value)
        {
            int index = 0;

            json = json.SkipWhitespace();

            if (json[0] != '\"')
            {
                throw new InvalidJsonException($"Expected DateTimeOffset property to start with a quote but instead got '{(char)json[0]}'", Encoding.UTF8.GetString(json));
            }
            json = json.Slice(1);

            int year =
                (json[0] - 48) * 1000 +
                (json[1] - 48) * 100 +
                (json[2] - 48) * 10 +
                (json[3] - 48);
            int month =
                (json[5] - 48) * 10 +
                (json[6] - 48);
            int day =
                (json[8] - 48) * 10 +
                (json[9] - 48);

            if (json[index + 10] == '\"')
            {
                value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
                return json.Slice(11);
            }

            int hour =
                (json[11] - 48) * 10 +
                (json[12] - 48);

            int minute =
                (json[14] - 48) * 10 +
                (json[15] - 48);

            int second =
                (json[17] - 48) * 10 +
                (json[18] - 48);


            index += 19;
            var character = json[index];
            if (character == '\"')
            {
                value = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);
                return json.Slice(20);
            }

            double milliseonds = 0;
            if (character == '.')
            {
                index++;
                //milliseconds
                int subSeconds = 0;
                int millisecondsStart = index;

                while (true)
                {
                    character = json[index];
                    if (character >= '0' && character <= '9')
                    {
                        subSeconds = (subSeconds * 10) + (character - 48);
                        index++;
                    }
                    else
                    {
                        int millisecondsLength = index - millisecondsStart;
                        double multiplier = 1;
                        switch (millisecondsLength)
                        {
                            case 1:
                                multiplier = 100;
                                break;
                            case 2:
                                multiplier = 10;
                                break;
                            case 3:
                                multiplier = 1;
                                break;
                            case 4:
                                multiplier = 0.1d;
                                break;
                            case 5:
                                multiplier = 0.01d;
                                break;
                            case 6:
                                multiplier = 0.001d;
                                break;
                            case 7:
                                multiplier = 0.0001d;
                                break;
                            case 8:
                                multiplier = 0.00001d;
                                break;
                        }
                        milliseonds = subSeconds * multiplier;
                        break;
                    }
                }
            }
            TimeSpan offset = TimeSpan.Zero;
            if (character == '\"')
            {

            }
            else if (character == 'Z')
            {
                index++;
            }
            else
            {
                int offsetSign = character == '-' ? -1 : 1;
                int offsetHours =
                    (json[index + 1] - 48) * 10 +
                    (json[index + 2] - 48);
                int offsetMinutes =
                    (json[index + 4] - 48) * 10 +
                    (json[index + 5] - 48);
                offset = new TimeSpan(offsetSign * offsetHours, offsetMinutes, 0);
                var localDateTime = new DateTimeOffset(year, month, day, hour, minute, second, offset).AddMilliseconds(milliseonds);
                value = localDateTime;
                return json.Slice(index + 7);
            }

            value = new DateTimeOffset(year, month, day, hour, minute, second, offset).AddMilliseconds(milliseonds);
            return json.Slice(index + 1);
        }

        public static ReadOnlySpan<byte> ReadNullableDateTimeOffset(this ReadOnlySpan<byte> json, out DateTimeOffset? value)
        {
            int index = 0;

            json = json.SkipWhitespace();

            switch (json[0])
            {
                case (byte)'n':
                    value = null;
                    return json.Slice(4);
                case (byte)'\"':
                    break;
                default:
                    throw new InvalidJsonException($"Expected DateTimeOffset? property to start with a quote or n but instead got '{(char)json[0]}'", Encoding.UTF8.GetString(json));
            }
            json = json.Slice(1);

            int year =
                (json[0] - 48) * 1000 +
                (json[1] - 48) * 100 +
                (json[2] - 48) * 10 +
                (json[3] - 48);
            int month =
                (json[5] - 48) * 10 +
                (json[6] - 48);
            int day =
                (json[8] - 48) * 10 +
                (json[9] - 48);

            if (json[index + 10] == '\"')
            {
                value = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
                return json.Slice(11);
            }

            int hour =
                (json[11] - 48) * 10 +
                (json[12] - 48);

            int minute =
                (json[14] - 48) * 10 +
                (json[15] - 48);

            int second =
                (json[17] - 48) * 10 +
                (json[18] - 48);


            index += 19;
            var character = json[index];
            if (character == '\"')
            {
                value = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);
                return json.Slice(20);
            }

            double milliseonds = 0;
            if (character == '.')
            {
                index++;
                //milliseconds
                int subSeconds = 0;
                int millisecondsStart = index;

                while (true)
                {
                    character = json[index];
                    if (character >= '0' && character <= '9')
                    {
                        subSeconds = (subSeconds * 10) + (character - 48);
                        index++;
                    }
                    else
                    {
                        int millisecondsLength = index - millisecondsStart;
                        double multiplier = 1;
                        switch (millisecondsLength)
                        {
                            case 1:
                                multiplier = 100;
                                break;
                            case 2:
                                multiplier = 10;
                                break;
                            case 3:
                                multiplier = 1;
                                break;
                            case 4:
                                multiplier = 0.1d;
                                break;
                            case 5:
                                multiplier = 0.01d;
                                break;
                            case 6:
                                multiplier = 0.001d;
                                break;
                            case 7:
                                multiplier = 0.0001d;
                                break;
                            case 8:
                                multiplier = 0.00001d;
                                break;
                        }
                        milliseonds = subSeconds * multiplier;
                        break;
                    }
                }
            }
            TimeSpan offset = TimeSpan.Zero;
            if (character == '\"')
            {

            }
            else if (character == 'Z')
            {
                index++;
            }
            else
            {
                int offsetSign = character == '-' ? -1 : 1;
                int offsetHours =
                    (json[index + 1] - 48) * 10 +
                    (json[index + 2] - 48);
                int offsetMinutes =
                    (json[index + 4] - 48) * 10 +
                    (json[index + 5] - 48);
                offset = new TimeSpan(offsetSign * offsetHours, offsetMinutes, 0);
                var localDateTime = new DateTimeOffset(year, month, day, hour, minute, second, offset).AddMilliseconds(milliseonds);
                value = localDateTime;
                return json.Slice(index + 7);
            }

            value = new DateTimeOffset(year, month, day, hour, minute, second, offset).AddMilliseconds(milliseonds);
            return json.Slice(index + 1);
        }

        public static ReadOnlySpan<byte> ReadDateTime(this ReadOnlySpan<byte> json, out DateTime value)
        {
            int index = 0;

            json = json.SkipWhitespace();

            if (json[0] != (byte)'\"')
            {
                throw new InvalidJsonException($"Expected DateTime property to start with a quote but instead got '{(char)json[0]}'", Encoding.UTF8.GetString(json));
            }
            json = json.Slice(1);

            int year =
                (json[0] - 48) * 1000 +
                (json[1] - 48) * 100 +
                (json[2] - 48) * 10 +
                (json[3] - 48);
            int month =
                (json[5] - 48) * 10 +
                (json[6] - 48);
            int day =
                (json[8] - 48) * 10 +
                (json[9] - 48);

            if (json[index + 10] == '\"')
            {
                value = new DateTime(year, month, day);
                return json.Slice(11);
            }

            int hour =
                (json[11] - 48) * 10 +
                (json[12] - 48);

            int minute =
                (json[14] - 48) * 10 +
                (json[15] - 48);

            int second =
                (json[17] - 48) * 10 +
                (json[18] - 48);


            index += 19;
            var character = json[index];
            if (character == '\"')
            {
                value = new DateTime(year, month, day, hour, minute, second);
                return json.Slice(20);
            }

            double milliseonds = 0;
            if (character == '.')
            {
                index++;
                //milliseconds
                int subSeconds = 0;
                int millisecondsStart = index;

                while (true)
                {
                    character = json[index];
                    if (character >= '0' && character <= '9')
                    {
                        subSeconds = (subSeconds * 10) + (character - 48);
                        index++;
                    }
                    else
                    {
                        int millisecondsLength = index - millisecondsStart;
                        double multiplier = 1;
                        switch (millisecondsLength)
                        {
                            case 1:
                                multiplier = 100;
                                break;
                            case 2:
                                multiplier = 10;
                                break;
                            case 3:
                                multiplier = 1;
                                break;
                            case 4:
                                multiplier = 0.1d;
                                break;
                            case 5:
                                multiplier = 0.01d;
                                break;
                            case 6:
                                multiplier = 0.001d;
                                break;
                            case 7:
                                multiplier = 0.0001d;
                                break;
                            case 8:
                                multiplier = 0.00001d;
                                break;
                        }
                        milliseonds = subSeconds * multiplier;
                        break;
                    }
                }
            }
            DateTimeKind kind = DateTimeKind.Unspecified;
            if (character == '\"')
            {

            }
            else if (character == 'Z')
            {
                kind = DateTimeKind.Utc;
                index++;
            }
            else
            {
                int offsetSign = character == '-' ? -1 : 1;
                int offsetHours =
                    (json[index + 1] - 48) * 10 +
                    (json[index + 2] - 48);
                int offsetMinutes =
                    (json[index + 4] - 48) * 10 +
                    (json[index + 5] - 48);
                var offset = new TimeSpan(offsetSign * offsetHours, offsetMinutes, 0);
                var localDateTime = new DateTimeOffset(year, month, day, hour, minute, second, offset).AddMilliseconds(milliseonds).LocalDateTime;
                value = localDateTime;
                return json.Slice(index + 7);
            }

            value = new DateTime(year, month, day, hour, minute, second, kind).AddMilliseconds(milliseonds);
            return json.Slice(index + 1);
        }

        public static ReadOnlySpan<byte> ReadNullableDateTime(this ReadOnlySpan<byte> json, out DateTime? value)
        {
            int index = 0;

            json = json.SkipWhitespace();

            switch (json[0])
            {
                case (byte)'n':
                    value = null;
                    return json.Slice(4);
                case (byte)'\"':
                    break;
                default:
                    throw new InvalidJsonException($"Expected DateTime property to start with a quote but instead got '{(char)json[0]}'", Encoding.UTF8.GetString(json));
            }

            json = json.Slice(1);

            int year =
                (json[0] - 48) * 1000 +
                (json[1] - 48) * 100 +
                (json[2] - 48) * 10 +
                (json[3] - 48);
            int month =
                (json[5] - 48) * 10 +
                (json[6] - 48);
            int day =
                (json[8] - 48) * 10 +
                (json[9] - 48);

            if (json[index + 10] == '\"')
            {
                value = new DateTime(year, month, day);
                return json.Slice(11);
            }

            int hour =
                (json[11] - 48) * 10 +
                (json[12] - 48);

            int minute =
                (json[14] - 48) * 10 +
                (json[15] - 48);

            int second =
                (json[17] - 48) * 10 +
                (json[18] - 48);


            index += 19;
            var character = json[index];
            if (character == '\"')
            {
                value = new DateTime(year, month, day, hour, minute, second);
                return json.Slice(20);
            }

            double milliseonds = 0;
            if (character == '.')
            {
                index++;
                //milliseconds
                int subSeconds = 0;
                int millisecondsStart = index;

                while (true)
                {
                    character = json[index];
                    if (character >= '0' && character <= '9')
                    {
                        subSeconds = (subSeconds * 10) + (character - 48);
                        index++;
                    }
                    else
                    {
                        int millisecondsLength = index - millisecondsStart;
                        double multiplier = 1;
                        switch (millisecondsLength)
                        {
                            case 1:
                                multiplier = 100;
                                break;
                            case 2:
                                multiplier = 10;
                                break;
                            case 3:
                                multiplier = 1;
                                break;
                            case 4:
                                multiplier = 0.1d;
                                break;
                            case 5:
                                multiplier = 0.01d;
                                break;
                            case 6:
                                multiplier = 0.001d;
                                break;
                            case 7:
                                multiplier = 0.0001d;
                                break;
                            case 8:
                                multiplier = 0.00001d;
                                break;
                        }
                        milliseonds = subSeconds * multiplier;
                        break;
                    }
                }
            }
            DateTimeKind kind = DateTimeKind.Unspecified;
            if (character == '\"')
            {

            }
            else if (character == 'Z')
            {
                kind = DateTimeKind.Utc;
                index++;
            }
            else
            {
                int offsetSign = character == '-' ? -1 : 1;
                int offsetHours =
                    (json[index + 1] - 48) * 10 +
                    (json[index + 2] - 48);
                int offsetMinutes =
                    (json[index + 4] - 48) * 10 +
                    (json[index + 5] - 48);
                var offset = new TimeSpan(offsetSign * offsetHours, offsetMinutes, 0);
                var localDateTime = new DateTimeOffset(year, month, day, hour, minute, second, offset).AddMilliseconds(milliseonds).LocalDateTime;
                value = localDateTime;
                return json.Slice(index + 7);
            }

            value = new DateTime(year, month, day, hour, minute, second, kind).AddMilliseconds(milliseonds);
            return json.Slice(index + 1);
        }

        public static byte FromHexChar(byte character)
        {
            switch (character)
            {
                case (byte)'0': return 0;
                case (byte)'1': return 1;
                case (byte)'2': return 2;
                case (byte)'3': return 3;
                case (byte)'4': return 4;
                case (byte)'5': return 5;
                case (byte)'6': return 6;
                case (byte)'7': return 7;
                case (byte)'8': return 8;
                case (byte)'9': return 9;
                case (byte)'a': return 10;
                case (byte)'A': return 10;
                case (byte)'b': return 11;
                case (byte)'B': return 11;
                case (byte)'c': return 12;
                case (byte)'C': return 12;
                case (byte)'d': return 13;
                case (byte)'D': return 13;
                case (byte)'e': return 14;
                case (byte)'E': return 14;
                case (byte)'f': return 15;
                case (byte)'F': return 15;
                default:
                    throw new InvalidJsonException("character must be a hex value");
            }
        }

        public static ReadOnlySpan<byte> ReadGuid(this ReadOnlySpan<byte> json, out Guid value)
        {
            json.SkipWhitespace();

            if (json[0] != (byte)'\"')
            {
                throw new InvalidJsonException($"Expected DateTime property to start with a quote but instead got '{(char)json[0]}'", Encoding.UTF8.GetString(json));
            }
            json = json.Slice(1);

            uint a =
                ((uint)FromHexChar(json[0]) << 28) +
                ((uint)FromHexChar(json[1]) << 24) +
                (uint)(FromHexChar(json[2]) << 20) +
                (uint)(FromHexChar(json[3]) << 16) +
                (uint)(FromHexChar(json[4]) << 12) +
                (uint)(FromHexChar(json[5]) << 8) +
                (uint)(FromHexChar(json[6]) << 4) +
                (uint)FromHexChar(json[7]);

            ushort b = (ushort)
                ((FromHexChar(json[9]) << 12) +
                (FromHexChar(json[10]) << 8) +
                (FromHexChar(json[11]) << 4) +
                FromHexChar(json[12]));

            ushort c = (ushort)
                ((FromHexChar(json[14]) << 12) +
                (FromHexChar(json[15]) << 8) +
                (FromHexChar(json[16]) << 4) +
                FromHexChar(json[17]));

            byte d = (byte)((FromHexChar(json[19]) << 4) + FromHexChar(json[20]));
            byte e = (byte)((FromHexChar(json[21]) << 4) + FromHexChar(json[22]));

            byte f = (byte)((FromHexChar(json[24]) << 4) + FromHexChar(json[25]));
            byte g = (byte)((FromHexChar(json[26]) << 4) + FromHexChar(json[27]));
            byte h = (byte)((FromHexChar(json[28]) << 4) + FromHexChar(json[29]));
            byte i = (byte)((FromHexChar(json[30]) << 4) + FromHexChar(json[31]));
            byte j = (byte)((FromHexChar(json[32]) << 4) + FromHexChar(json[33]));
            byte k = (byte)((FromHexChar(json[34]) << 4) + FromHexChar(json[35]));

            value = new Guid(a, b, c, d, e, f, g, h, i, j, k);

            return json.Slice(37);
        }

        public static ReadOnlySpan<byte> ReadNullableGuid(this ReadOnlySpan<byte> json, out Guid? value)
        {
            json.SkipWhitespace();

            switch (json[0])
            {
                case (byte)'n':
                    value = null;
                    return json.Slice(4);
                case (byte)'\"':
                    break;
                default:
                    throw new InvalidJsonException($"Expected Guid property to start with a quote but instead got '{(char)json[0]}'", Encoding.UTF8.GetString(json));
            }
            json = json.Slice(1);

            uint a =
                ((uint)FromHexChar(json[0]) << 28) +
                ((uint)FromHexChar(json[1]) << 24) +
                (uint)(FromHexChar(json[2]) << 20) +
                (uint)(FromHexChar(json[3]) << 16) +
                (uint)(FromHexChar(json[4]) << 12) +
                (uint)(FromHexChar(json[5]) << 8) +
                (uint)(FromHexChar(json[6]) << 4) +
                (uint)FromHexChar(json[7]);

            ushort b = (ushort)
                ((FromHexChar(json[9]) << 12) +
                (FromHexChar(json[10]) << 8) +
                (FromHexChar(json[11]) << 4) +
                FromHexChar(json[12]));

            ushort c = (ushort)
                ((FromHexChar(json[14]) << 12) +
                (FromHexChar(json[15]) << 8) +
                (FromHexChar(json[16]) << 4) +
                FromHexChar(json[17]));

            byte d = (byte)((FromHexChar(json[19]) << 4) + FromHexChar(json[20]));
            byte e = (byte)((FromHexChar(json[21]) << 4) + FromHexChar(json[22]));

            byte f = (byte)((FromHexChar(json[24]) << 4) + FromHexChar(json[25]));
            byte g = (byte)((FromHexChar(json[26]) << 4) + FromHexChar(json[27]));
            byte h = (byte)((FromHexChar(json[28]) << 4) + FromHexChar(json[29]));
            byte i = (byte)((FromHexChar(json[30]) << 4) + FromHexChar(json[31]));
            byte j = (byte)((FromHexChar(json[32]) << 4) + FromHexChar(json[33]));
            byte k = (byte)((FromHexChar(json[34]) << 4) + FromHexChar(json[35]));

            value = new Guid(a, b, c, d, e, f, g, h, i, j, k);

            return json.Slice(37);
        }
    }
}

#nullable restore