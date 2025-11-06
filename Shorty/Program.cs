using Microsoft.AspNetCore.Components; 
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorty.Components;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Services;

using Shorty.Dal.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("Postgres")
		 ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(cs));

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shorty API", Version = "v1" });
});

builder.Services.AddSingleton<TestRepository>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped(sp =>
{
	var nav = sp.GetRequiredService<NavigationManager>();
	return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
});
var app = builder.Build();

app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();
builder.Services.AddScoped<IShortyUrlService, ShortyUrlService>();

app.UseSwagger();
app.UseSwaggerUI();
var app = builder.Build();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();



app.UseRouting();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapControllers();

app.Run();
