using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;
using Microsoft.AspNetCore.Http;

namespace SporSalonu.Controllers
{
    public class UyelerController : Controller
    {
        private readonly SporContext _context;

        public UyelerController(SporContext context)
        {
            _context = context;
        }

        // --- GÜVENLİK KONTROLÜ ---
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // Üyeleri Listeleme
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var uyeler = await _context.Uyeler.Include(u => u.UyelikPaketi).ToListAsync();
            return View(uyeler);
        }

        // Yeni Üye Ekleme (GET)
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            ViewBag.Paketler = new SelectList(_context.UyelikPaketleri, "Id", "PaketAdi");
            return View();
        }

        // Yeni Üye Kaydetme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Uyeler uye)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                // Email kontrolü (Aynı emaille ikinci kayıt olmasın)
                if (_context.Kullanicilar.Any(k => k.Email == uye.Email))
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanımda.");
                }
                else
                {
                    uye.KayitTarihi = DateTime.Now;
                    _context.Add(uye);

                    var yeniKullanici = new Kullanicilar
                    {
                        Email = uye.Email,
                        Sifre = "123456", // Varsayılan şifre
                        Rol = "Uye"
                    };
                    _context.Kullanicilar.Add(yeniKullanici);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Paketler = new SelectList(_context.UyelikPaketleri, "Id", "PaketAdi", uye.UyelikPaketiId);
            return View(uye);
        }

        // Düzenleme Sayfası (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            if (id == null) return NotFound();

            var uye = await _context.Uyeler.FindAsync(id);
            if (uye == null) return NotFound();

            ViewBag.Paketler = new SelectList(_context.UyelikPaketleri, "Id", "PaketAdi", uye.UyelikPaketiId);
            return View(uye); // Burada hata alıyorsan Views/Uyeler/Edit.cshtml dosyasını oluşturmalısın.
        }

        // Düzenleme Kaydetme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Uyeler uye)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            if (id != uye.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Eski veriyi çekiyoruz (Email değişikliğini takip etmek için)
                    var eskiUye = await _context.Uyeler.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

                    if (eskiUye != null && eskiUye.Email != uye.Email)
                    {
                        // Email değiştiyse Kullanicilar tablosundaki hesabı da güncelle
                        var hesap = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Email == eskiUye.Email);
                        if (hesap != null)
                        {
                            hesap.Email = uye.Email;
                            _context.Update(hesap);
                        }
                    }

                    _context.Update(uye);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Uyeler.Any(e => e.Id == uye.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Paketler = new SelectList(_context.UyelikPaketleri, "Id", "PaketAdi", uye.UyelikPaketiId);
            return View(uye);
        }

        // Silme İşlemi
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var uye = await _context.Uyeler.FindAsync(id);
            if (uye != null)
            {
                var hesap = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Email == uye.Email);
                if (hesap != null) _context.Kullanicilar.Remove(hesap);

                _context.Uyeler.Remove(uye);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}