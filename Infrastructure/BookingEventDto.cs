using EventSourcedDddKernal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class BookingEventDto
    {
        public long BookingId;
        public long EventId;
        public string EventType;
        public string Payload;
        public DateTime CreatedOn;
        public string User;

        public DomainEvent ToDomain(List<Type> eventTypes)
        {
            var domainEventType = eventTypes.SingleOrDefault(x => x.ToString() == this.EventType);

            if(domainEventType == null) {
                throw new Exception(String.Format("Event Type {0} is not supported for the Booking aggregate", this.EventType));
            }

            var domainEvent = (DomainEvent)JsonConvert.DeserializeObject(Payload, domainEventType);
            domainEvent.SetEventId(this.EventId);
            return domainEvent;
        }
    }
}
