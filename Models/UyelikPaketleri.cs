using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace SporSalonu.Models
{
    public class UyelikPaketleri
    {
        public int Id { get; set; }
        public string PaketAdi { get; set; }
        public string KacAylikPaket { get; set; }
        [Column(TypeName = "decimal(18,2)")] // 18 basamaklı, 2 basamağı kuruş olan sayı
        public decimal Ucret { get; set; }
        public string Aciklama { get; set; }
        // bir paketin bir çok üyesi olabilir.
        public ICollection<Uyeler> Uyeler { get; set; }

    }
}
