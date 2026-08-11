using System;
using System.Text;

namespace JsonSrcGen
{
    internal interface ICustomConverter<T>
    {
        void ToJson(IJsonBuilder builder, T target);

        ReadOnlySpan<char> FromJson(ReadOnlySpan<char> span, scoped ref T value);
        ReadOnlySpan<byte> FromJson(ReadOnlySpan<byte> span, scoped ref T value);
    }
}