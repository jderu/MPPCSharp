namespace model {
    public class BookedTripDTO {
        public int ClientId { get; }
        public string ClientName { get; }
        public int SeatNumber { get; }

        public BookedTripDTO(int clientId, string clientName, int seatNumber) {
            ClientId = clientId;
            ClientName = clientName;
            SeatNumber = seatNumber;
        }
    }
}