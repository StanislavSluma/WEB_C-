using Microsoft.EntityFrameworkCore;
using WEB_253505_Bekarev.API.Data;
using WEB_253505_Bekarev.API.Services.AnimeService;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;

namespace WEB_Bekarev_253505.Tests
{
    public class AnimeServiceTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext context;
        public AnimeServiceTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                        .UseInMemoryDatabase("AnimeServiceTest")
                                        .Options;
            context = new AppDbContext(_dbContextOptions);
        }

        [Fact]
        public void Handle_ValidRequest_ShouldReturnPaginatedListWith3ItemsAndCorrectlyCountTotalPages()
        {
            // Arrange
            CreateContext(ref context);
            var service = new AnimeService(context);

            // Act
            var result = service.GetProductListAsync(null).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Anime>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(5, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.Animes.First(), result.Data.Items[0]);
        }

        [Fact]
        public void Handle_ValidReuqest_ShouldCorrectlyChooseGivenPage()
        {
            // Arrange
            CreateContext(ref context);
            var service = new AnimeService(context);
            int pageNo = 2;

            // Act
            var result = service.GetProductListAsync(null, pageNo).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Anime>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(2, result.Data.CurrentPage);
        }

        [Fact]
        public void Handle_ValidRequest_ShouldCorrectlyFilterByCategory()
        {
            // Arrange
            CreateContext(ref context);
            var service = new AnimeService(context);
            string category = "name-1";

            // Act
            var result = service.GetProductListAsync(category).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Anime>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.Items.Count);
        }

        [Fact]
        public void Handle_SetPageSizeGreaterThanMaximum_ShouldNotAllowSet()
        {
            // Arrange
            CreateContext(ref context);
            var service = new AnimeService(context);
            int pageSize = 54;

            // Act
            var result = service.GetProductListAsync(null, pageSize: pageSize).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Anime>>>(result);
            Assert.True(result.Successfull);
            Assert.True((int)Math.Ceiling(result.Data.Items.Count/ (double)result.Data.TotalPages) != pageSize);
        }

        [Fact]
        public void Handle_PageNoGreaterThanMaximumRequest_ReturnsSuccesfullIsFalse()
        {
            // Arrange
            CreateContext(ref context);
            var service = new AnimeService(context);
            int pageNo = 54;

            // Act
            var result = service.GetProductListAsync(null, pageNo).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Anime>>>(result);
            Assert.False(result.Successfull);
        }


        private void CreateContext(ref AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
                
            context.Categories.AddRange(
                new Category { Name = "Name1", NormalizedName = "name-1" },
                new Category { Name = "Name2", NormalizedName = "name-2" },
                new Category { Name = "Name3", NormalizedName = "name-3" });


            context.Animes.AddRange(
                new Anime { Name = "Name1", CategoryId = 1, SeriesAmount = 1, SeriesTime = 1, TotalTime = 1, Description = "Name1" },
                new Anime { Name = "Name2", CategoryId = 2, SeriesAmount = 2, SeriesTime = 2, TotalTime = 4, Description = "Name2" },
                new Anime { Name = "Name2", CategoryId = 2, SeriesAmount = 2, SeriesTime = 2, TotalTime = 4, Description = "Name2" },
                new Anime { Name = "Name3", CategoryId = 3, SeriesAmount = 3, SeriesTime = 3, TotalTime = 9, Description = "Name3" },
                new Anime { Name = "Name3", CategoryId = 3, SeriesAmount = 3, SeriesTime = 3, TotalTime = 9, Description = "Name3" },
                new Anime { Name = "Name3", CategoryId = 3, SeriesAmount = 3, SeriesTime = 3, TotalTime = 9, Description = "Name3" }
            );

            context.SaveChanges();
        }
    }
}
