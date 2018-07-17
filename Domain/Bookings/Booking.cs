using Domain.Bookings.Commands;
using Domain.Bookings.Events;
using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Booking : EventSourcedAggregate
    {
        public IEnumerable<long> Rooms { get; private set; }
        public BookingStatus BookingStatus { get; private set; }
        public int NumberOfOccupents { get; private set; }
        public int DaysBooked { get; private set; }
        public DateTime CheckInDate { get; private set; }
        public DateTime DayBooked { get; private set; }

        public static Booking Create(IEnumerable<long> roomIds, int numberOfOccupents, int daysBooked, DateTime checkInDate, string user)
        {
            var booking = new Booking();

            var bookingCreatedEvent = new BookingCreated(
                roomIds,
                BookingStatus.Booked,
                numberOfOccupents,
                daysBooked,
                checkInDate,
                DateTime.Now,
                user
            );

            booking.ApplyEvent(bookingCreatedEvent);

            return booking;
        }

        public static Booking Reconstitute(long id, IEnumerable<DomainEvent> events)
        {
            var booking = new Booking()
            {
                Id = id
            };
            booking.ReplayEvents(events);
            return booking;
        }

        public void Process(StartBooking command)
        {
            var e = new BookingStarted(command.User);
            ApplyEvent(e);
        }

        public void Process(CompleteBooking command)
        {
            var e = new BookingCompleted(command.User);
            ApplyEvent(e);
        }

        public void Process(CancelBooking command)
        {
            var e = new BookingCanceled(command.User);
            ApplyEvent(e);
        }

        public void Process(ChangeCheckInDate command)
        {
            var e = new CheckInDateChanged(command.NewCheckInDate, command.User);
            ApplyEvent(e);
        }

        [EventApplier]
        public void Apply(BookingCreated dEvent)
        {
            this.Rooms = dEvent.Rooms;
            this.BookingStatus = dEvent.BookingStatus;
            this.NumberOfOccupents = dEvent.NumberOfOccupents;
            this.DaysBooked = dEvent.DaysBooked;
            this.CheckInDate = dEvent.CheckInDate;
            this.DayBooked = dEvent.DayBooked;
        }

        [EventApplier]
        public void Apply(BookingStarted dEvent)
        {
            this.BookingStatus = dEvent.newBookingStatus;
        }

        [EventApplier]
        public void Apply(BookingCompleted dEvent)
        {
            this.BookingStatus = dEvent.newBookingStatus;
        }

        [EventApplier]
        public void Apply(BookingCanceled dEvent)
        {
            this.BookingStatus = dEvent.newBookingStatus;
        }

        [EventApplier]
        public void Apply(CheckInDateChanged dEvent)
        {
            this.CheckInDate = dEvent.NewCheckInDate;
        }

        public bool IsNewBooking()
        {
            return this.Id == -1;
        }

        public static List<Type> SupportedEventList()
        {
            return ListOfSupportedEvents<Booking>();
        }
    }
}
