using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportStore.Models;
using SportStore.Models.ViewModels;

namespace SportStore.Controllers.Products
{
    public class ProductsController : Controller
    {
        private IProductRepository repo;
        public int pageSize = 4;
        
        public ProductsController(IProductRepository repository)
        {
            this.repo = repository;
        }

        public ViewResult Index(string category, int productPage = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = repo.Products
                                    .Where(p => category == null || p.Category == category)
                                    .OrderBy(p => p.Id)
                                    .Skip((productPage - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = pageSize,
                    TotalItem = repo.Products
                                    .Where(p => category == null || p.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}
