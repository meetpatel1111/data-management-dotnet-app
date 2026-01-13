using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
        return View(items ?? new List<Item>());
    }
}

public record Item(int Id, string Name, string Description);
