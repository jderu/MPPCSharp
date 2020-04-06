using System;
using System.Collections.Generic;
using model;

namespace persistence {
    public interface ITripRepository : ICrudRepository<int, Trip> {
        public List<TripDTO> GetAllTrips();
        public int? GetTripIdByDestinationAndDeparture(string destinationName, DateTime departure);
    }
}