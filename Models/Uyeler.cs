using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonu.Models
{
    public class Uyeler
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;

        [NotMapped]
        public string AdSoyad => $"{Ad} {Soyad}";

        public string? Telefon { get; set; }
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public int UyelikPaketiId { get; set; }
        public int GunlukKaloriHedefi { get; set; } = 2000;
        public DateTime UyelikBitisTarihi { get; set; } = DateTime.Now.AddMonths(1); // Varsayılan 1 ay
        [ForeignKey("UyelikPaketiId")]
        public virtual UyelikPaketleri? UyelikPaketi { get; set; }
        public virtual ICollection<GirisKayitlari> GirisKayitlari { get; set; } = new List<GirisKayitlari>();
    }
}