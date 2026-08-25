using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using JsonSrcGen.AspNet;

public class JsonSrcGenOutputFormatter : TextOutputFormatter
{
    public CustomJsonOutputFormatter()
    {
        // Define which media types this formatter supports
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
        SupportedEncodings.Add(Encoding.UTF8);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;

        // Serialize the object using your custom JSON library
        string jsonOutput = CustomJsonSerializer.Stringify(context.Object);

        await response.WriteAsync(jsonOutput, selectedEncoding);
    }
}