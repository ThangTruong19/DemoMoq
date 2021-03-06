using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IProductRepository repository;

        public NavigationMenuViewComponent(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProduct = RouteData?.Values["category"]??"";
            return View(repository.Products.Select(product => product.Category).Distinct().OrderBy(c => c));
        }
    }
}
