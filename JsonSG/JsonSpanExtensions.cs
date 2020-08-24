using System;

namespace JsonSG
{
    public static class JsonSpanExtensions
    {
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