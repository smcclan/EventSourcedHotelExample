CREATE TABLE [dbo].[Bookings]
(
	[BookingId] BIGINT NOT NULL ,
	[EventId] BIGINT NOT NULL , 
    [EventType] VARCHAR(100) NOT NULL, 
    [Payload] VARCHAR(MAX) NOT NULL, 
    [CreatedOn] DATETIME2 NOT NULL, 
    [User] VARCHAR(50) NOT NULL, 

    CONSTRAINT [PK_Bookings] PRIMARY KEY ([BookingId], [EventId])
);

GO

CREATE NONCLUSTERED INDEX [IX_Bookings_BookingId] ON [dbo].[Bookings]
(
	[BookingId] ASC
);
GO
