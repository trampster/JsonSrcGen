
[assembly: JsonSrcGen.JsonArray(typeof(AspNetPlayground.Todo))]

namespace AspNetPlayground;

[JsonSrcGen.Json]
public class Todo
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime? DueBy
    {
        get;
        set;
    }
    public bool IsComplete
    {
        get;
        set;
    }
}