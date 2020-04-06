using System;
using System.Collections.Generic;
using model;

namespace persistence {
    public interface IBookedTripRepository : ICrudRepository<BookedTripID, BookedTrip> {
        public List<BookedTripDTO> Search(string destinationName, DateTime departure);
    }
}