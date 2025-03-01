using System.ComponentModel.DataAnnotations;

namespace RestaurantesReservas.Web.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}
