using AspNetCoreHero.ToastNotification.Abstractions;
using DICHOSAIGON.Extension;
using DICHOSAIGON.Helpper;
using DICHOSAIGON.Models;
using DICHOSAIGON.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DICHOSAIGON.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly SaiGonDiChoContext _context;
        public INotyfService _notifyService { get; }
        public CheckoutController(SaiGonDiChoContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        // GET:Checkout/Index
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            // lấy giỏ hàng ra đề xử lý
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
                if(khachhang.LocationId.HasValue) model.TinhThanh = khachhang.LocationId.Value;
                if (khachhang.District.HasValue) model.QuanHuyen = khachhang.District.Value;
                if (khachhang.Ward.HasValue) model.PhuongXa = khachhang.Ward.Value;
            }
            ViewData["TinhThanh"] = new SelectList(_context.Locations.AsNoTracking().Where(x => x.Levels == 0).OrderBy(x => x.Name).ToList(), "LocationId", "Name");
            ViewData["QuanHuyen"] = new SelectList(_context.Locations.AsNoTracking().Where(x => x.Levels == 1 && x.Idhuyen==model.QuanHuyen).OrderBy(x => x.Name).ToList(), "Idhuyen", "Name");
            ViewData["PhuongXa"] = new SelectList(_context.Locations.AsNoTracking().Where(x => x.Levels == 2 && x.LocationId == model.PhuongXa).OrderBy(x => x.Name).ToList(), "LocationId", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }

        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(MuaHangVM muaHang)
        {
            // lấy giỏ hàng ra đề xử lý
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;

                khachhang.LocationId = muaHang.TinhThanh;
                khachhang.District = muaHang.QuanHuyen;
                khachhang.Ward = muaHang.PhuongXa;
                khachhang.Address = muaHang.Address;
                _context.Update(khachhang);
                _context.SaveChanges();
            }

            try
            {
                if(ModelState.IsValid)
                {
                    // Khởi tạo đơn hàng
                    Order donhang = new Order();
                    donhang.CustomerId = model.CustomerId;
                    donhang.Address = model.Address;
                    donhang.LocationId = muaHang.TinhThanh;
                    donhang.District = muaHang.QuanHuyen;
                    donhang.Ward = muaHang.PhuongXa;
                    donhang.OrderDate = DateTime.Now;
                    donhang.TransactStatusId = 1;// Đơn hàng mới
                    donhang.CustomerId = model.CustomerId;
                    donhang.Deleted = false;
                    donhang.Paid = false;
                    donhang.Note = Utilities.StripHTML(model.Note);
                    donhang.TotalMoney = Convert.ToInt32(cart.Sum(x => x.TotalMoney));
                    _context.Add(donhang);
                    _context.SaveChanges();

                    // Tạo danh sách đơn hàng
                    foreach (var item in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = donhang.OrderId;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.Amount = item.amount;
                        orderDetail.TotalMoney = item.amount * item.product.Price;
                        orderDetail.Price = item.product.Price;
                        orderDetail.CreateDate = DateTime.Now;
                        _context.Add(orderDetail);
                        var sanpham = _context.Products.Where(p=>p.ProductId== item.product.ProductId).FirstOrDefault();
                        sanpham.UnitsInStock = sanpham.UnitsInStock - item.amount;
                        _context.Update(sanpham);
                    }
                    _context.SaveChanges();
                    // Clear giỏ hàng
                    HttpContext.Session.Remove("GioHang");
                    // Xuất thông báo
                    _notifyService.Success("Đơn hàng đặt thành công");
                    // Cập nhật thông tin khách hàng
                    return RedirectToAction("Success");
                }
            }
            catch(Exception ex)
            {
                ViewData["TinhThanh"] = new SelectList(_context.Locations.AsNoTracking().Where(x => x.Levels == 0).OrderBy(x => x.Name).ToList(), "LocationId", "Name");
                ViewBag.GioHang = cart;
                return View(model);
            }
            ViewData["TinhThanh"] = new SelectList(_context.Locations.AsNoTracking().Where(x => x.Levels == 0).OrderBy(x => x.Name).ToList(), "LocationId", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }

        [Route("dat-hang-thanh-cong.html", Name="Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                var donhang = _context.Orders.Where(x => x.CustomerId == Convert.ToInt32(taikhoanID)).OrderByDescending(x => x.OrderId)
                    .FirstOrDefault();
                MuaHangSuccessVM successVM = new MuaHangSuccessVM();
                successVM.FullName = khachhang.FullName;
                successVM.DonHangID = donhang.OrderId;
                successVM.Phone = khachhang.Phone;
                successVM.Address = khachhang.Address;
                successVM.PhuongXa = GetNameLocation(donhang.Ward.Value, 2);
                successVM.QuanHuyen = GetNameLocation(donhang.District.Value, 1);
                successVM.TinhThanh = GetNameLocation(donhang.LocationId.Value, 0);
                
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }
        public string GetNameLocation(int idlocation, int level)
        {
            try
            {
                if(level == 0)
                {
                    var location = _context.Locations.Where(x=>x.LocationId==idlocation).FirstOrDefault();
                    return location.Name;
                }
                if (level == 1)
                {
                    var location = _context.Locations.Where(x => x.Idhuyen == idlocation && x.Levels==1).FirstOrDefault();
                    return location.Name;
                }
                if (level == 2)
                {
                    var location = _context.Locations.Where(x => x.LocationId == idlocation && x.Levels == 2).FirstOrDefault();
                    return location.Name;
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
    }
}
