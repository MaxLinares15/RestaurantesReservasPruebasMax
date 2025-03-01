using RestaurantesReservas.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantesReservas.Web.Service
{
    public class ReservationService : IReservationService
    {
        private readonly List<Reservation> _reservations;
        private readonly List<Restaurant> _restaurants;

        public ReservationService()
        {
            // Inicializar con datos de ejemplo o cargar desde una fuente de datos
            _reservations = new List<Reservation>();
            _restaurants = new List<Restaurant>();
        }

        public IEnumerable<Reservation> GetActiveReservations(string userId)
        {
            return _reservations.Where(r => r.UserId == userId);
        }

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            return _restaurants;
        }
    }
}