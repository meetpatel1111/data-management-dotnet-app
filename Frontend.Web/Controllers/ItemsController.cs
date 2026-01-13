using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

public class ItemsController : Controller
{
    private readonly HttpClient _http;

    public ItemsController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
        _http.BaseAddress = new Uri("http://localhost:5000");
    }

    public async Task<IActionResult> Index()
    {
        var items = await _http.GetFromJsonAsync<List<Item>>("/api/items");
        return View(items);
    }
}

public record Item(int Id, string Name, string Description);
