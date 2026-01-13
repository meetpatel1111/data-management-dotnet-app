using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
