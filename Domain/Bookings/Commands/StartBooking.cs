using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Commands
{
    public class StartBooking: DomainCommand
    {
        public StartBooking(string user) : base(user)
        {

        }
    }
}
