using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Events
{
    public class CheckInDateChanged : DomainEvent
    {
        public DateTime NewCheckInDate;

        public CheckInDateChanged(DateTime newCheckInDate, string user) : base(user)
        {
            this.NewCheckInDate = newCheckInDate;
        }
    }
}
