using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorty.Components;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// ==================== DATABASE CONFIG ====================
var cs = builder.Configuration.GetConnectionString("Postgres")
	?? throw new InvalidOperationException("Connection string 'Postgres' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(cs));

// ==================== RAZOR COMPONENTS ====================
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// ==================== HTTP CLIENT (for Blazor pages) ====================
builder.Services.AddHttpClient("ServerAPI", client =>
{
	client.BaseAddress = new Uri("http://localhost:5211/");
});

// ==================== DEPENDENCY INJECTION ====================
builder.Services.AddScoped<IShortyUrlService, ShortyUrlService>();
builder.Services.AddControllers();

// ==================== SWAGGER ====================
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Shorty API",
		Version = "v1"
	});
});

var app = builder.Build();

// ==================== MIDDLEWARE PIPELINE ====================
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shorty API v1");
		options.RoutePrefix = "swagger";
	});
}

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

// ==================== ROUTES ====================
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
