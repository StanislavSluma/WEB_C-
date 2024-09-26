using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using WEB_253505_Bekarev.Controllers;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;
using WEB_253505_Bekarev.Services.CategoryService;
using WEB_253505_Bekarev.Services.ProductService;
using Xunit.Abstractions;

namespace WEB_Bekarev_253505.Tests
{
    public class ProductControllerTest
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductControllerTest()
        {
            _productService = Substitute.For<IProductService>();
            _categoryService = Substitute.For<ICategoryService>();
        }

        [Theory]
        [InlineData(false, true )]
        [InlineData(true, false)]

        public void Index_GettingProductListFailed_ShouldReturn404(bool prod, bool cat)
        {
            // Arrange
            _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Anime>>()
            {
                Successfull = prod
            });

            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
            {
                Successfull = cat
            });

            var controllerContext = new ControllerContext();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Headers.Returns(new HeaderDictionary());
            controllerContext.HttpContext = httpContext;

            var controller = new ProductController(_categoryService, _productService)
            {
                ControllerContext = controllerContext
            };

            // var header = new Dictionary<string, StringValues>(){ ["x-requested-with"] = "XMLHttpRequest" };
            // Act
            var result = controller.Index(null).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        //[Fact]
        //public void Index_GettingCategoryListFailed_ShouldReturn404()
        //{
        //    // Arrange
        //    _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Anime>>()
        //    {
        //        Successfull = true
        //    });

        //    _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
        //    {
        //        Successfull = false
        //    });

        //    var controllerContext = new ControllerContext();
        //    var httpContext = Substitute.For<HttpContext>();
        //    httpContext.Request.Headers.Returns(new HeaderDictionary());
        //    controllerContext.HttpContext = httpContext;

        //    var controller = new ProductController(_categoryService, _productService)
        //    {
        //        ControllerContext = controllerContext
        //    };

        //    // Act
        //    var result = controller.Index(null).Result;

        //    // Assert
        //    Assert.IsType<NotFoundObjectResult>(result);
        //}

        [Fact]
        public void Index_ViewData_Should_Contain_CategoryList()
        {
            // Arrange
            _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Anime>>()
            {
                Successfull = true,
                Data = new ListModel<Anime>
                {
                    Items = new List<Anime>()
                    {
                        new Anime() { Id = 1, Name = "1", SeriesAmount=1, SeriesTime=1, TotalTime=1, CategoryId=1 },
                        new Anime() { Id = 2, Name = "2", SeriesAmount=2, SeriesTime=2, TotalTime=2, CategoryId=2 },
                        new Anime() { Id = 3, Name = "3", SeriesAmount=3, SeriesTime=3, TotalTime=3, CategoryId=3 },
                    }
                }
            });

            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
            {
                Successfull = true,
                Data = new List<Category>
                {
                    new Category {Id = 1, Name="1", NormalizedName="1"},
                    new Category {Id = 2, Name="2", NormalizedName="2"},
                    new Category {Id = 3, Name="3", NormalizedName="3"}
                }
            });

            _categoryService.FromNormalizedNameAsync(null).Returns(new ResponseData<string>()
            {
                Successfull = true,
                Data = "Все"
            });

            var controllerContext = new ControllerContext();
            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Headers.Returns(new HeaderDictionary());
            controllerContext.HttpContext = httpContext;

            var controller = new ProductController(_categoryService, _productService)
            {
                ControllerContext = controllerContext,
                TempData = new TempDataDictionary(controllerContext.HttpContext, tempDataProvider)
            };

            // Act
            var result = controller.Index(null).Result;

            // Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var categories = viewResult.ViewData["Categories"] as List<Category>;

            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
            Assert.Equal(new List<Category>
                {
                    new Category {Id = 1, Name="1", NormalizedName="1"},
                    new Category {Id = 2, Name="2", NormalizedName="2"},
                    new Category {Id = 3, Name="3", NormalizedName="3"}
                },
                categories,
                new CategoryComparer());
        }

        [Fact]
        public void Index_ViewData_Should_Contain_CorrectCurrentCategory()
        {
            // Arrange
            _productService.GetProductListAsync("name-1").Returns(new ResponseData<ListModel<Anime>>()
            {
                Successfull = true,
                Data = new ListModel<Anime>
                {
                    Items = new List<Anime>() {
                        new Anime() { Id = 1, Name = "1", SeriesAmount=1, SeriesTime=1, TotalTime=1, CategoryId= 1 },
                        new Anime() { Id = 2, Name = "2", SeriesAmount=2, SeriesTime=2, TotalTime=2, CategoryId= 2 },
                        new Anime() { Id = 3, Name = "3", SeriesAmount=3, SeriesTime=3, TotalTime=3, CategoryId= 3 }
                    }
                }
            });

            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
            {
                Successfull = true,
                Data = new List<Category>
                {
                    new Category {Id = 1, Name="name1", NormalizedName="name-1"},
                    new Category {Id = 2, Name="name2", NormalizedName="name-2"},
                    new Category {Id = 3, Name="name3", NormalizedName="name-3"}
                }
            });

            _categoryService.FromNormalizedNameAsync("name-1").Returns(new ResponseData<string>()
            {
                Successfull = true,
                Data = "name1"
            });

            var controllerContext = new ControllerContext();
            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Headers.Returns(new HeaderDictionary());
            controllerContext.HttpContext = httpContext;

            var controller = new ProductController(_categoryService, _productService)
            {
                ControllerContext = controllerContext,
                TempData = new TempDataDictionary(controllerContext.HttpContext, tempDataProvider)
            };

            // Act
            var result = controller.Index("name-1").Result;

            // Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var currentCategory = viewResult.ViewData["current_category"].ToString();

            Assert.NotNull(currentCategory);
            Assert.NotEmpty(currentCategory);
            Assert.Equal("name1", currentCategory);
        }

        [Fact]
        public void Index_View_Should_Contain_ProductList()
        {
            // Arrange
            _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Anime>>()
            {
                Successfull = true,
                Data = new ListModel<Anime>
                {
                    Items = new List<Anime>() {
                        new Anime() { Id = 1, Name = "1", SeriesAmount=1, SeriesTime=1, TotalTime=1, CategoryId= 1 },
                        new Anime() { Id = 2, Name = "2", SeriesAmount=2, SeriesTime=2, TotalTime=2, CategoryId= 2 },
                        new Anime() { Id = 3, Name = "3", SeriesAmount=3, SeriesTime=3, TotalTime=3, CategoryId= 3 }
                    }   
                }
            });

            _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
            {
                Successfull = true,
                Data = new List<Category>
                {
                    new Category {Id = 1, Name="name1", NormalizedName="name-1"},
                    new Category {Id = 2, Name="name2", NormalizedName="name-2"},
                    new Category {Id = 3, Name="name3", NormalizedName="name-3"}
                }
            });

            _categoryService.FromNormalizedNameAsync(null).Returns(new ResponseData<string>()
            {
                Successfull = true,
                Data = "Все"
            });

            var controllerContext = new ControllerContext();
            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Headers.Returns(new HeaderDictionary());
            controllerContext.HttpContext = httpContext;

            var controller = new ProductController(_categoryService, _productService)
            {
                ControllerContext = controllerContext,
                TempData = new TempDataDictionary(controllerContext.HttpContext, tempDataProvider)
            };

            // Act
            var result = controller.Index(null).Result;

            // Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var productsList = Assert.IsType<ListModel<Anime>>(viewResult.Model);

            Assert.NotNull(productsList);
            Assert.NotEmpty(productsList.Items);
            Assert.Equal(new List<Anime>() {
                        new Anime() { Id = 1, Name = "1", SeriesAmount=1, SeriesTime=1, TotalTime=1, CategoryId= 1 },
                        new Anime() { Id = 2, Name = "2", SeriesAmount=2, SeriesTime=2, TotalTime=2, CategoryId= 2 },
                        new Anime() { Id = 3, Name = "3", SeriesAmount=3, SeriesTime=3, TotalTime=3, CategoryId= 3 }
                    }, productsList.Items, new ProductComparer());
        }

    }

    public class CategoryComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category? x, Category? y)
        {
            if (ReferenceEquals(y, null) || ReferenceEquals(x, null))
                return false;
            
            return x.Id == y.Id && x.Name == y.Name && x.NormalizedName == y.NormalizedName;
        }

        public int GetHashCode([DisallowNull] Category obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class ProductComparer : IEqualityComparer<Anime>
    {
        public bool Equals(Anime? x, Anime? y)
        {
            if (ReferenceEquals(y, null) || ReferenceEquals(x, null))
                return false;

            return x.Id == y.Id && x.Name == y.Name && x.CategoryId == y.CategoryId && x.SeriesTime == y.SeriesTime && x.SeriesAmount == y.SeriesAmount;
        }

        public int GetHashCode([DisallowNull] Anime obj)
        {
            return obj.Id.GetHashCode() ^ obj.SeriesAmount.GetHashCode() ^ obj.SeriesTime.GetHashCode();
        }
    }
}
