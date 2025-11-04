using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorty.Components;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Services;
using Shorty.Dal.Repository.Interface;
using Shorty.Dal.Repository.Implementation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<AppDbContext>();

// builder.Services.AddHttpClient("ServerAPI", client =>
// {
//     client.BaseAddress = new Uri("http://localhost:5211/");
// });
builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:5221/"); // ← HTTPS + ճիշտ պորտ
});
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));
builder.Services.AddScoped<IShortyUrlService, ShortyUrlService>();
builder.Services.AddScoped<IDeletionRepository, DeletionRepository>();
builder.Services.AddScoped<IDeletionUrlService, DeletionUrlService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shorty", Version = "v1" });
});


builder.Services.AddSingleton<TestRepository>();
builder.Services.AddControllers();
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();



// Configure the HTTP request pipeline
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

app.UseSwagger();
app.UseSwaggerUI();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();