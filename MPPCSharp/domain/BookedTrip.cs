namespace MPPCSharp.domain {
	public class BookedTrip : Entity<BookedTripID> {
		public int ClientId { get; set; }

		public int TripId { get => Id.TripId; set => Id.TripId = value; }
		public int SeatNumber { get => Id.SeatNumber; set => Id.SeatNumber = value; }

		public BookedTrip(BookedTripID id, int clientId) : base(id) { ClientId = clientId; }

		public BookedTrip(int tripId, int seatNumber, int clientId) : base(new BookedTripID(tripId, seatNumber)) {
			ClientId = clientId;
		}
	}
}