using System;

namespace model {
	[Serializable]public class BookedTripDTO {
		public int ClientId { get; set; }
		public string ClientName { get; set; }
		public int SeatNumber { get; set; }

		public BookedTripDTO(int clientId, string clientName, int seatNumber) {
			ClientId = clientId;
			ClientName = clientName;
			SeatNumber = seatNumber;
		}
	}
}