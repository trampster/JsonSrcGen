using System;

namespace JsonSrcGen
{
    internal class InvalidJsonException : Exception
    {
        public InvalidJsonException(string message, ReadOnlySpan<char> json) : base($"{message} at ...{LimitLength(json)}")
        {
        }

        public InvalidJsonException(string message) : base(message)
        {
        }

        static string LimitLength(ReadOnlySpan<char> json)
        {
            const int limit = 50;
            if(json.Length < limit)
            {
                return new string(json);
            }
            return new string(json.Slice(0, limit));
        }
    }
}