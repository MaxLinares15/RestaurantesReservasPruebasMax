using Microsoft.AspNetCore.Mvc;
using RestaurantesReservas.Web.Models;
using RestaurantesReservas.Web.Service;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; // Asegúrate de incluir este espacio de nombres

namespace RestaurantesReservas.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly string _userFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
        private readonly string _reservationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "reservations.json");
        private readonly string _restaurantFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "restaurants.json");

        public AccountController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var users = GetUsers();
                var user = users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Guardar el usuario en la sesión
                    HttpContext.Session.SetString("UserId", user.Username);

                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Account");
                    }
                    else
                    {
                        // Redirigir a otro dashboard o página para usuarios normales
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var users = GetUsers();
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "User" // Asignar rol de usuario por defecto
                };
                users.Add(user);
                SaveUsers(users);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var reservations = GetReservations().Where(r => r.UserId == userId).ToList();
            var restaurants = GetRestaurants();
            ViewBag.Restaurants = restaurants;
            return View(reservations);
        }

        [HttpPost]
        public IActionResult CreateReservation(Reservation model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("UserId");
                var reservations = GetReservations();
                model.Id = reservations.Count > 0 ? reservations.Max(r => r.Id) + 1 : 1;
                model.UserId = userId;
                reservations.Add(model);
                SaveReservations(reservations);
                return RedirectToAction("Dashboard");
            }
            var userReservations = GetReservations().Where(r => r.UserId == HttpContext.Session.GetString("UserId")).ToList();
            var restaurants = GetRestaurants();
            ViewBag.Restaurants = restaurants;
            return View("Dashboard", userReservations);
        }

        [HttpPost]
        public IActionResult CancelReservation(int id)
        {
            var reservations = GetReservations();
            var reservation = reservations.FirstOrDefault(r => r.Id == id);
            if (reservation != null)
            {
                reservations.Remove(reservation);
                SaveReservations(reservations);
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AddRestaurant(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                var restaurants = GetRestaurants();
                restaurants.Add(restaurant);
                SaveRestaurants(restaurants);
                return RedirectToAction("Dashboard");
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult RestaurantList()
        {
            var restaurants = GetRestaurants();
            return View(restaurants);
        }

        [HttpPost]
        public IActionResult MakeReservation(Reservation reservation)
        {
            if (string.IsNullOrEmpty(reservation.RestaurantName) || reservation.NumberOfPeople <= 0 || reservation.ReservationDate == default)
            {
                return View("Error");
            }

            var reservations = GetReservations();
            reservation.Id = reservations.Count > 0 ? reservations.Max(r => r.Id) + 1 : 1;
            reservations.Add(reservation);
            SaveReservations(reservations);
            return RedirectToAction("Dashboard");
        }

        private List<User> GetUsers()
        {
            try
            {
                if (!System.IO.File.Exists(_userFilePath))
                {
                    return new List<User>();
                }

                var json = System.IO.File.ReadAllText(_userFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al leer el archivo de usuarios: {ex.Message}");
                return new List<User>();
            }
        }

        private void SaveUsers(List<User> users)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(users, options);
                System.IO.File.WriteAllText(_userFilePath, json);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al guardar el archivo de usuarios: {ex.Message}");
            }
        }

        private List<Reservation> GetReservations()
        {
            try
            {
                if (!System.IO.File.Exists(_reservationFilePath))
                {
                    return new List<Reservation>();
                }

                var json = System.IO.File.ReadAllText(_reservationFilePath);
                return JsonSerializer.Deserialize<List<Reservation>>(json) ?? new List<Reservation>();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al leer el archivo de reservas: {ex.Message}");
                return new List<Reservation>();
            }
        }

        private void SaveReservations(List<Reservation> reservations)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(reservations, options);
                System.IO.File.WriteAllText(_reservationFilePath, json);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al guardar el archivo de reservas: {ex.Message}");
            }
        }

        private List<Restaurant> GetRestaurants()
        {
            try
            {
                if (!System.IO.File.Exists(_restaurantFilePath))
                {
                    return new List<Restaurant>();
                }

                var json = System.IO.File.ReadAllText(_restaurantFilePath);
                return JsonSerializer.Deserialize<List<Restaurant>>(json) ?? new List<Restaurant>();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al leer el archivo de restaurantes: {ex.Message}");
                return new List<Restaurant>();
            }
        }

        private void SaveRestaurants(List<Restaurant> restaurants)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(restaurants, options);
                System.IO.File.WriteAllText(_restaurantFilePath, json);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al guardar el archivo de restaurantes: {ex.Message}");
            }
        }
    }
}

