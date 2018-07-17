using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Commands
{
    public class CancelBooking : DomainCommand
    {
        public CancelBooking(string user) : base(user)
        {
        }
    }
}
