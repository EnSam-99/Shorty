using Microsoft.OpenApi.Models;
using Shorty.Components;
using Shorty.Dal;
using Shorty.Dal.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "shorty", Version = "v1" });
});

builder.Services.AddSingleton<TestRepository>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseSwagger();
app.UseSwaggerUI();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/api/shorty/create", async (CreateRequestModel request, AppDbContext db) =>
{
    
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
    if (user == null)
    {
        user = new User { Email = request.Email };
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    
    string shortCode;
    do
    {
        shortCode = Guid.NewGuid().ToString().Substring(0, 6);
    } while (await db.Shorties.AnyAsync(s => s.ShortUrl == shortCode));

    var shorty = new Shorty.Dal.Models.Shorty()
    {
        Url = request.Url,
        ShortUrl = shortCode,
        CreatedAt = DateTime.UtcNow,
        UserId = user.Id
    };

    db.Shorties.Add(shorty);
    await db.SaveChangesAsync();

    return Results.Ok(shorty);
});

app.MapControllers();

app.Run();

public class CreateRequestModel
{
    public string Email { get; set; }
    public string Url { get; set; }
}