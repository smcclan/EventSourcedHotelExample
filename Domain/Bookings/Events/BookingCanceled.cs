using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Events
{
    public class BookingCanceled : DomainEvent
    {
        public BookingStatus newBookingStatus = BookingStatus.Canceled;
        public BookingCanceled(string user) : base(user)
        {
        }
    }
}
