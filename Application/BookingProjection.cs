using Application.Interfaces;
using Application.ReadModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BookingProjection : AsyncRequestHandler<BookingEvent>
    {
        IBookingDao dao;

        public BookingProjection(IBookingDao dao)
        {
            this.dao = dao;
        }

        protected override async Task HandleCore(BookingEvent request)
        {
            var projection = new BookingRm()
            {
                BookingId = request.Booking.Id,
                Rooms = request.Booking.Rooms,
                ActiveBooking = request.Booking.BookingStatus == Domain.BookingStatus.Started,
                NumberOfOccupents = request.Booking.NumberOfOccupents,
                DaysBooked = request.Booking.DaysBooked,
                BookingStartDate = request.Booking.CheckInDate,
                BookingEndDate = request.Booking.CheckInDate.AddDays(request.Booking.DaysBooked)
            };

            await this.dao.StoreBooking(projection);
        }
    }
}
