using System;
using System.Collections.Generic;
using System.Text;

namespace JsonSrcGen
{
    internal static class StringBuilderExtension
    {
        static StringBuilderExtension()
        {
            InitializeEscapingLookups();
        }

        static bool[] _needsEscaping = new bool[128];
        static string[] _escapeLookup = new string[128];

        static void InitializeEscapingLookups()
        {
            for(int index = 0; index < 32; index++)
            {
                _needsEscaping[index] = true;
            }
            _needsEscaping['\"'] = true;

            _needsEscaping['\\'] = true;
            _needsEscaping['/'] = true;
            _needsEscaping['\b'] = true;
            _needsEscaping['\f'] = true;
            _needsEscaping['\n'] = true;
            _needsEscaping['\r'] = true;
            _needsEscaping['\t'] = true;

            for(int index = 0; index < 32; index++)
            {
                var hex = index.ToString("X4");
                _escapeLookup[index] = "\\u" + hex;
            }
            _escapeLookup['\"'] = "\\\"";
            _escapeLookup ['\\'] = "\\\\";
            _escapeLookup['/'] = "\\/";
            _escapeLookup['\b'] = "\\b";
            _escapeLookup['\f'] = "\\f";
            _escapeLookup['\n'] = "\\n";
            _escapeLookup['\r'] = "\\r";
            _escapeLookup['\t'] = "\\t";
        }

        public static StringBuilder AppendDoubleEscaped(this StringBuilder builder, string input)
        {
            var firstBuilder = new StringBuilder();
            firstBuilder.AppendEscaped(input);
            builder.AppendEscaped(firstBuilder.ToString());
            return builder;
        }

        public static StringBuilder AppendEscaped(this StringBuilder builder, string input)
        {
            int start = 0;
            for(int index = 0; index < input.Length; index++)
            {
                char character = input[index];

                if(character < 93 && _needsEscaping[character])
                {
                    builder.Append(input, start, index - start);
                    builder.Append(_escapeLookup[character]);
                    start = index + 1;
                }
            }
            return builder
                .Append(input, start, input.Length - start);
        }

        public static StringBuilder AppendEscaped(this StringBuilder builder, char input)
        {
            builder.Append('\"');
            if(input < 93 && _needsEscaping[input])
            {
                return builder
                    .Append(_escapeLookup[input])
                    .Append('\"');
            }
            
            return builder
                .Append(input)
                .Append('\"');
        }

        public static StringBuilder AppendEscaped(this StringBuilder builder, char? input)
        {
            if(input == null)
            {
                return builder.Append("null");
            }
            char character = input.Value;

            builder.Append('\"');
            if(input < 93 && _needsEscaping[character])
            {
                return builder
                    .Append(_escapeLookup[character])
                    .Append('\"');
            }
            
            return builder
                .Append(character)
                .Append('\"');
        }

        public static StringBuilder AppendList(this StringBuilder builder, List<int> property)
        {
            builder.Append('[');
            if(property.Count >= 1)
            {
                builder.AppendInt(property[0]);
            }
            for(int index = 1; index < property.Count; index++)
            {
                builder.Append(',');
                builder.AppendInt(property[index]);
            }
            builder.Append(']');
            return builder;
        }

        public static StringBuilder AppendList<T>(this StringBuilder builder, List<T> property)
        {
            builder.Append('[');
            if(property.Count >= 1)
            {
                builder.Append(property[0]);
            }
            for(int index = 1; index < property.Count; index++)
            {
                builder.Append(',');
                builder.Append(property[index]);
            }
            builder.Append(']');
            return builder;
        }

        public static StringBuilder AppendList<T>(this StringBuilder builder, T[] property)
        {
            builder.Append('[');
            if(property.Length >= 1)
            {
                builder.Append(property[0]);
            }
            for(int index = 1; index < property.Length; index++)
            {
                builder.Append(',');
                builder.Append(property[index]);
            }
            builder.Append(']');
            return builder;
        }

