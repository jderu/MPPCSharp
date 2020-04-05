using System;
using System.Collections.Generic;
using MPPCSharp.domain;
using MPPCSharp.repository;

namespace MPPCSharp.service {
	public class AppService {
		private IBookedTripRepository _bookedTripRepo;
		private IClientRepository _clientRepo;
		private IDestinationRepository _destinationRepo;
		private ITripRepository _tripRepo;

		public AppService(IBookedTripRepository bookedTripRepo, IClientRepository clientRepo, IDestinationRepository destinationRepo,
			ITripRepository tripRepo) {
			_bookedTripRepo = bookedTripRepo;
			_clientRepo = clientRepo;
			_destinationRepo = destinationRepo;
			_tripRepo = tripRepo;
		}

		public List<BookedTripDTO> Search(string destinationName, DateTime departure) {
			return _bookedTripRepo.Search(destinationName, departure);
		}

		public BookedTrip FindClientId(int tripId, int seatNumber) {
			return _bookedTripRepo.FindOne(new BookedTripID(tripId, seatNumber));
		}

		public void Reserve(int tripId, string clientName, int seatNumber) {
			Trip trip = _tripRepo.FindOne(tripId);
			if (trip == null)
				throw new AppServiceException("No trip with the given id");

			BookedTrip bookedTrip = _bookedTripRepo.FindOne(new BookedTripID(tripId, seatNumber));
			if (bookedTrip != null)
				throw new AppServiceException("Seat is already booked for the specified trip");

			Client client = _clientRepo.FindByName(clientName);
			if (client == null) {
				_clientRepo.Save(new Client(-1, clientName));
				client = _clientRepo.FindByName(clientName);
			}

			_bookedTripRepo.Save(new BookedTrip(new BookedTripID(tripId, seatNumber), client.Id));
			trip.FreeSeats--;
			_tripRepo.Update(trip);
		}

		public List<TripDTO> ShowTrips() { return _tripRepo.GetAllTrips(); }

		public void AddTripDto() { _tripRepo.Save(new Trip(20, 2, DateTime.Now, 12)); }

		public int? GetTripIdByDestinationAndDeparture(String destination, DateTime departure) {
			return _tripRepo.GetTripIdByDestinationAndDeparture(destination, departure);
		}

		public List<BookedTripDTO> CreateList(List<BookedTripDTO> result) {
			List<BookedTripDTO> temporary = new List<BookedTripDTO>(18);
			for (int i = 0; i < 18; i++)
				temporary.Add(new BookedTripDTO(-1, "-", i + 1));
			foreach (var a in result)
				temporary[a.SeatNumber - 1] = a;
			return temporary;
		}
	}
}