using DICHOSAIGON.Models;
using DICHOSAIGON.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DICHOSAIGON.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public int checklog = 0;
        private readonly SaiGonDiChoContext _context;

        public HomeController(ILogger<HomeController> logger, SaiGonDiChoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewVM model = new HomeViewVM();
            var lsProducts = _context.Products.AsNoTracking()
                        .Where(x => x.Active == true && x.HomeFlag==true)
                        .OrderBy(x => x.ProductName)
                        .ToList();

            List<ProductHomeVM> lsProductViews = new List<ProductHomeVM>();
            var lsCats = _context.Categories
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderBy(x => x.CatId)
                .ToList();
            foreach (var item in lsCats)
            {
                ProductHomeVM productHome = new ProductHomeVM();
                productHome.category = item;
                productHome.lsProducts = lsProducts.Where(x => x.CatId == item.CatId).ToList();
                lsProductViews.Add(productHome);
            }
            var TinTuc = _context.TinDangs
                .AsNoTracking()
                .Where(x => x.Published == true && x.IsNewfeed ==true)
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .ToList();
            model.Products = lsProductViews;
            model.TinTucs = TinTuc;
            ViewBag.AllProducts = lsProducts;
            return View(model);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Blog()
        {
            var tintuc = _context.TinDangs.Where(x => x.Published == true && x.IsNewfeed == true).OrderByDescending(x=>x.CreatedDate).ToList();
            ViewBag.Tintuc = tintuc;
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetCat(int CatId)
        {
            var cat = _context.Categories.AsNoTracking().Where(x=>x.CatId == CatId).FirstOrDefault();
            string alias = cat.Alias;
             return Json(new { status = alias });
        }
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
    }
}
