using System;

namespace JsonSG
{
    public static class JsonSpanExtensions
    {
        public static ReadOnlySpan<char> ReadBool(this ReadOnlySpan<char> json, out bool value)
        {
            json = json.SkipWhitespace();
            if(json[0] == 't')
            {
                value = true;
                return json.Slice(4);
            }
            value = false;
            return json.Slice(5);
        }

        public static ReadOnlySpan<char> ReadInt(this ReadOnlySpan<char> json, out int value)
        {
            json = json.SkipWhitespace();
            int afterIntIndex = 0;
            for(int index =0; index < json.Length; index++)
            {
                var character = json[index];
                switch(character)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        continue;
                    default:
                        afterIntIndex = index;
                        break;
                }
                break;
            }
            
            var intSpan = json.Slice(0, afterIntIndex);
            value = int.Parse(json.Slice(0, afterIntIndex));

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<char> ReadUInt(this ReadOnlySpan<char> json, out uint value)
        {
            json = json.SkipWhitespace();
            int afterIntIndex = 0;
            int soFar = 0;
            for(int index =0; index < json.Length; index++)
            {
                var character = json[index];
                switch(character)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        int digit = ((int)character) - 48;
                        soFar *= 10;
                        soFar += digit; 
                        continue;

                    default:
                        afterIntIndex = index;
                        break;
                }
                break;
            }
            
            var intSpan = json.Slice(0, afterIntIndex);
            value = (uint)soFar;

            return json.Slice(afterIntIndex);
        }

        public static ReadOnlySpan<char> ReadString(this ReadOnlySpan<char> json, out string value)
        {
            json = json.SkipWhitespaceTo('\"', 'n', out char found);
            if(found == 'n')
            {
                value = null;
                return json.Slice(3);
            }
            var propertyValue = json.ReadTo('\"');
            value = new string(propertyValue);
            return json.Slice(value.Length + 1);
        }

        public static ReadOnlySpan<char> SkipWhitespace(this ReadOnlySpan<char> json)
        {
            for(int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                if(value == ' ' && value == '\t')
                {
                    continue;
                }
                return json.Slice(index);
            }
            throw new InvalidJsonException($"Unexpected end of json while skipping whitespace");
        }

        public static ReadOnlySpan<char> SkipWhitespaceTo(this ReadOnlySpan<char> json, char to)
        {
            for(int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                if(value == ' ' && value == '\t')
                {
                    continue;
                }
                if(value == to)
                {
                    index++;
                    return json.Slice(index);
                }
                throw new InvalidJsonException($"Unexpected character expected '{to}' but got '{value}' in '{new string(json)}'");
            }
            throw new InvalidJsonException($"Unexpected end of json while looking for '{to}'");
        }

        public static ReadOnlySpan<char> SkipWhitespaceTo(this ReadOnlySpan<char> json, char to1, char to2, out char found)
        {
            for(int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                if(value == ' ' && value == '\t')
                {
                    continue;
                }
                if(value == to1)
                {
                    index++;
                    found = to1;
                    return json.Slice(index);
                }
                if(value == to2)
                {
                    index++;
                    found = to2;
                    return json.Slice(index);
                }
                throw new InvalidJsonException($"Unexpected character expected '{to1}' or '{to2}' but got '{value}'");
            }
            throw new InvalidJsonException($"Unexpected end of json while looking for '{to1}' or {to2}");
        }

        public static ReadOnlySpan<char> ReadTo(this ReadOnlySpan<char> json, char to)
        {
            for(int index = 0; index < json.Length; index++)
            {
                var value = json[index];
                if(value == to)
                {
                    return json.Slice(0, index);
                }
            }
            throw new InvalidJsonException($"Unexpected end of json while looking for '{to}'");
        }

        public static bool EqualsString(this ReadOnlySpan<char> json, string other)
        {
            if(json.Length != other.Length)
            {
                return false;
            }
            for(int index = 0; index < 0; index++)
            {
                if(json[0] != other[0])
                {
                    return false;
                }
            }
            return true;
        }
    }
}