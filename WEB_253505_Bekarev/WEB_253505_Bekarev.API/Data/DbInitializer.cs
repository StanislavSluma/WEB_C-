using Microsoft.EntityFrameworkCore;
using WEB_253505_Bekarev.Domain.Entities;

namespace WEB_253505_Bekarev.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {

            string ImagePath = app.Configuration.GetSection("ImagePath").Value ?? "";

            List<Category> _categories = new List<Category>{
                new Category {Name="Фентези", NormalizedName="fantasy"},
                new Category {Name="Детектив", NormalizedName="detective"},
                new Category {Name="Приключения", NormalizedName="adventures"},
                new Category {Name="Комедия", NormalizedName="comedy"},
                new Category {Name="Повседневность", NormalizedName="everyday-life"},
                new Category {Name="Драма", NormalizedName="drama"}
            };

            List<Anime> _animes = new List<Anime>()
            {
                new Anime()
                {
                    Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image=$"{ImagePath}Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image=$"{ImagePath}Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image=$"{ImagePath}Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                },
                /*new Anime()
                {
                    Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image=$"{ImagePath}Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image=$"{ImagePath}Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image=$"{ImagePath}Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                },
                new Anime()
                {
                    Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image=$"{ImagePath}Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image=$"{ImagePath}Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image=$"{ImagePath}Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                },
                new Anime()
                {
                    Name="Форма Голоса", Description="Интересное, грустное",
                    SeriesAmount=1, SeriesTime=130, TotalTime=130, Image=$"{ImagePath}Images/2.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("drama"))
                },
                new Anime()
                {
                    Name="LinkClick", Description="Интересное, затягивающее",
                    SeriesAmount=12, SeriesTime=24, TotalTime=288, Image=$"{ImagePath}Images/1.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("detective"))
                },
                new Anime()
                {
                    Name="Патэма наоборот", Description="Интересное, неожиданность",
                    SeriesAmount=1, SeriesTime=100, TotalTime=100, Image=$"{ImagePath}Images/3.jpg",
                    Category=_categories.Find(x=>x.NormalizedName.Equals("fantasy"))
                }*/
            };

            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //dbContext.Database.EnsureCreated();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
               dbContext.Database.Migrate();
            }

            dbContext.Categories.AddRange(_categories);
            dbContext.Animes.AddRange(_animes);

            dbContext.SaveChanges();
        }
    }
}
