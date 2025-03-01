using System.ComponentModel.DataAnnotations;

namespace RestaurantesReservas.Web.Models
{
    public class Restaurant
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Cuisine { get; set; }
    }
}
