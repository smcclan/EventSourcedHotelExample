using Domain.Rooms.Commands;
using Domain.Rooms.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Room
    {
        public long Id { get; private set; }
        public int FloorNumber { get; private set; }
        public int RoomNumber { get; private set; }
        public int Capacity { get; private set; }
        public bool Booked { get; private set; }

        public Room()
        {
            this.Id = 1;
            this.FloorNumber = 12;
            this.RoomNumber = 48;
            this.Capacity = 2;
            this.Booked = false;
        }

        public void Process(CheckIntoRoom command)
        {
            //Create RoomCheckedInto
        }

        public void Process(CheckOutOfRoom command)
        {
            //Create RoomCheckedOutOf
        }

        public void Apply(RoomCheckedInto dEvent)
        {

        }

        public void Apply(RoomCheckedOutOf dEvent)
        {

        }
    }
}