        public static StringBuilder AppendList(this StringBuilder builder, int[] property)
        {
            builder.Append('[');
            if(property.Length >= 1)
            {
                builder.AppendInt(property[0]);
            }
            for(int index = 1; index < property.Length; index++)
            {
                builder.Append(',');
                builder.AppendInt(property[index]);
            }
            builder.Append(']');
            return builder;
        }

        public static StringBuilder AppendList(this StringBuilder builder, List<string> property)
        {
            if(property.Count >= 1)
            {
                builder.Append('[');
                builder.AppendEscaped(property[0]);
            }
            else
            {
                builder.Append("[]");
                return builder;
            }
            for(int index = 1; index < property.Count; index++)
            {
                builder.Append(',');
                builder.AppendEscaped(property[index]);
            }
            builder.Append("]");
            return builder;
        }

        public static StringBuilder AppendList(this StringBuilder builder, string[] property)
        {
            if(property.Length >= 1)
            {
                builder.Append('[');
                builder.AppendEscaped(property[0]);
            }
            else
            {
                builder.Append("[]");
                return builder;
            }
            for(int index = 1; index < property.Length; index++)
            {
                builder.Append(',');
                builder.AppendEscaped(property[index]);
            }
            builder.Append(']');
            return builder;
        }

        public static StringBuilder AppendInt(this StringBuilder builder, int val)
        {
            int number;
            
            if(val < 0)
            {
                builder.Append('-');
                uint num = (uint)(val*-1);
                if(num > int.MaxValue)
                {
                    //deal with the billions so that we can fit the rest in the faster int type
                    uint billionsUint = num/1000000000;
                    uint soFarUint = billionsUint*1000000000;
                    builder.Append((char)('0' + billionsUint));
                    num = num - soFarUint;
                }
                number = (int)num;
            }
            else
            {
                number = val;
            }

            int soFar = 0;

            if(number < 1000000)
            {
                if(number < 100)
                {
                    if(number < 10) goto Ones;
                    goto Tens;
                }
                if(number < 10000)
                {
                    if(number < 1000) goto Hundreds;
                    goto Thousands;
                }
                if(number < 100000) goto TenThousands;
                goto HundredThousands;
            }
            if(number < 100000000)
            {
                if(number < 10000000) goto Millions;
                goto TenMillions;
            }
            if(number < 1000000000) goto HundredMillions;

            int billions = (number)/1000000000;
            soFar += billions*1000000000;
            builder.Append((char)('0' + billions));

            HundredMillions:
            int hundredMillions = (number-soFar)/100000000;
            soFar += hundredMillions*100000000;
            builder.Append((char)('0' + hundredMillions));

            TenMillions:
            int tenMillions = (number-soFar)/10000000;
            soFar += tenMillions*10000000;
            builder.Append((char)('0' + tenMillions));

            Millions:
            int millions = (number-soFar)/1000000;
            soFar += millions*1000000;
            builder.Append((char)('0' + millions));

            HundredThousands:
            int hundredThousands = (number-soFar)/100000;
            soFar += hundredThousands*100000;
            builder.Append((char)('0' + hundredThousands));

            TenThousands:
            int tenThousands = (number-soFar)/10000;
            soFar += tenThousands*10000;
            builder.Append((char)('0' + tenThousands));

            Thousands:
            int thousands = (number-soFar)/1000;
            soFar += thousands*1000;
            builder.Append((char)('0' + thousands));

            Hundreds:
            int hundreds = (number-soFar)/100;
            soFar += hundreds*100;
            builder.Append((char)('0' + hundreds));
            
            Tens:
            int tens = (number - soFar)/10;
            soFar += tens*10;
            builder.Append((char)('0' + tens));

            Ones:
            int ones = number - soFar;
            return builder.Append((char)('0' + ones));
        }


        static string _offset;
        static int _offsetCacheTime = 0;

