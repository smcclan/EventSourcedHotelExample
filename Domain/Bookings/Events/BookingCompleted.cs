using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Events
{
    public class BookingCompleted : DomainEvent
    {
        public BookingStatus newBookingStatus = BookingStatus.Complete;

        public BookingCompleted(string user) : base(user)
        {
        }
    }
}
