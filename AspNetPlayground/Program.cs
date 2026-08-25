using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using AspNetPlayground;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


Todo[] sampleTodos =
[
    new Todo{Id=1, Title="Walk the dog" },
    new Todo{Id=2, Title="Do the dishes", DueBy = DateTime.Now },
    new Todo{Id=3, Title="Do the laundry", DueBy = DateTime.Now.AddDays(1) },
    new Todo{Id=4, Title="Clean the bathroom" },
    new Todo{Id=5, Title="Clean the car", DueBy = DateTime.Now.AddDays(2) }
];

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", (HttpContext context) => context.JsonResult(sampleTodos))
        .WithName("GetTodos");

todosApi.MapGet("/{id}", Results<Ok<Todo>, NotFound> (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? TypedResults.Ok(todo)
        : TypedResults.NotFound())
    .WithName("GetTodoById");

app.Run();






[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