        public static string GetOffset()
        {
            int tickCount = Environment.TickCount;
            if(_offsetCacheTime + 1000 < tickCount)
            {
                _offsetCacheTime = tickCount;
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                var builder = new StringBuilder();
                if(offset.TotalMinutes > 0) builder.Append('+');
                else builder.Append('-');
                builder.AppendIntTwo(Math.Abs(offset.Hours));
                builder.Append(':');
                builder.AppendIntTwo(offset.Minutes);
                _offset = builder.ToString();
            }
            return _offset;
        }

        public static StringBuilder AppendDate(this StringBuilder builder, DateTime date)
        {
            builder
                .Append('\"')
                .AppendIntFour(date.Year)
                .Append('-')
                .AppendIntTwo(date.Month)
                .Append('-')
                .AppendIntTwo(date.Day)
                .Append('T')
                .AppendIntTwo(date.Hour)
                .Append(':')
                .AppendIntTwo(date.Minute)
                .Append(':')
                .AppendIntTwo(date.Second);
            var fractions = date.Ticks % TimeSpan.TicksPerSecond * TimeSpan.TicksPerMillisecond;
            if(fractions != 0)
            {
                builder
                    .Append('.')
                    .AppendDateTimeFraction(fractions);
            }

            //offset
            if(date.Kind == DateTimeKind.Utc)
            {
                return builder.Append("Z\"");
            }
            else if(date.Kind == DateTimeKind.Unspecified)
            {
                return builder.Append('\"');
            }
            builder
                .Append(GetOffset())
                .Append('\"');
            
            return builder;
        }

        public static StringBuilder AppendIntTwo(this StringBuilder builder, int number)
        {
            int tens = number/10;
            int soFar = tens*10;
            builder.Append((char)('0' + tens));

            int ones = number - soFar;
            return builder.Append((char)('0' + ones));
        }

        public static StringBuilder AppendIntFour(this StringBuilder builder, int number)
        {
            int thousands = (number)/1000;
            int soFar = thousands*1000;
            builder.Append((char)('0' + thousands));

            int hundreds = (number-soFar)/100;
            soFar += hundreds*100;
            builder.Append((char)('0' + hundreds));
            
            int tens = (number - soFar)/10;
            soFar += tens*10;
            builder.Append((char)('0' + tens));

            int ones = number - soFar;
            return builder.Append((char)('0' + ones));
        }

        public static StringBuilder AppendDateTimeFraction(this StringBuilder builder, long number)
        {
            //24 973 300 000
            long soFar = 0;

            long tenBillions = (number)/10000000000;
            soFar += tenBillions*10000000000;
            builder.Append((char)('0' + tenBillions));
            long leftLong = number-soFar;
            if(leftLong == 0) return builder;

            long billions = leftLong/1000000000;
            builder.Append((char)('0' + billions));
            int left = (int)(leftLong-(billions*1000000000));
            if(left == 0) return builder;

            int hundredMillions = left/100000000;
            builder.Append((char)('0' + hundredMillions));
            left = left-(hundredMillions*100000000);
            if(left == 0) return builder;

            int tenMillions = left/10000000;
            builder.Append((char)('0' + tenMillions));
            left = left-(tenMillions*10000000);
            if(left == 0) return builder;

            int millions = left/1000000;
            builder.Append((char)('0' + millions));
            left = left-(millions*1000000);
            if(left == 0) return builder;

            int hundredThousands = left/100000;
            builder.Append((char)('0' + hundredThousands));
            left = left-(hundredThousands*100000);
            if(left == 0) return builder;

            int tenThousands = left/10000;
            builder.Append((char)('0' + tenThousands));
            left = left-(tenThousands*10000);
            if(left == 0) return builder;

            int thousands = left/1000;
            builder.Append((char)('0' + thousands));
            left = left-(thousands*1000);
            if(left == 0) return builder;

            int hundreds = left/100;
            builder.Append((char)('0' + hundreds));
            left = left-(hundreds*100);
            if(left == 0) return builder;

            int tens = left/10;
            builder.Append((char)('0' + tens));
            left = left-(tens*10);
            if(left == 0) return builder;

            int ones = left;
            return builder.Append((char)('0' + ones));
        }
    }
}