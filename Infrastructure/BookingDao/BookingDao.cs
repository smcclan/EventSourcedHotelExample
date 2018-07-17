using Application.Interfaces;
using Application.ReadModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BookingDao
{
    public class BookingDao : IBookingDao
    {
        private string connectionString;

        public BookingDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<BookingRm>> GetBookings()
        {
            var GetBookingsSql = @"
                SELECT TOP (1000) [BookingId]
                    ,[ActiveBooking]
                    ,[NumberOfOccupents]
                    ,[DaysBooked]
                    ,[BookingStartDate]
                    ,[BookingEndDate]
                FROM [EventSourcedHotel].[dbo].[Rm_Bookings];

                SELECT TOP (1000) [RoomId]
                    ,[BookingId]
                FROM [EventSourcedHotel].[dbo].[Rm_BookedRooms]";

            using (var conn = new SqlConnection(this.connectionString))
            {
                var multi = await conn.QueryMultipleAsync(GetBookingsSql);

                var bookings = (await multi.ReadAsync<BookingRm>()).ToList();
                var roomsList = await multi.ReadAsync<RoomDto>();

                var rooms = roomsList.GroupBy(x => x.BookingId).ToDictionary(x => x.Key, x => x.Select(y => y.RoomId).ToList());

                bookings.ForEach(x => x.Rooms = rooms[x.BookingId]);

                return bookings;
            }
        }

        public async Task StoreBooking(BookingRm booking)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    var deleteSql = "Delete FROM [dbo].[Rm_Bookings] WHERE BookingId = @bookingId; DELETE FROM [dbo].[Rm_BookedRooms] WHERE BookingId = @bookingId";
                    await conn.ExecuteAsync(deleteSql, new { bookingId = booking.BookingId }, transaction);

                    using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Rm_BookedRooms";
                        bulkCopy.ColumnMappings.Add("RoomId", "RoomId");
                        bulkCopy.ColumnMappings.Add("BookingId", "BookingId");

                        var table = new DataTable();
                        table.Columns.Add("RoomId");
                        table.Columns.Add("BookingId");

                        foreach (var roomId in booking.Rooms)
                        {
                            var row = table.NewRow();
                            row["RoomId"] = roomId;
                            row["BookingId"] = booking.BookingId;
                            table.Rows.Add(row);
                        }

                        DataTableReader reader = table.CreateDataReader();
                        await bulkCopy.WriteToServerAsync(reader);

                        var insertBookingSql = @"
                            INSERT INTO [dbo].[Rm_Bookings] (
	                            [BookingId]
                                ,[ActiveBooking]
                                ,[NumberOfOccupents]
                                ,[DaysBooked]
                                ,[BookingStartDate]
                                ,[BookingEndDate])
                            VALUES
                            (
	                            @bookingId,
                                @activeBooking,
                                @numberOfOccupents,
                                @daysBooked,
                                @bookingStartDate,
                                @bookingEndDate
                            )";

                        var sqlParams = new
                        {
                            bookingId = booking.BookingId,
                            activeBooking = booking.ActiveBooking,
                            numberOfOccupents = booking.NumberOfOccupents,
                            daysBooked = booking.DaysBooked,
                            bookingStartDate = booking.BookingStartDate,
                            bookingEndDate = booking.BookingEndDate
                        };

                        await conn.ExecuteAsync(insertBookingSql, sqlParams, transaction);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
