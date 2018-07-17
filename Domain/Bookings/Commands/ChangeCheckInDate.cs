using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Commands
{
    public class ChangeCheckInDate : DomainCommand
    {
        public DateTime NewCheckInDate { get; private set; }

        public ChangeCheckInDate(DateTime newCheckInDate, string user) : base(user)
        {
            this.NewCheckInDate = newCheckInDate;
        }
    }
}
