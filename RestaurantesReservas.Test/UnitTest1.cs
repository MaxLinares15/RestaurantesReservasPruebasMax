using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using RestaurantesReservas.Web.Controllers;
using RestaurantesReservas.Web.Models;
using RestaurantesReservas.Web.Service;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantesReservas.Test
{
    public class AccountControllerTests
    {
        [Fact]
        public void Dashboard_LoadsUserReservations()
        {
            // Arrange
            var mockReservationService = new Mock<IReservationService>();
            var userId = "admin";
            var reservations = new List<Reservation>
                {
                    new Reservation { Id = 1, UserId = userId, RestaurantName = "Laurel", ReservationDate = DateTime.Now, NumberOfPeople = 2 },
                    new Reservation { Id = 2, UserId = userId, RestaurantName = "Sully's", ReservationDate = DateTime.Now, NumberOfPeople = 4 }
                };
            mockReservationService.Setup(service => service.GetActiveReservations(userId)).Returns(reservations);
            var controller = new AccountController(mockReservationService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Session = new MockHttpSession();
            controller.ControllerContext.HttpContext.Session.SetString("UserId", userId);

            // Act
            var result = controller.Dashboard() as ViewResult;
            var model = result.Model as List<Reservation>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Login_And_Enter_Dashboard()
        {
            // Arrange
            var mockReservationService = new Mock<IReservationService>();
            var controller = new AccountController(mockReservationService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Session = new MockHttpSession();
            controller.ControllerContext.HttpContext.Session.SetString("UserId", "user123");

            // Act
            var result = controller.Dashboard() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Open_Restaurant_List()
        {
            // Arrange
            var mockReservationService = new Mock<IReservationService>();
            var controller = new AccountController(mockReservationService.Object);

            // Act
            var result = controller.RestaurantList() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Reservation_Missing_NumberOfPeople()
        {
            // Arrange
            var mockReservationService = new Mock<IReservationService>();
            var controller = new AccountController(mockReservationService.Object);
            var reservation = new Reservation { RestaurantName = "Adrian Tropical", ReservationDate = DateTime.Now };

            // Act
            var result = controller.MakeReservation(reservation) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }

        [Fact]
        public void Reservation_Missing_Date()
        {
            // Arrange
            var mockReservationService = new Mock<IReservationService>();
            var controller = new AccountController(mockReservationService.Object);
            var reservation = new Reservation { RestaurantName = "Adrian Tropical", NumberOfPeople = 4 };

            // Act
            var result = controller.MakeReservation(reservation) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }
    }
}
