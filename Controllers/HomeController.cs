using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace SporSalonu.Controllers
{
    public class HomeController : Controller
    {
        private readonly SporContext _context;

        public HomeController(SporContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string? userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Admin")
            {
                if (userRole == "Uye") return RedirectToAction("Profilim");
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public IActionResult Profilim()
        {
            string? userRole = HttpContext.Session.GetString("UserRole");
            var uyeId = HttpContext.Session.GetInt32("UyeId");

            if (string.IsNullOrEmpty(userRole) || uyeId == null) return RedirectToAction("Login", "Account");
            if (userRole == "Admin") return RedirectToAction("Index");

            var uye = _context.Uyeler.Include(u => u.UyelikPaketi).FirstOrDefault(u => u.Id == uyeId);
            if (uye == null) return RedirectToAction("Login", "Account");

            // --- Antrenman Programý Kontrolü ---
            var mevcutProgram = _context.AntrenmanProgramlari.Where(x => x.UyeId == uyeId).ToList();
            if (mevcutProgram.Count < 7)
            {
                if (mevcutProgram.Any()) _context.AntrenmanProgramlari.RemoveRange(mevcutProgram);

                var detayliListe = new List<AntrenmanProgramlari>
                {
                    new AntrenmanProgramlari { UyeId = uyeId.Value, Gun = "Pazartesi", Bolge = "Göðüs - Ön Kol", HareketAdi = "Bench Press, Bicep Curl", Set = 4, Tekrar = 12, Aciklama = "60sn" },
                    new AntrenmanProgramlari { UyeId = uyeId.Value, Gun = "Salý", Bolge = "Sýrt - Arka Kol", HareketAdi = "Lat Pulldown, Pushdown", Set = 4, Tekrar = 12, Aciklama = "60sn" },
                    new AntrenmanProgramlari { UyeId = uyeId.Value, Gun = "Çarþamba", Bolge = "Omuz - Bacak", HareketAdi = "Shoulder Press, Squat", Set = 4, Tekrar = 10, Aciklama = "90sn" },
                    new AntrenmanProgramlari { UyeId = uyeId.Value, Gun = "Perþembe", Bolge = "Dinlenme", HareketAdi = "Dinlenme", Set = 0, Tekrar = 0, Aciklama = "-" },
                    new AntrenmanProgramlari { UyeId = uyeId.Value, Gun = "Cuma", Bolge = "Full Body", HareketAdi = "Deadlift, Pull-up", Set = 3, Tekrar = 8, Aciklama = "120sn" },
                    new AntrenmanProgramlari { UyeId = uyeId.Value, Gun = "Cumartesi", Bolge = "Kardiyo", HareketAdi = "Koþu Bandý (HIIT)", Set = 1, Tekrar = 30, Aciklama = "dk" },
                    new AntrenmanProgramlari { UyeId = uyeId.Value, Gun = "Pazar", Bolge = "Dinlenme", HareketAdi = "Dinlenme", Set = 0, Tekrar = 0, Aciklama = "-" }
                };
                _context.AntrenmanProgramlari.AddRange(detayliListe);
                _context.SaveChanges();
                mevcutProgram = detayliListe;
            }

            string bugun = DateTime.Now.ToString("dddd", new CultureInfo("tr-TR"));
            var bugunkuProgram = mevcutProgram.FirstOrDefault(x => x.Gun == bugun);
            ViewBag.BugunkuProgram = bugunkuProgram?.Bolge ?? "Dinlenme Günü";

            // --- ÖLÇÜM VERÝLERÝ ---
            ViewBag.SonOlcum = _context.Olcumler.Where(x => x.UyeId == uyeId).OrderByDescending(x => x.OlcumTarihi).FirstOrDefault();
            ViewBag.OlcumGecmisi = _context.Olcumler.Where(x => x.UyeId == uyeId).OrderBy(x => x.OlcumTarihi).ToList();

            // --- GÝRÝÞ KAYITLARI ---
            ViewBag.SonGirisler = _context.GirisKayitlari.Where(x => x.UyeId == uyeId).OrderByDescending(x => x.GirisZamani).Take(5).ToList();

            // --- BESLENME PROGRAMI ---
            ViewBag.BeslenmeProgrami = _context.BeslenmeProgramlari.Where(x => x.UyeId == uyeId).ToList();

            return View(uye);
        }

        // --- ADMIN ÝÇÝN ÜYE ÖLÇÜM YÖNETÝMÝ ---
        public IActionResult UyeOlcumleri(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin") return RedirectToAction("Login", "Account");

            var uye = _context.Uyeler.Find(id);
            if (uye == null) return NotFound();

            var olcumler = _context.Olcumler
                .Where(o => o.UyeId == id)
                .OrderByDescending(o => o.OlcumTarihi)
                .ToList();

            ViewBag.UyeAdSoyad = $"{uye.Ad} {uye.Soyad}";
            ViewBag.UyeId = id;

            return View(olcumler);
        }

        // --- ÖLÇÜM EKLEME (Üye Kendi Eklerken - Eski Mantýk Korundu) ---
        [HttpGet]
        public IActionResult OlcumEkle()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            // Eðer adminse burayý deðil, UyeOlcumleri sayfasýný kullanmalý
            if (userRole == "Admin") return RedirectToAction("Index");
            if (userRole == null) return RedirectToAction("Login", "Account");
            return View();
        }

        // --- ÖLÇÜM KAYDETME (Hem Admin Hem Üye Ýçin Ortak POST) ---
        [HttpPost]
        public IActionResult OlcumEkle(Olcumler yeniOlcum)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var oturumdakiUyeId = HttpContext.Session.GetInt32("UyeId");

            // Eðer admin ekliyorsa yeniOlcum.UyeId zaten formdan geliyor
            // Eðer üye ekliyorsa güvenli olmasý için sessiondan ID'yi biz atýyoruz
            if (userRole == "Uye")
            {
                if (oturumdakiUyeId == null) return RedirectToAction("Login", "Account");
                yeniOlcum.UyeId = oturumdakiUyeId.Value;
            }

            yeniOlcum.OlcumTarihi = DateTime.Now;

            _context.Olcumler.Add(yeniOlcum);
            _context.SaveChanges();

            // Admin eklediyse liste sayfasýna, üye eklediyse profile dön
            if (userRole == "Admin") return RedirectToAction("UyeOlcumleri", new { id = yeniOlcum.UyeId });
            return RedirectToAction("Profilim");
        }

        public IActionResult Antrenman(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var oturumdakiUyeId = HttpContext.Session.GetInt32("UyeId");

            if (string.IsNullOrEmpty(userRole)) return RedirectToAction("Login", "Account");

            int hedefId;
            if (userRole == "Admin")
            {
                if (!id.HasValue)
                {
                    ViewBag.UyeListesi = _context.Uyeler.ToList();
                    return View("AdminAntrenmanYoneticisi", new List<AntrenmanProgramlari>());
                }
                hedefId = id.Value;
            }
            else
            {
                if (oturumdakiUyeId == null) return RedirectToAction("Login", "Account");
                hedefId = oturumdakiUyeId.Value;
            }

            var mevcutProgram = _context.AntrenmanProgramlari.Where(x => x.UyeId == hedefId).ToList();

            if (mevcutProgram.Count == 0)
            {
                var varsayilanListe = new List<AntrenmanProgramlari>
                {
                    new AntrenmanProgramlari { UyeId = hedefId, Gun = "Pazartesi", Bolge = "Göðüs - Ön Kol", HareketAdi = "Bench Press", Set = 4, Tekrar = 12, Aciklama = "Giriþ" },
                    new AntrenmanProgramlari { UyeId = hedefId, Gun = "Salý", Bolge = "Sýrt - Arka Kol", HareketAdi = "Lat Pulldown", Set = 4, Tekrar = 12, Aciklama = "Giriþ" },
                    new AntrenmanProgramlari { UyeId = hedefId, Gun = "Çarþamba", Bolge = "Dinlenme", HareketAdi = "-", Set = 0, Tekrar = 0, Aciklama = "-" },
                    new AntrenmanProgramlari { UyeId = hedefId, Gun = "Perþembe", Bolge = "Omuz - Bacak", HareketAdi = "Shoulder Press", Set = 4, Tekrar = 12, Aciklama = "Giriþ" },
                    new AntrenmanProgramlari { UyeId = hedefId, Gun = "Cuma", Bolge = "Full Body", HareketAdi = "Þýnav/Mekik", Set = 3, Tekrar = 15, Aciklama = "Giriþ" },
                    new AntrenmanProgramlari { UyeId = hedefId, Gun = "Cumartesi", Bolge = "Kardiyo", HareketAdi = "Yürüyüþ", Set = 1, Tekrar = 30, Aciklama = "dk" },
                    new AntrenmanProgramlari { UyeId = hedefId, Gun = "Pazar", Bolge = "Dinlenme", HareketAdi = "-", Set = 0, Tekrar = 0, Aciklama = "-" }
                };

                _context.AntrenmanProgramlari.AddRange(varsayilanListe);
                _context.SaveChanges();
                mevcutProgram = varsayilanListe;
            }

            var seciliUye = _context.Uyeler.Find(hedefId);
            ViewBag.SeciliUyeAd = $"{seciliUye.Ad} {seciliUye.Soyad}";
            ViewBag.UyeId = hedefId;
            ViewBag.UyeListesi = _context.Uyeler.ToList();

            if (userRole == "Uye") return View("UyeAntrenmanGorunumu", mevcutProgram);
            return View("AdminAntrenmanYoneticisi", mevcutProgram);
        }

        public async Task<IActionResult> Duzenle()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var uyeId = HttpContext.Session.GetInt32("UyeId");
            if (userRole == "Admin" || uyeId == null) return RedirectToAction("Index");

            var uye = await _context.Uyeler.FindAsync(uyeId);
            if (uye == null) return NotFound();
            return View(uye);
        }

        public IActionResult BeslenmeYonetimi(int id)
        {
            var uye = _context.Uyeler.Find(id);
            if (uye == null) return NotFound();

            ViewBag.UyeId = id;
            ViewBag.UyeAd = uye.Ad + " " + uye.Soyad;

            var program = _context.BeslenmeProgramlari.Where(x => x.UyeId == id).ToList();
            return View(program);
        }

        [HttpPost]
        public IActionResult OgunEkle(BeslenmeProgramlari model)
        {
            if (ModelState.IsValid)
            {
                model.Tarih = DateTime.Now;
                _context.BeslenmeProgramlari.Add(model);
                _context.SaveChanges();
                return RedirectToAction("BeslenmeYonetimi", new { id = model.UyeId });
            }
            return BadRequest("Veri kaydedilemedi.");
        }

        [HttpPost]
        public IActionResult OgunSil(int id)
        {
            var ogun = _context.BeslenmeProgramlari.Find(id);
            if (ogun != null)
            {
                int uyeId = ogun.UyeId;
                _context.BeslenmeProgramlari.Remove(ogun);
                _context.SaveChanges();
                return RedirectToAction("BeslenmeYonetimi", new { id = uyeId });
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> BesinAra(string kelime)
        {
            if (string.IsNullOrEmpty(kelime)) return Json(new { products = new List<object>() });

            try
            {
                string encodeKelime = Uri.EscapeDataString(kelime);
                string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={encodeKelime}&search_simple=1&action=process&json=1&page_size=5";

                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "SporSalonuApp/1.0");

                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return Content(content, "application/json");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { hata = ex.Message });
            }
            return Json(new { products = new List<object>() });
        }

        public IActionResult GirisTakibi()
        {
            var sonGirisler = _context.GirisKayitlari
                .Include(g => g.Uye)
                .OrderByDescending(g => g.GirisZamani)
                .Take(10)
                .ToList();

            ViewBag.UyeListesi = _context.Uyeler.ToList();
            return View(sonGirisler);
        }

        [HttpPost]
        public IActionResult TurnikeGecisi(int id)
        {
            var uye = _context.Uyeler.Find(id);
            if (uye == null)
            {
                TempData["Hata"] = "Geçersiz Üye!";
                return RedirectToAction("Index");
            }

            var yeniGiris = new GirisKayitlari
            {
                UyeId = id,
                GirisZamani = DateTime.Now
            };

            _context.GirisKayitlari.Add(yeniGiris);
            _context.SaveChanges();

            TempData["Mesaj"] = $"{uye.Ad} {uye.Soyad} giriþ yaptý. Ýyi antrenmanlar!";
            return RedirectToAction("Index");
        }
        public IActionResult OdemeGecmisi(int id)
        {
            var uye = _context.Uyeler.Find(id);
            if (uye == null) return NotFound();

            var odemeler = _context.Odemeler
                .Where(o => o.UyeId == id)
                .OrderByDescending(o => o.OdemeTarihi)
                .ToList();

            ViewBag.UyeAdSoyad = $"{uye.Ad} {uye.Soyad}";
            ViewBag.UyeId = id;
            return View(odemeler);
        }

        [HttpPost]
        public IActionResult OdemeAl(Odemeler yeniOdeme)
        {
            // Modelinde OdemeTarihi olduðu için dýþarýdan girmek yerine otomatik atýyoruz
            yeniOdeme.OdemeTarihi = DateTime.Now;

            _context.Odemeler.Add(yeniOdeme);
            _context.SaveChanges();

            return RedirectToAction("OdemeGecmisi", new { id = yeniOdeme.UyeId });
        }
    }
}