using System;
using System.Collections.Generic;
using MPPCSharp.domain;

namespace MPPCSharp.repository {
    public interface IBookedTripRepository : ICrudRepository<BookedTripID, BookedTrip> {
        public List<BookedTripDTO> Search(string destinationName, DateTime departure);
    }
}