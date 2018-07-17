using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Commands
{
    public class CompleteBooking : DomainCommand
    {
        public CompleteBooking(string user) : base(user)
        {
        }
    }
}
