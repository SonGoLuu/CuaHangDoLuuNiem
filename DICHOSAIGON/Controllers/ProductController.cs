using AspNetCoreHero.ToastNotification.Abstractions;
using DICHOSAIGON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Collections.Generic;
using System.Linq;

namespace DICHOSAIGON.Controllers
{
    public class ProductController : Controller
    {
        private readonly SaiGonDiChoContext _context;
        public ProductController(SaiGonDiChoContext context)
        {
            _context = context;
        }
        [Route("shop.html", Name = "ShopProduct")]
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 5;
                var lsProduct = _context.Products.AsNoTracking()
                    .OrderBy(x => x.ProductName);

                PagedList<Product> models = new PagedList<Product>(lsProduct, pageNumber, pageSize);
                var lsRate = _context.Products.AsNoTracking()
                    .Where(x => x.BestSellers == true && x.Active == true)
                    .OrderBy(x => x.ProductName)
                    .Take(3).ToList();
                ViewBag.CurrentPage = pageNumber;
                ViewBag.lsRate = lsRate;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult Search(int page = 1, string tags = "")
        {
            try
            {
                var pageNumber = page;
                var pageSize = 5;
                List<Product> lsBooks = new List<Product>();
                lsBooks = _context.Products.AsNoTracking()
                    .Where(x => x.Tags.Contains(tags) || x.ProductName.Contains(tags))
                .Include(x => x.Cat)
                .OrderBy(x => x.ProductName).ToList();
                ViewBag.CurrentCateID = tags;
                PagedList<Product> models = new PagedList<Product>(lsBooks.AsQueryable(), pageNumber, pageSize);

                var lsRate = _context.Products.AsNoTracking()
                    .Where(x => x.BestSellers == true && x.Active == true)
                    .OrderBy(x => x.ProductName)
                    .Take(3).ToList();
                ViewBag.CurrentPage = pageNumber;
                ViewBag.lsRate = lsRate;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [Route("/{Alias}", Name = "ProductList")]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                var pageSize = 5;
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
                var lsProduct = _context.Products.AsNoTracking()
                    .Where(x => x.CatId == danhmuc.CatId)
                    .OrderBy(x => x.ProductName);

                PagedList<Product> models = new PagedList<Product>(lsProduct, page, pageSize);
                var lsRate = _context.Products.AsNoTracking()
                    .Where(x => x.BestSellers == true && x.Active == true)
                   .OrderBy(x => x.ProductName)
                    .Take(3).ToList();
                ViewBag.CurrentPage = page;
                ViewBag.lsRate = lsRate;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
        {
            var products = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
            if (products == null)
            {
                return RedirectToAction("Index");
            }
            var lsProduct = _context.Products.AsNoTracking()
                .Where(x => x.CatId == products.CatId && x.ProductId != id && x.Active == true)
               .OrderBy(x => x.ProductName)
                .Take(4).ToList();
            ViewBag.SanPham = lsProduct;
            return View(products); ;
        }
    }
}
