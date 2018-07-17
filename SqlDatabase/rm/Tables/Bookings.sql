CREATE TABLE [dbo].[Rm_Bookings]
(
	[BookingId] BIGINT NOT NULL PRIMARY KEY,
	[ActiveBooking] BIT NOT NUll,
	[NumberOfOccupents] INT NOT NULL,
	[DaysBooked] INT NOT NULL,
	[BookingStartDate] DATETIME2 NOT NULL,
	[BookingEndDate] DATETIME2 NOT NULL
)
