using Microsoft.AspNetCore.Components; 
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorty.Components;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("Postgres")
		 ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(cs));

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shorty API", Version = "v1" });
});

builder.Services.AddScoped(sp =>
{
	var nav = sp.GetRequiredService<NavigationManager>();
	return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
});


builder.Services.AddScoped<IShortyUrlService, ShortyUrlService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shorty API v1");
		options.RoutePrefix = "swagger";
	});
}

app.UseRouting();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapControllers();

app.Run();
