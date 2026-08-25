using JsonSrcGen;

namespace AspNetPlayground;

public static class JsonSrcGenHttpContextExtensions
{
    static readonly JsonConverter _converter = new();

    public static ValueTask JsonResult(this HttpContext httpContext, Todo[] todo)
    {
        httpContext.Response.ContentType = "application/json";

        var utf8Bytes = _converter.ToJsonUtf8(todo);

        var writer = httpContext.Response.BodyWriter;
        var buffer = writer.GetSpan(utf8Bytes.Length);
        utf8Bytes.CopyTo(buffer);
        writer.Advance(utf8Bytes.Length);
        writer.Complete();

        return ValueTask.CompletedTask;
    }
}