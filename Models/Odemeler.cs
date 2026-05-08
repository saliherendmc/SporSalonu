using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class Odemeler
    {
        [Key]
        public int Id { get; set; }

        public int UyeId { get; set; }

        public decimal Miktar { get; set; }

        public DateTime OdemeTarihi { get; set; }
    }
}