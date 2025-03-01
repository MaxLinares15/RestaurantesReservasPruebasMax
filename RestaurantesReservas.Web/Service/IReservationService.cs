using RestaurantesReservas.Web.Models;
using System.Collections.Generic;

namespace RestaurantesReservas.Web.Service
{
    public interface IReservationService
    {
        IEnumerable<Reservation> GetActiveReservations(string userId);
        IEnumerable<Restaurant> GetAllRestaurants();
    }
}
