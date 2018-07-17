using Application;
using Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class Class1
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Test1()
        {
            var rooms = new List<Room>()
            {
                new Room()
            };

            var a = Booking.Create(rooms.Select(x => x.Id), 4, 6, DateTime.Today, "TempUser");
        }

        [Test]
        public void Test2()
        {
            //var a = new HotelBookings();
        }
    }
}
