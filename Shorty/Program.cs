using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorty.Components;
using Shorty.Controllers;
using Shorty.Dal.Db;
using Shorty.Dal.DbModel.Repositories;
using Shorty.Dal.Entities;
using Shorty.Dal.Models;
using Shorty.Domain.Services.IServices;
using Shorty.Services;
using Shorty.Services.IServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "shorty", Version = "v1" });
});


builder.Services.AddScoped<IUserRepository<User>, UserRepository>();
builder.Services.AddScoped<IUserService<User>, UserService>();
builder.Services.AddScoped<IShortyUrlRepository<ShortyModel>, ShortyUrlRepository>();
builder.Services.AddScoped<IUrlService<ShortyModel>, UrlService>();
builder.Services.AddScoped<IShortCodeHistoryRepository<ShortyHistoryModel>, ShortCodeHistoryRepository>();
builder.Services.AddScoped<IShortCodeHistoryService, ShortCodeHistoryService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
    app.UseHsts();
}

app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseSwagger();
app.UseSwaggerUI();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();

