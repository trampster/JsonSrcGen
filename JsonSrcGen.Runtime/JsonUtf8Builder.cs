using System;

namespace JsonSrcGen
{
    public class JsonUtf8Builder : IJsonBuilder
    {
        byte[] _buffer = new byte[5];
        int _index = 0;

        public JsonUtf8Builder()
        {
            InitializeEscapingLookups();
        }

        void ResizeBuffer(int added)
        {
            var newSize = Math.Max(_buffer.Length * 2, _buffer.Length + added);
            var newArray = new byte[newSize];
            Array.Copy(_buffer, newArray, _buffer.Length);
            _buffer = newArray;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IJsonBuilder Append(string value)
        {
            if (_index + value.Length > _buffer.Length)
            {
                ResizeBuffer(value.Length);
            }

            _index += System.Text.Encoding.UTF8.GetBytes(value, _buffer.AsSpan(_index));

            return this;
        }

        public IJsonBuilder Append(ReadOnlySpan<char> value)
        {
            if (_index + value.Length > _buffer.Length)
            {
                ResizeBuffer(value.Length);
            }

            _index += System.Text.Encoding.UTF8.GetBytes(value, _buffer.AsSpan(_index));

            return this;
        }

        public IJsonBuilder AppendAscii(char value)
        {
            if (_index + 1 > _buffer.Length)
            {
                ResizeBuffer(1);
            }
            _buffer[_index] = (byte)value;
            _index += 1;
            return this;
        }

        public IJsonBuilder Append(byte value)
        {
            // if (_index + 3 > _buffer.Length)
            // {
            //     ResizeBuffer(3);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(short value)
        {
            // if (_index + 6 > _buffer.Length)
            // {
            //     ResizeBuffer(6);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(ushort value)
        {
            // if (_index + 5 > _buffer.Length)
            // {
            //     ResizeBuffer(5);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(int value)
        {
            // if (_index + 11 > _buffer.Length)
            // {
            //     ResizeBuffer(11);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(uint value)
        {
            // if (_index + 10 > _buffer.Length)
            // {
            //     ResizeBuffer(10);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(long value)
        {
            // if (_index + 19 > _buffer.Length)
            // {
            //     ResizeBuffer(19);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(ulong value)
        {
            // if (_index + 20 > _buffer.Length)
            // {
            //     ResizeBuffer(20);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(float value)
        {
            // if (_index + 19 > _buffer.Length)
            // {
            //     ResizeBuffer(19);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(double value)
        {
            // if (_index + 19 > _buffer.Length)
            // {
            //     ResizeBuffer(19);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public IJsonBuilder Append(Guid value)
        {
            // if (_index + 36 > _buffer.Length)
            // {
            //     ResizeBuffer(36);
            // }
            // value.TryFormat(_buffer.AsSpan(_index), out int charsWriten);
            // _index += charsWriten;
            // return this;
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _index = 0;
        }

        public override string ToString()
        {
            // return new string(_buffer.AsSpan(0, _index));
            throw new NotImplementedException();
        }

        public ReadOnlySpan<char> AsSpan()
        {
            // return _buffer.AsSpan(0, _index);
            throw new NotImplementedException();
        }

        bool[] _needsEscaping = new bool[128];
        string[] _escapeLookup = new string[128];

        void InitializeEscapingLookups()
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

        public IJsonBuilder AppendEscaped(char input)
        {
            if(input < 93 && _needsEscaping[input])
            {
                return Append(_escapeLookup[input]);
            }
            return Append(input);
        }

        public IJsonBuilder AppendEscaped(string input)
        {
            int start = 0;
            for(int index = 0; index < input.Length; index++)
            {
                char character = input[index];

                if(character < 93 && _needsEscaping[character])
                {
                    this.Append(input.AsSpan(start, index - start));
                    this.Append(_escapeLookup[character]);
                    start = index + 1;
                }
            }
            return this
                .Append(input.AsSpan(start, input.Length - start));
        }

        string _offset = "";
        int _offsetCacheTime = 0;

        string GetOffset()
        {
            int tickCount = Environment.TickCount;
            if(_offsetCacheTime + 1000 < tickCount)
            {
                _offsetCacheTime = tickCount;
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                var builder = new JsonUtf8Builder();
                if(offset.TotalMinutes > 0) builder.Append('+');
                else builder.Append('-');
                builder.AppendIntTwo(Math.Abs(offset.Hours));
                builder.Append(':');
                builder.AppendIntTwo(offset.Minutes);
                _offset = builder.ToString();
            }
            return _offset;
        }

        IJsonBuilder AppendOffset(TimeSpan offset)
        {
            if (_index + 6 > _buffer.Length)
            {
                ResizeBuffer(6);
            }
            if(offset.TotalMinutes >= 0) Append('+');
            else Append('-');
            AppendIntTwo(Math.Abs(offset.Hours));
            Append(':');
            AppendIntTwo(Math.Abs(offset.Minutes));
            return this;
        }

        public IJsonBuilder AppendDate(DateTime date)
        {
            Append('\"');
            AppendIntFour(date.Year);
            Append('-');
            AppendIntTwo(date.Month);
            Append('-');
            AppendIntTwo(date.Day);
            Append('T');
            AppendIntTwo(date.Hour);
            Append(':');
            AppendIntTwo(date.Minute);
            Append(':');
            AppendIntTwo(date.Second);
            var fractions = date.Ticks % TimeSpan.TicksPerSecond * TimeSpan.TicksPerMillisecond;
            if(fractions != 0)
            {
                Append('.');
                AppendDateTimeFraction(fractions);
            }

            //offset
            if(date.Kind == DateTimeKind.Utc)
            {
                return Append("Z\"");
            }
            else if(date.Kind == DateTimeKind.Unspecified)
            {
                return Append('\"');
            }
            Append(GetOffset());
            Append('\"');
            
            return this;
        }

        public IJsonBuilder AppendDateTimeOffset(DateTimeOffset date)
        {
            Append('\"');
            AppendIntFour(date.Year);
            Append('-');
            AppendIntTwo(date.Month);
            Append('-');
            AppendIntTwo(date.Day);
            Append('T');
            AppendIntTwo(date.Hour);
            Append(':');
            AppendIntTwo(date.Minute);
            Append(':');
            AppendIntTwo(date.Second);
            var fractions = date.Ticks % TimeSpan.TicksPerSecond * TimeSpan.TicksPerMillisecond;
            if(fractions != 0)
            {
                Append('.');
                AppendDateTimeFraction(fractions);
            }

            //offset
            AppendOffset(date.Offset);
            Append('\"');
            
            return this;
        }

        JsonUtf8Builder AppendIntTwo(int number)
        {
            if (_index + 2 > _buffer.Length)
            {
                ResizeBuffer(2);
            }
            int tens = number/10;
            int soFar = tens*10;
            _buffer[_index] = (byte)('0' + tens);
            _index++;

            int ones = number - soFar;
            _buffer[_index] = (byte)('0' + ones);
            _index++;
            return this;
        }

        JsonUtf8Builder AppendIntFour(int number)
        {
            if (_index + 4 > _buffer.Length)
            {
                ResizeBuffer(4);
            }
            int thousands = (number)/1000;
            int soFar = thousands*1000;
            _buffer[_index] = (byte)('0' + thousands);
            _index++;

            int hundreds = (number-soFar)/100;
            soFar += hundreds*100;
            _buffer[_index] = (byte)('0' + hundreds);
            _index++;
            
            int tens = (number - soFar)/10;
            soFar += tens*10;
            _buffer[_index] = (byte)('0' + tens);
            _index++;

            int ones = number - soFar;
            _buffer[_index] = (byte)('0' + ones);
            _index++;
            return this;
        }

        JsonUtf8Builder AppendDateTimeFraction(long number)
        {
            if (_index + 11 > _buffer.Length)
            {
                ResizeBuffer(11);
            }

            //24 973 300 000
            long soFar = 0;

            long tenBillions = (number)/10000000000;
            soFar += tenBillions*10000000000;
            _buffer[_index] = (byte)('0' + tenBillions);
            _index++;
            long leftLong = number-soFar;
            if(leftLong == 0) return this;

            long billions = leftLong/1000000000;
            _buffer[_index] = (byte)('0' + billions);
            _index++;
            int left = (int)(leftLong-(billions*1000000000));
            if(left == 0) return this;

            int hundredMillions = left/100000000;
            _buffer[_index] = (byte)('0' + hundredMillions);
            _index++;
            left = left-(hundredMillions*100000000);
            if(left == 0) return this;

            int tenMillions = left/10000000;
            _buffer[_index] = (byte)('0' + tenMillions);
            _index++;
            left = left-(tenMillions*10000000);
            if(left == 0) return this;

            int millions = left/1000000;
            _buffer[_index] = (byte)('0' + millions);
            _index++;
            left = left-(millions*1000000);
            if(left == 0) return this;

            int hundredThousands = left/100000;
            _buffer[_index] = (byte)('0' + hundredThousands);
            _index++;
            left = left-(hundredThousands*100000);
            if(left == 0) return this;

            int tenThousands = left/10000;
            _buffer[_index] = (byte)('0' + tenThousands);
            _index++;
            left = left-(tenThousands*10000);
            if(left == 0) return this;

            int thousands = left/1000;
            _buffer[_index] = (byte)('0' + thousands);
            _index++;
            left = left-(thousands*1000);
            if(left == 0) return this;

            int hundreds = left/100;
            _buffer[_index] = (byte)('0' + hundreds);
            _index++;
            left = left-(hundreds*100);
            if(left == 0) return this;

            int tens = left/10;
            _buffer[_index] = (byte)('0' + tens);
            _index++;
            left = left-(tens*10);
            if(left == 0) return this;

            int ones = left;
            _buffer[_index] = (byte)('0' + ones);
            _index++;
            return this;
        }
    }
}