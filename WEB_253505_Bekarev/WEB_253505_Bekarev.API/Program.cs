using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WEB_253505_Bekarev.API.Data;
using WEB_253505_Bekarev.API.Models;
using WEB_253505_Bekarev.API.Services.AnimeService;
using WEB_253505_Bekarev.API.Services.CategoryService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("AnimeDb")));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAnimeService, AnimeService>();



var authServer = builder.Configuration
                            .GetSection("AuthServer")
                            .Get<AuthServerData>();
// Добавить сервис аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    o =>
                    {
                        // Адрес метаданных конфигурации OpenID
                        o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
                        // Authority сервера аутентификации
                        o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
                        // Audience для токена JWT
                        o.Audience = "account";
                        // Запретить HTTPS для использования локальной версии Keycloak
                        // В рабочем проекте должно быть true
                        o.RequireHttpsMetadata = false;
                    });

    
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//await DbInitializer.SeedData(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
