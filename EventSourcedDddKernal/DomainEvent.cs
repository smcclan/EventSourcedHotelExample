using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcedDddKernal
{
    public class DomainEvent
    {
        /// <summary>
        /// A sequencial Id the indicates the position a domain event has in the event stream. 
        /// This Id is created when the event is, and is usefull when saving an event. It helps to prevent race conditions,
        /// if another process modifies the same entity the Id count will be off and we will get a key conflict when we save.
        /// </summary>
        public long DomainEventId { get; private set; }
        public string User { get; private set; }
        public DateTime DateCreated { get; private set; }

        public DomainEvent(string user)
        {
            this.DomainEventId = -1;
            this.DateCreated = DateTime.Now;
            this.User = user;
        }

        public void SetEventId(long id)
        {
            if (this.DomainEventId != -1)
            {
                throw new ArgumentException("Id has already been set for this event and cannot be set or changed");
            }

            this.DomainEventId = id;
        }
    }
}
