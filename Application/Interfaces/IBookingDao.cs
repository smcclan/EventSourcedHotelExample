using Application.ReadModels;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingDao
    {
        Task<IEnumerable<BookingRm>> GetBookings();

        Task StoreBooking(BookingRm booking);
    }
}
