using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    //this would probably be fleshed out into an actual event structure
    public class BookingEvent: IRequest
    {
        public Booking Booking;

        public BookingEvent(Booking booking)
        {
            this.Booking = booking;
        }
    }
}
