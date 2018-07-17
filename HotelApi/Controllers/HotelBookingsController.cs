using Application;
using HotelApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApi.Controllers
{
    [Route("api/bookings")]
    public class HotelBookingsController: Controller
    {
        HotelBookings hotelBookings;

        public HotelBookingsController(HotelBookings hotelBookings)
        {
            this.hotelBookings = hotelBookings;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                return Ok(await hotelBookings.GetBookings());
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingModel model)
        {
            try
            {
                await this.hotelBookings.CreateBooking(model.Rooms, model.NumberOfOccupents, model.DaysBooked, model.CheckInDate, this.User.Identity.Name ?? "TempUser");
                return Ok();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
