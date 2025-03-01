using System.ComponentModel.DataAnnotations;

namespace RestaurantesReservas.Web.Models
{
    public class Reservation
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RestaurantName { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
    }
}
