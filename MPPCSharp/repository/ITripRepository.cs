using System;
using System.Collections.Generic;
using MPPCSharp.domain;

namespace MPPCSharp.repository {
    public interface ITripRepository : ICrudRepository<int, Trip> {
        public List<TripDTO> GetAllTrips();
        public int? GetTripIdByDestinationAndDeparture(string destinationName, DateTime departure);
    }
}