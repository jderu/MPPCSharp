using System;

namespace networking.dto {
	[Serializable]public class NetUpdateDTO {
		private string _destinationName;
		private DateTime _departure;
		private int _seatNumber;
		private string _clientName;

		public NetUpdateDTO(string destinationName, DateTime departure, int seatNumber, string clientName) {
			this._destinationName = destinationName;
			this._departure = departure;
			this._seatNumber = seatNumber;
			this._clientName = clientName;
		}

		public string DestinationName { get => _destinationName; set => _destinationName = value; }
		public DateTime Departure { get => _departure; set => _departure = value; }
		public int SeatNumber { get => _seatNumber; set => _seatNumber = value; }
		public string ClientName { get => _clientName; set => _clientName = value; }
	}
}