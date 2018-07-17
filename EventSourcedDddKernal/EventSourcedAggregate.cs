using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventSourcedDddKernal
{
    public abstract class EventSourcedAggregate
    {
        public long Id { get; protected set; } = -1;

        private List<DomainEvent> domainEvents;
        private long nextEventId;
        private Dictionary<Type, MethodInfo> applyFunctionMap;

        protected EventSourcedAggregate()
        {
            this.domainEvents = new List<DomainEvent>();

            applyFunctionMap = this.GetType()
                 .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)
                 .Where(m => m.GetCustomAttributes(typeof(EventApplier), false).FirstOrDefault() != null)
                 .ToDictionary(x =>
                 {
                     var parameters = x.GetParameters();

                     if (parameters.Count() > 1)
                     {
                         throw new ArgumentException("Error on Method " + x.Name + ": Event Appliers must have only one parameter");
                     }

                     var paramType = parameters[0].ParameterType;
                     if (!paramType.IsSubclassOf(typeof(DomainEvent)))
                     {
                         throw new ArgumentException("Error on Method " + x.Name + "Event Appliers must have exactly one paramter that inherits from the DomainEvent class");
                     }

                     return paramType;
                 });

            nextEventId = 1;
        }

        protected static List<Type> ListOfSupportedEvents<T>() where T : EventSourcedAggregate
        {
            return typeof(T)
                 .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)
                 .Where(m => m.GetCustomAttributes(typeof(EventApplier), false).FirstOrDefault() != null)
                 .Select(x =>
                 {
                     var parameters = x.GetParameters();

                     if (parameters.Count() > 1)
                     {
                         throw new ArgumentException("Error on Method " + x.Name + ": Event Appliers must have only one parameter");
                     }

                     var paramType = parameters[0].ParameterType;
                     if (!paramType.IsSubclassOf(typeof(DomainEvent)))
                     {
                         throw new ArgumentException("Error on Method " + x.Name + "Event Appliers must have exactly one paramter that inherits from the DomainEvent class");
                     }

                     return paramType;
                 }).ToList();
        }

        public bool IsNew()
        {
            return this.Id == -1;
        }

        public List<DomainEvent> GetNewDomainEvents()
        {
            return this.domainEvents;
        }

        public void SetAggregateId(long id)
        {
            if (this.Id != -1)
            {
                throw new ArgumentException("Id has already been set for this aggregate and cannot be set or changed");
            }

            this.Id = id;
        }

        protected void ApplyEvent(DomainEvent dEvent)
        {
            this.ApplyEventOntoAggregate(dEvent);
            this.AddEventToEventList(dEvent);
        }

        private void ApplyEventOntoAggregate(DomainEvent dEvent)
        {
            MethodInfo applyMethod;

            if (!applyFunctionMap.TryGetValue(dEvent.GetType(), out applyMethod))
            {
                throw new Exception("The Aggregate " + this.GetType() + " does not have an event applier tagged that accepts the event type " + dEvent.GetType());
            }

            applyMethod.Invoke(this, new object[1] { dEvent });
        }

        protected void ReplayEvents(IEnumerable<DomainEvent> events)
        {
            var orderedEvents = events.OrderBy(x => x.DomainEventId);

            foreach(var e in events)
            {
                this.ApplyEventOntoAggregate(e);
            }

            this.nextEventId = orderedEvents.Last().DomainEventId + 1;
        }

        private void AddEventToEventList(DomainEvent dEvent)
        {
            dEvent.SetEventId(this.nextEventId);
            this.domainEvents.Add(dEvent);
            this.nextEventId++;
        }
    }
}
