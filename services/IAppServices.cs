using System;
using System.Collections.Generic;
using model;

namespace services {
	public interface IAppServices {
		List<BookedTripDTO> Search(String destinationName, DateTime departure);

		BookedTrip FindClientId(int tripId, int seatNumber);

		void Reserve(int tripId, String clientName, int seatNumber);

		List<TripDTO> ShowTrips();

		int? GetTripIdByDestinationAndDeparture(String destination, DateTime departure);

		User Login(String username, String password, IAppObserver client);

		void Logout(int userId);
	}
}