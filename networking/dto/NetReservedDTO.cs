using System;

namespace networking.dto {
	[Serializable]public class NetReservedDTO {
		int _tripId;
		string _clientName;
		int _seatNumber;

		public NetReservedDTO(int tripId, string clientName, int seatNumber) {
			_tripId = tripId;
			_clientName = clientName;
			_seatNumber = seatNumber;
		}

		public int TripId { get => _tripId; set => _tripId = value; }
		public string ClientName { get => _clientName; set => _clientName = value; }
		public int SeatNumber { get => _seatNumber; set => _seatNumber = value; }
	}
}