using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportStore.Components;
using SportStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTest
    {
        private Product[] products;

        public NavigationMenuViewComponentTest()
        {
            products = new Product[]
            {
                new Product{Name="P1", Price=50, Category="Watersports", Id=1},
                new Product{Name="P2", Price=100, Category="Soccer", Id=2},
                new Product{Name="P3", Price=150, Category="Chess", Id=3},
                new Product{Name="P4", Price=200, Category="Watersports", Id=4},
                new Product{Name="P5", Price=250, Category="Soccer", Id=5},
                new Product{Name="P6", Price=300, Category="Watersports", Id=6},
                new Product{Name="P7", Price=350, Category="Chess", Id=7},
            };
        }

        [Fact]
        public void Can_Select_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable<Product>());

            NavigationMenuViewComponent viewComponent = new NavigationMenuViewComponent(mock.Object);
            IEnumerable<string> categories = (viewComponent.Invoke() as ViewViewComponentResult).ViewData.Model as IEnumerable<string>;
            Assert.True(Enumerable.SequenceEqual(new string[] { "Chess", "Soccer", "Watersports" }, categories.ToArray()));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            // Arrange
            string categoryToSelect = "Chess";

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable<Product>());

            NavigationMenuViewComponent viewComponent = new NavigationMenuViewComponent(mock.Object);
            viewComponent.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };

            viewComponent.RouteData.Values["category"] = categoryToSelect;

            // Action
            string category = (viewComponent.Invoke() as ViewViewComponentResult).ViewData["SelectedProduct"] as string;
            Assert.Equal("Chess", category);
        }
    }
}
