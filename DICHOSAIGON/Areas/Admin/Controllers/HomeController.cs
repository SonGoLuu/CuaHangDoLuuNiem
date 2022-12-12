using DICHOSAIGON.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace DICHOSAIGON.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly SaiGonDiChoContext _context;

        public HomeController(SaiGonDiChoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("Email");
            var tk =_context.Accounts.Where(a => a.Email.Contains(email)).FirstOrDefault();
            //lấy tổng doanh thu
            var order = _context.Orders.ToList();
            double tt = 0;
            foreach (var item in order)
            {
                tt += Convert.ToDouble(item.TotalMoney);
            }   
            ViewBag.TongTien = tt;
            //lấy phần trăm tăng doanh thu
            int month = DateTime.Now.Month;
            int monthtruoc;
            if (month == 1) monthtruoc = 12;
            else monthtruoc = month - 1;
            var thangtruoc = _context.Orders.Where(a => a.OrderDate.Value.Month == monthtruoc).ToList();
            double tttruoc = 0;
            foreach (var item in thangtruoc)
            {
                tttruoc += item.TotalMoney;
            }
            var thangnay = _context.Orders.Where(a => a.OrderDate.Value.Month == month).ToList();
            double ttnay = 0;
            foreach (var item in thangnay)
            {
                ttnay += item.TotalMoney;
            }
            double phantram = (ttnay - tttruoc) / tttruoc * 100;
            if (phantram > 0) ViewBag.PhanTram = "+" + String.Format("{0:0.##}", phantram);
            else ViewBag.PhanTram = String.Format("{0:0.##}", phantram);


            ////doanh thu theo ngày
            //var orderngay = _context.Orders.Where(x=>x.OrderDate.Value.Date==DateTime.Now.Date && x.OrderDate.Value.Month==DateTime.Now.Month && x.OrderDate.Value.Year==DateTime.Now.Year).ToList();
            //double ttngay = 0;
            //foreach (var item in orderngay)
            //{
            //    ttngay += Convert.ToDouble(item.TotalMoney);
            //}
            //ViewBag.TongTienNgay = ttngay;

            //lấy tổng đơn hàng
            int donhang = 0;
            foreach (var item in order)
            {
                donhang += 1;
            }
            ViewBag.DonHang = donhang;
            //lấy tổng khách hàng
            int khachhang = 0;
            var kh = _context.Customers.ToList();
            foreach (var item in kh)
            {
                khachhang += 1;
            }
            ViewBag.KhachHang = khachhang;
            //Phân quyền
            int phanquyen = Convert.ToInt32(tk.RoleId);
            if(phanquyen == 1)
            {
                var topProducts = _context.OrderDetails.Include(x => x.Product).ToList();
                ViewBag.TopProducts = topProducts;
                return View("Index");
            }    
            else return Redirect("/Admin/Home/No");
        }
        public IActionResult No()
        {
            return View();
        }
        public IActionResult Data()
        {
            var result = _context.OrderDetails
                .GroupBy(x => new { group = x.Product.Cat.CatName })
                .Select(group => new
                {
                    CatName = group.Key.group,
                    count = group.Count()
                }
                )
                .ToList();
            //return Json(result);
            var labels = result.Select(x => x.CatName).ToArray();
            var values = result.Select(x => x.count).ToArray();
            var max = values.Max();
            List<object> list1 = new List<object>();
            list1.Add(labels);
            list1.Add(values);
            list1.Add(max);
            return Json(list1);
        }
        public IActionResult Data2()
        {
            var result = _context.Orders
                .GroupBy(x => new { group = x.OrderDate.Value.Date } )
                .Select(group => new
                {
                    OrderDate = group.Key.group,
                    count = group.Count()
                }
                )
                .OrderByDescending(o => o.OrderDate).Take(7).OrderBy(o => o.OrderDate).ToList();
            //return Json(result);
            var labels = result.Select(x => "Ngày " + x.OrderDate.Day.ToString()).ToArray();
            var values = result.Select(x => x.count).ToArray();
            var max = values.Max();
            List<object> list1 = new List<object>();
            list1.Add(labels);
            list1.Add(values);
            list1.Add(max);
            return Json(list1);
        }
        public IActionResult Data3()
        {
            var result = _context.Orders
                .GroupBy(x => new { x.OrderDate.Value.Date })
                .Select(x => new
                {
                    OrderDate = x.Key.Date,
                    Money = x.Select(x=>x.TotalMoney).Sum()
                }
                )
                .OrderByDescending(o => o.OrderDate).Take(7).OrderBy(o => o.OrderDate).ToList();
            //return Json(result);
            var labels = result.Select(x => "Ngày " + x.OrderDate.Day.ToString()).ToArray();
            var values = result.Select(x => x.Money).ToArray();
            var max = result.Select(x => x.Money).ToArray().Max();
            List<object> list1 = new List<object>();
            list1.Add(labels);
            list1.Add(values);
            list1.Add(max);
            return Json(list1);
        }
    }
}
