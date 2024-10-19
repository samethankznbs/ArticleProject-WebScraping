using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebScraping.Models;
using WebScraping.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WebScrapingDatabaseSettings>(
                builder.Configuration.GetSection(nameof(WebScrapingDatabaseSettings)));

builder.Services.AddSingleton<IWebScrapingDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<WebScrapingDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("WebScrapingDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IArticleService, ArticleService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=arama_ekran}/{action=Index}/{id?}");

app.Run();