using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApi.Models
{
    public class CreateBookingModel
    {
        public IEnumerable<long> Rooms { get; set; }
        public int NumberOfOccupents { get; set; }
        public int DaysBooked { get; set; }
        public DateTime CheckInDate { get; set; }
    }
}
