using AspNetCoreHero.ToastNotification.Abstractions;
using DICHOSAIGON.Areas.Admin.Models;
using DICHOSAIGON.Extension;
using DICHOSAIGON.Helpper;
using DICHOSAIGON.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DICHOSAIGON.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly SaiGonDiChoContext _context;
        public INotyfService _notifyService { get; }
        public AccountsController(SaiGonDiChoContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("dang-xuat-admin.html", Name = "DangXuatAdmin")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("Email");
            return Redirect("dang-nhap-admin.html");
        }
        [Route("dang-nhap-admin.html", Name = "DangNhapAdmin")]
        public IActionResult Login()
        {
            var taikhoanID = HttpContext.Session.GetString("Email");
            if (taikhoanID != null)
            {

                return Redirect("/Admin/Home");

            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap-admin.html", Name = "DangNhapAdmin")]

        public async Task<IActionResult> Login(LoginViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(user.UserName);
                    if (!isEmail) return View(user);
                    var taikhoan = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == user.UserName);
                    if (taikhoan == null)
                    {
                        _notifyService.Success("Tài khoản không tồn tại");
                        return View(user);
                    }
                    string pass = (user.Password + taikhoan.Salt.Trim()).ToMD5();
                    if (taikhoan.Password != pass)
                    {
                        _notifyService.Success("Thông tin đăng nhập chưa chính xác");
                        return View(user);
                    }
                    // kiểm tra xem account có bị disable hay không
                    if (taikhoan.Active == false)
                    {
                        _notifyService.Success("Tài khoản bị vô hiệu hóa");
                        return View(user);
                    }

                    // Luu Session Makh
                    HttpContext.Session.SetString("Email", taikhoan.Email.ToString());
                    var email = HttpContext.Session.GetString("Email");

                    // Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, taikhoan.FullName),
                        new Claim("Email", taikhoan.Email.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    if (taikhoan.LastLogin == null) return RedirectToAction("ChangePassword", "AdminAccounts", new { Area = "Admin" });
                    else
                    {
                        taikhoan.LastLogin = DateTime.Now;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notifyService.Success("Đăng nhập thành công");
                        return Redirect("/Admin/Home");
                    }
                }
            }
            catch
            {
                return View(user);
            }
            return View(user);
        }
    }
}
