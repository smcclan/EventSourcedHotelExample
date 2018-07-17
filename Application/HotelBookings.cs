using Application.Interfaces;
using Application.ReadModels;
using Domain;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public class HotelBookings
    {
        private IBookingRepo bookingRepo;
        private IBookingDao bookingDao;
        private IMediator mediator;

        public HotelBookings(IBookingRepo bookingRepo, IBookingDao bookingDao, IMediator mediator)
        {
            this.bookingRepo = bookingRepo;
            this.bookingDao = bookingDao;
            this.mediator = mediator;
        }

        public async Task<IEnumerable<BookingRm>> GetBookings()
        {
            var bookings = await this.bookingDao.GetBookings();
            return bookings;
        }

        public async Task CreateBooking(IEnumerable<long> roomIds, int occupants, int daysBooked, DateTime checkInDate, string user)
        {
            //validation would go here for the rooms being requested for this booking
            //...
            //...

            var booking = Booking.Create(roomIds, occupants, daysBooked, checkInDate, user);

            await bookingRepo.Save(booking);

            await mediator.Send(new BookingEvent(booking));
        }
    }
}
