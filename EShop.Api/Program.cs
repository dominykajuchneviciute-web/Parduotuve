var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

var items = new List<ItemDto>
{
    new(1, "Miesto dviratis", "Tvarkingas dviratis, važiuoja puikiai.", "Transportas", "Gyvai", "Elektroninį paspirtuką arba išmanųjį laikrodį"),
    new(2, "Dell 24\" monitorius", "Full HD raiškos monitorius be jokių defektų.", "Elektronika", "Siuntimu", "Grafinę planšetę ar garso kolonėlę"),
    new(3, "C# Programavimo vadovėlis", "Naudinga knyga .NET kūrėjams.", "Knygos", "Gyvai arba siuntimu", "Kitas IT knygas / stalo žaidimus"),
    new(4, "Belaidės ausinės", "Geras garsas, baterija laiko puikiai.", "Elektronika", "Gyvai", "Klaviatūrą su apšvietimu")
};

app.MapGet("/api/status", () => new { Message = "Sveiki! Mainų platformos backend serveris veikia sėkmingai." });

app.MapGet("/api/items", () => items);

app.MapGet("/api/items/{id:int}", (int id) =>
{
    var item = items.FirstOrDefault(i => i.Id == id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
});

app.MapPost("/api/items", (CreateItemRequest request) =>
{
    var newId = items.Any() ? items.Max(i => i.Id) + 1 : 1;
    var newItem = new ItemDto(newId, request.Title, request.Description, request.Category, request.ExchangeType, request.LookingFor);
    items.Add(newItem);
    return Results.Created($"/api/items/{newId}", newItem);
});

app.MapDelete("/api/items/{id:int}", (int id) =>
{
    var item = items.FirstOrDefault(i => i.Id == id);
    if (item is null) return Results.NotFound();
    items.Remove(item);
    return Results.NoContent();
});

app.Run();

public record ItemDto(int Id, string Title, string Description, string Category, string ExchangeType, string LookingFor);
public record CreateItemRequest(string Title, string Description, string Category, string ExchangeType, string LookingFor);