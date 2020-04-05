namespace MPPCSharp.domain {
    public class BookedTripID {
        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public BookedTripID(int tripId, int seatNumber) {
            TripId = tripId;
            SeatNumber = seatNumber;
        }
    }
}