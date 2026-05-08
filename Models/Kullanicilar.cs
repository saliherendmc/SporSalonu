using System.ComponentModel.DataAnnotations;

namespace SporSalonu.Models
{
    public class Kullanicilar
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "E-posta gereklidir")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        // "Admin" veya "Uye" değerlerini alacak
        public string Rol { get; set; } = "Uye";
    }
}