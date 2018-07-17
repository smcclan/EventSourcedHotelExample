CREATE TABLE [dbo].[Rm_BookedRooms]
(
	[RoomId] BIGINT NOT NULL,
	[BookingId] BIGINT NOT NULl, 
    CONSTRAINT [PK_Rm_BookedRooms] PRIMARY KEY ([RoomId], [BookingId])
)
