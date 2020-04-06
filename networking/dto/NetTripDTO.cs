using System;

namespace networking.dto {
	[Serializable]public class NetTripDTO {
		private string _destinationName;
		private DateTime _departure;

		public NetTripDTO(string destinationName, DateTime departure) {
			_destinationName = destinationName;
			_departure = departure;
		}

		public string DestinationName { get => _destinationName; set => _destinationName = value; }
		public DateTime Departure { get => _departure; set => _departure = value; }
	}
}