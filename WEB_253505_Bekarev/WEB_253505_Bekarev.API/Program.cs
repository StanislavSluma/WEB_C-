using Microsoft.EntityFrameworkCore;
using WEB_253505_Bekarev.API.Data;
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

app.UseAuthorization();

app.MapControllers();

app.Run();
