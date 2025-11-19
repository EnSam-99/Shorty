using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Shorty.Components;
using Shorty.Dal;
using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Db.Repositories;
using Shorty.Dal.Entities;
using Shorty.Domain.Services;
using Shorty.Domain.Services.Abstractions;
using Shorty.Middleware;
using Shorty.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "shorty", Version = "v1" });
});

builder.Services.AddScoped<IUserRepository<UserEntity>, UserRepository>();
builder.Services.AddScoped<IUserService<UserEntity>, UserService>();
builder.Services.AddScoped<IShortyUrlRepository<ShortyEntity>, ShortyUrlRepository>();
builder.Services.AddScoped<IUrlService<ShortyEntity>, UrlService>();
builder.Services.AddScoped<IShortCodeHistoryRepository<ShortyHistoryEntity>, ShortCodeHistoryRepository>();
builder.Services.AddScoped<IShortCodeHistoryService, ShortCodeHistoryService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();


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
app.UseMiddleware<GlobalExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseStaticFiles();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();


