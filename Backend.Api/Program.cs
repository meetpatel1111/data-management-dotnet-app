using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DataStore>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/items", (DataStore store) => store.GetAll());
app.MapGet("/api/items/{id:int}", (int id, DataStore store) =>
    store.Get(id) is Item item ? Results.Ok(item) : Results.NotFound());

app.MapPost("/api/items", (Item item, DataStore store) =>
{
    store.Add(item);
    return Results.Created($"/api/items/{item.Id}", item);
});

app.MapPut("/api/items/{id:int}", (int id, Item updated, DataStore store) =>
{
    return store.Update(id, updated) ? Results.Ok(updated) : Results.NotFound();
});

app.MapDelete("/api/items/{id:int}", (int id, DataStore store) =>
{
    return store.Delete(id) ? Results.NoContent() : Results.NotFound();
});

app.Run();

record Item(int Id, string Name, string Description);

class DataStore
{
    private readonly List<Item> _items = new();
    private int _id = 1;

    public IEnumerable<Item> GetAll() => _items;

    public Item? Get(int id) => _items.FirstOrDefault(x => x.Id == id);

    public void Add(Item item)
    {
        _items.Add(item with { Id = _id++ });
    }

    public bool Update(int id, Item updated)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index == -1) return false;
        _items[index] = updated with { Id = id };
        return true;
    }

    public bool Delete(int id)
    {
        var item = Get(id);
        if (item == null) return false;
        _items.Remove(item);
        return true;
    }
}
