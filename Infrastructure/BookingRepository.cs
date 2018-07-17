using Application;
using Dapper;
using Domain;
using EventSourcedDddKernal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class BookingRepository : IBookingRepo
    {
        private string connectionString;

        public BookingRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Booking> GetById(long id)
        {
            var selectBookingSql = @"Select [BookingId], [EventId], [EventType], [Payload], [CreatedOn], [User] 
                                     FROM [dbo].[Bookings] WHERE BookingId = @bookingId";

            var supportedEvents = Booking.SupportedEventList();

            using (var conn = new SqlConnection(this.connectionString))
            {
                var eventsDto = await conn.QueryAsync<BookingEventDto>(selectBookingSql, new { bookingId = id });

                var bookingId = eventsDto.Select(x => x.BookingId).ToHashSet().SingleOrDefault();

                var domainEvents = eventsDto.Select(x =>  x.ToDomain(supportedEvents)).ToList();

                return Booking.Reconstitute(bookingId, domainEvents);
            }
        }

        public async Task Save(Booking booking)
        {
            var newEvents = booking.GetNewDomainEvents();

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        DataTable dataTable;

                        bulkCopy.DestinationTableName = "dbo.Bookings";
                        bulkCopy.ColumnMappings.Add("BookingId", "BookingId");
                        bulkCopy.ColumnMappings.Add("EventId", "EventId");
                        bulkCopy.ColumnMappings.Add("EventType", "EventType");
                        bulkCopy.ColumnMappings.Add("Payload", "Payload");
                        bulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                        bulkCopy.ColumnMappings.Add("User", "User");

                        if (booking.IsNewBooking())
                        {
                            await conn.ExecuteAsync("INSERT INTO [dbo].[BookingIds] DEFAULT VALUES", new { }, transaction);
                            var newId = await conn.QueryFirstAsync<long>("SELECT SCOPE_IDENTITY() AS [NewBookingId]; ", transaction: transaction);
                            booking.SetAggregateId(newId);
                            dataTable = BookingEventToDataTable.Convert(newId, newEvents);
                        }
                        else
                        {
                            dataTable = BookingEventToDataTable.Convert(booking.Id, newEvents);
                        }

                        DataTableReader reader = dataTable.CreateDataReader();
                        await bulkCopy.WriteToServerAsync(reader);

                        transaction.Commit();
                    }
                }
            }
        }
    }
}
