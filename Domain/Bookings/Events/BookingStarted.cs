using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Events
{
    public class BookingStarted : DomainEvent
    {
        public BookingStatus newBookingStatus = BookingStatus.Started;
        public BookingStarted(string user) : base(user)
        {

        }
    }
}
