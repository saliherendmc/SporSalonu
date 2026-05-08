using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonu.Models
{
    public class BeslenmeProgramlari
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UyeId { get; set; }

        [Required]
        [Display(Name = "Öğün Adı")]
        public string OgunAdi { get; set; } // Örn: Kahvaltı, Ara Öğün
        public DateTime Tarih { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "İçerik")]
        public string Icerik { get; set; } // Örn: 3 Yumurta, 100gr Lor

        [Display(Name = "Kalori (kcal)")]
        public int Kalori { get; set; }

        [ForeignKey("UyeId")]
        public virtual Uyeler? Uye { get; set; }
    }
}