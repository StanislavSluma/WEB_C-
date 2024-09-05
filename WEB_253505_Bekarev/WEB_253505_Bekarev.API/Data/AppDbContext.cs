using Microsoft.EntityFrameworkCore;
using WEB_253505_Bekarev.Domain.Entities;

namespace WEB_253505_Bekarev.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Anime> Animes { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
    }
}
