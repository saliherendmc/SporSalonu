using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class Olcumler
    {
        [Key]
        public int Id { get; set; }
        public int UyeId { get; set; }
        public DateTime OlcumTarihi { get; set; }
        public double Kilo { get; set; }
        public int Boy { get; set; }

        // Bu alanların her birinden SADECE BİRER TANE olduğundan emin ol:
        public double? YagOrani { get; set; }
        public double? BelCevresi { get; set; }
        public double? KolCevresi { get; set; }
        public double? GogusCevresi { get; set; }

        // Navigation Property - Hata almamak için sonuna ? koyarak nullable yapıyoruz
        public virtual Uyeler? Uye { get; set; }
    }
}