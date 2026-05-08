using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class AntrenmanProgramlari
    {
        [Key]
        public int Id { get; set; }
        public int UyeId { get; set; }

        // Navigation Property - Admin panelinde üye adını görmek için şart
        public virtual Uyeler? Uye { get; set; }

        public string? Gun { get; set; }
        public string? Bolge { get; set; }
        public string? HareketAdi { get; set; }
        public int Set { get; set; }
        public int Tekrar { get; set; }

        // HATA BURADAYDI: Bu alan eksik olduğu için HomeController hata veriyordu
        public string? Aciklama { get; set; }
    }
}