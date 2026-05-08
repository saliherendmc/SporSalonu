using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Session için gerekli
using SporSalonu.Data;
using SporSalonu.Models;
using System.Linq;

namespace SporSalonu.Controllers
{
    public class AccountController : Controller
    {
        private readonly SporContext _context;

        public AccountController(SporContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [HttpPost]
        [HttpPost]
        public IActionResult Login(string email, string sifre)
        {
            // 1. Kullanıcıyı ve Rolünü buluyoruz
            var girisBilgisi = _context.Kullanicilar.FirstOrDefault(u => u.Email == email && u.Sifre == sifre);

            if (girisBilgisi != null)
            {
                // Session'a Rolü kaydediyoruz ki diğer sayfalarda kontrol edebilelim
                HttpContext.Session.SetString("UserRole", girisBilgisi.Rol);
                HttpContext.Session.SetString("UserEmail", girisBilgisi.Email);

                // 2. ROL KONTROLÜ BAŞLIYOR
                if (girisBilgisi.Rol == "Admin")
                {
                    // Eğer Admin ise Yönetim Paneline (Index) gönder
                    return RedirectToAction("Index", "Home");
                }
                else if (girisBilgisi.Rol == "Uye")
                {
                    // Eğer Üye ise önce Uyeler tablosunda var mı bak
                    var uye = _context.Uyeler.FirstOrDefault(x => x.Email == email);
                    if (uye != null)
                    {
                        HttpContext.Session.SetInt32("UyeId", uye.Id);
                        HttpContext.Session.SetString("UyeAdSoyad", uye.Ad + " " + uye.Soyad);
                        return RedirectToAction("Profilim", "Home");
                    }
                }
            }

            ViewBag.Hata = "E-posta, şifre hatalı veya yetkisiz erişim!";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            // Cache temizleme işlemini daha güvenli bir yöntemle yapalım
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Login", "Account");
        }
    }
}