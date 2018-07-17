using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ReadModels
{
    public class BookingRm
    {
        public long BookingId { get; set; }
        public IEnumerable<long> Rooms { get; set; }
        public bool ActiveBooking { get; set; }
        public int NumberOfOccupents { get; set; }
        public int DaysBooked { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
    }
}
