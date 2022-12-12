using AspNetCoreHero.ToastNotification.Abstractions;
using DICHOSAIGON.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DICHOSAIGON.Controllers
{
    public class LocationController : Controller
    {
        private readonly SaiGonDiChoContext _context;
        public INotyfService _notifyService { get; }
        public LocationController(SaiGonDiChoContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult QuanHuyenList(int LocationId)
        {
            var QuanHuyens = _context.Locations.OrderBy(x => x.LocationId)
                .Where(x => x.Parent == LocationId && x.Levels == 1)
                .OrderBy(x => x.NameWithType)
                .ToList();
            return Json(QuanHuyens);
        }
        public ActionResult PhuongXaList(int IdHuyen)
        {
            var PhuongXas = _context.Locations.OrderBy(x => x.LocationId)
                  .Where(x => x.ThuocHuyen == IdHuyen && x.Levels == 2)
                  .OrderBy(x => x.NameWithType)
                  .ToList();
            return Json(PhuongXas);
        }
    }
}
