using System;

namespace networking.dto {
	[Serializable] public class NetBookedTripDTO {
		int _tripId;
		int _seatNumber;

		public NetBookedTripDTO(int tripId, int seatNumber) {
			_tripId = tripId;
			_seatNumber = seatNumber;
		}

		public int TripId { get => _tripId; set => _tripId = value; }
		public int SeatNumber { get => _seatNumber; set => _seatNumber = value; }
	}
}