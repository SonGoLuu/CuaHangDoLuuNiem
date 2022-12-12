using DICHOSAIGON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DICHOSAIGON.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly SaiGonDiChoContext _context;
        public SearchController(SaiGonDiChoContext context)
        {
            _context = context;
        }
        // GET: Search/FindProduct
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            ls = _context.Products
                  .AsNoTracking()
                  .Include(a => a.Cat)
                  .Where(x => x.ProductName.Contains(keyword))
                  .OrderByDescending(x => x.ProductName)
                  .Take(10)
                  .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }
    }
}
