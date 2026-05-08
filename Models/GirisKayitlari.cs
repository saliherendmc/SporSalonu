using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonu.Models
{
    public class GirisKayitlari
    {
        [Key]
        public int Id { get; set; }

        // Hangi üyenin giriş yaptığını tutan yabancı anahtar
        [Required]
        public int UyeId { get; set; }

        // Giriş anındaki tarih ve saat bilgisi
        // Varsayılan olarak o anki zamanı atar
        public DateTime GirisZamani { get; set; } = DateTime.Now;

        // Navigasyon Özelliği: 
        // Kod tarafında girişi yapan üyenin bilgilerine (Ad, Soyad vb.) direkt ulaşmanı sağlar.
        [ForeignKey("UyeId")]
        public virtual Uyeler? Uye { get; set; }
    }
}