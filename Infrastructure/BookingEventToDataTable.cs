using EventSourcedDddKernal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace Infrastructure
{
    public class BookingEventToDataTable
    {
        public static DataTable Convert(long bookingId, IEnumerable<DomainEvent> events)
        {
            var table = new DataTable();
            table.Columns.Add("BookingId");
            table.Columns.Add("EventId");
            table.Columns.Add("EventType");
            table.Columns.Add("Payload");
            table.Columns.Add("CreatedOn");
            table.Columns.Add("User");

            foreach (var evnt in events)
            {
                var row = table.NewRow();
                row["BookingId"] = bookingId;
                row["EventId"] = evnt.DomainEventId;
                row["EventType"] = evnt.GetType().ToString();
                row["Payload"] = JsonConvert.SerializeObject(evnt);
                row["CreatedOn"] = evnt.DateCreated;
                row["User"] = evnt.User;

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
