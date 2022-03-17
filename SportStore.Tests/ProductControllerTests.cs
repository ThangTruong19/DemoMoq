using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Controllers.Products;
using SportStore.Models;
using SportStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        private Product[] products;
        public ProductControllerTests()
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
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable<Product>());

            ProductsController controller = new ProductsController(mock.Object);
            controller.pageSize = 2;

            ProductsListViewModel viewModel = controller.Index(null,2).ViewData.Model as ProductsListViewModel;
            Assert.Equal(2, viewModel.Products.Count());
            Assert.Equal("P3", viewModel.Products.ElementAt(0).Name);
            Assert.Equal("P4", viewModel.Products.ElementAt(1).Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable<Product>());

            ProductsController controller = new ProductsController(mock.Object);
            controller.pageSize = 3;

            ProductsListViewModel viewModel = controller.Index(null,2).ViewData.Model as ProductsListViewModel;
            Assert.Equal(7, viewModel.PagingInfo.TotalItem);
            Assert.Equal(3, viewModel.PagingInfo.ItemsPerPage);
            Assert.Equal(2, viewModel.PagingInfo.CurrentPage);
            Assert.Equal(3, viewModel.PagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable<Product>());

            ProductsController controller = new ProductsController(mock.Object);
            ProductsListViewModel viewModel = controller.Index("Soccer", 1).ViewData.Model as ProductsListViewModel;
            Assert.Equal(2, viewModel.Products.Count());
            Assert.True(viewModel.Products.ElementAt(0).Name == "P2" && viewModel.Products.ElementAt(0).Price == 100);
            Assert.True(viewModel.Products.ElementAt(1).Name == "P5" && viewModel.Products.ElementAt(1).Price == 250);
        }
        
        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable<Product>());

            ProductsController controller = new ProductsController(mock.Object);

            Func<ViewResult, ProductsListViewModel> GetModel = result =>
                result?.ViewData?.Model as ProductsListViewModel;

            int result1 = GetModel(controller.Index("Watersports")).PagingInfo.TotalItem;
            int result2 = GetModel(controller.Index("Soccer")).PagingInfo.TotalItem;
            int result3 = GetModel(controller.Index("Chess")).PagingInfo.TotalItem;

            Assert.Equal(3, result1);
            Assert.Equal(2, result2);
            Assert.Equal(2, result3);
        }
    }
}
