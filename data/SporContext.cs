using Microsoft.EntityFrameworkCore;
using SporSalonu.Models;

namespace SporSalonu.Data
{
    public class SporContext : DbContext
    {
        public SporContext(DbContextOptions<SporContext> options) : base(options) { }

        // Sisteme giriş yapan ve ölçümleri tutulan ana tablo
        public DbSet<Uyeler> Uyeler { get; set; }

        // Admin veya personel girişleri için kullandığın tablo
        public DbSet<Kullanicilar> Kullanicilar { get; set; }

        // Üyelerin boy, kilo, yağ oranı verileri
        public DbSet<Olcumler> Olcumler { get; set; }

        // Günlük antrenman hareketleri ve bölge bilgileri
        public DbSet<AntrenmanProgramlari> AntrenmanProgramlari { get; set; }

        // Salon giriş-çıkış logları
        public DbSet<GirisKayitlari> GirisKayitlari { get; set; }

        // Gold, Silver gibi üyelik türleri
        public DbSet<UyelikPaketleri> UyelikPaketleri { get; set; }
        public DbSet<BeslenmeProgramlari> BeslenmeProgramlari { get; set; }
        public DbSet<SporSalonu.Models.Odemeler> Odemeler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Eğer veritabanında tabloların çoğul (S takısı) değilse 
            // burada eşleştirme yaparak "bulunamadı" hatalarını önleyebilirsin.
            modelBuilder.Entity<Uyeler>().ToTable("Uyeler");
            modelBuilder.Entity<Kullanicilar>().ToTable("Kullanicilar");
            modelBuilder.Entity<AntrenmanProgramlari>().ToTable("AntrenmanProgramlari");

            base.OnModelCreating(modelBuilder);
        }
    }
}