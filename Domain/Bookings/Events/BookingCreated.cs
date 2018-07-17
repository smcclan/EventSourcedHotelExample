using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bookings.Events
{
    public class BookingCreated : DomainEvent
    {
        public IEnumerable<long> Rooms { get; private set; }
        public BookingStatus BookingStatus { get; private set; }
        public int NumberOfOccupents { get; private set; }
        public int DaysBooked { get; private set; }
        public DateTime CheckInDate { get; private set; }
        public DateTime DayBooked { get; private set; }

        public BookingCreated(IEnumerable<long> rooms, BookingStatus bookingStatus, int numberOfOccupents, 
            int daysBooked, DateTime checkInDate, DateTime dayBooked, string user) : base(user)
        {
            Rooms = rooms;
            BookingStatus = bookingStatus;
            NumberOfOccupents = numberOfOccupents;
            DaysBooked = daysBooked;
            CheckInDate = checkInDate;
            DayBooked = dayBooked;
        }
    }
}
