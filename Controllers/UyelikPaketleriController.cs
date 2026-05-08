using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;
using SporSalonu.Models;

namespace SporSalonu.Controllers
{
    public class UyelikPaketleriController : Controller
    {
        private readonly SporContext _context;

        public UyelikPaketleriController(SporContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME SAYFASI (Index)
        // GET: /UyelikPaketleri
        public async Task<IActionResult> Index()
        {
            // Veritabanındaki tüm paketleri çekip sayfaya gönderiyoruz
            var paketler = await _context.UyelikPaketleri.ToListAsync();
            return View(paketler);
        }

        // 2. KAYIT FORMU SAYFASI (Görüntüleme)
        // GET: /UyelikPaketleri/Create
        public IActionResult Create()
        {
            return View();
        }

        // 3. KAYIT İŞLEMİ (Veritabanına Yazma)
        // POST: /UyelikPaketleri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UyelikPaketleri paket)
        {
            // ModelState kontrolünü geçici olarak devre dışı bırakıyoruz
            try
            {
                _context.Add(paket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Eğer veritabanı aşamasında hata verirse buraya düşecek
                ViewBag.Hata = ex.Message;
                return View(paket);
            }
        }
            // 4. PAKET SİLME İŞLEMİ
            // GET: /UyelikPaketleri/Delete/5
public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paket = await _context.UyelikPaketleri
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paket == null)
            {
                return NotFound();
            }

            // Direkt silme işlemi (Onay sayfasıyla uğraşmamak için hızlı yöntem)
            _context.UyelikPaketleri.Remove(paket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
    }
