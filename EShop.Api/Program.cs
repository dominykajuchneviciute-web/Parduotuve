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

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowAll");

var items = new List<ItemDto>
{
    new(1, "Miesto dviratis", "Tvarkingas dviratis, paruoštas sezonui.", "Naudotas", "L", "Kross", "Juoda"),
    new(2, "Dell 24\" monitorius", "Full HD monitorius be defektų.", "Naudotas", "24 coliai", "Dell", "Juoda"),
    new(3, "C# Programavimo vadovėlis", "Naudinga knyga .NET programuotojams.", "Naujas", "A5", "Alma littera", "Mėlyna")
};

app.MapGet("/api/status", () => new { Message = "Sveiki! Backend serveris veikia sėkmingai." });

app.MapGet("/api/items", () => items);

app.MapGet("/api/items/{id:int}", (int id) =>
{
    var item = items.FirstOrDefault(i => i.Id == id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
});

app.MapPost("/api/items", (CreateItemRequest request) =>
{
    var newId = items.Any() ? items.Max(i => i.Id) + 1 : 1;
    var newItem = new ItemDto(newId, request.Name, request.Description, request.Condition, request.Size, request.Manufacturer, request.Color);
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
app.UseHsts();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public record ItemDto(int Id, string Name, string Description, string? Condition, string? Size, string? Manufacturer, string? Color);
public record CreateItemRequest(string Name, string Description, string? Condition, string? Size, string? Manufacturer, string? Color);