using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shorty.Components;
using Shorty.Dal;
using Shorty.Dal.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "shorty", Version = "v1" });
});

builder.Services.AddSingleton<TestRepository>();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddQuickGridEntityFrameworkAdapter();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
    //app.UseMigrationsEndPoint();
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


