using System;

namespace model {
    [Serializable]public class TripDTO {
        public string DestinationName { get; set; }
        public DateTime Departure { get; set; }
        public int FreeSeats { get; set; }

        public TripDTO(string destinationName, DateTime departure, int freeSeats) {
            DestinationName = destinationName;
            Departure = departure;
            FreeSeats = freeSeats;
        }
    }
}