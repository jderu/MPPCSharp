using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model;
using persistence;
using services;

namespace server {
	public class Server : IAppServices {
		private readonly IBookedTripRepository _bookedTripRepo;
		private readonly IClientRepository _clientRepo;
		private readonly IDestinationRepository _destinationRepo;
		private readonly ITripRepository _tripRepo;
		private readonly IUserRepository _users;
		private readonly IDictionary<int, IAppObserver> _loggedUsers;

		public Server(IUserRepository users, IBookedTripRepository bookedTripRepo, IClientRepository clientRepo,
			IDestinationRepository destinationRepo, ITripRepository tripRepo) {
			_users = users;
			_bookedTripRepo = bookedTripRepo;
			_clientRepo = clientRepo;
			_destinationRepo = destinationRepo;
			_tripRepo = tripRepo;
			_loggedUsers = new Dictionary<int, IAppObserver>();
		}

		public User Login(String username, String password, IAppObserver client) {
			User user = _users.FindByUsername(username);
			if (user == null)
				return null;
			if (_loggedUsers.ContainsKey(user.Id))
				throw new AppServiceException("User already logged in");
			if (User.Hash(password) == user.PasswordHash) {
				_loggedUsers[user.Id] = client;
				return user;
			}

			return null;
		}

		public void Logout(int userId) {
			if (_loggedUsers.ContainsKey(userId))
				_loggedUsers.Remove(userId);
			else
				throw new AppServiceException("User isn't logged in");
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

			Destination destination = _destinationRepo.FindOne(trip.DestinationId);

			_bookedTripRepo.Save(new BookedTrip(new BookedTripID(tripId, seatNumber), client.Id));
			trip.FreeSeats--;
			_tripRepo.Update(trip);
			NotifyUsers(destination.Name, trip.Departure, seatNumber, clientName);
		}

		private void NotifyUsers(String destinationName, DateTime departure, int seatNumber, String clientName) {
			foreach (User us in _loggedUsers.Values) {
				if (_loggedUsers.ContainsKey(us.Id)) {
					IAppObserver chatClient = _loggedUsers[us.Id];
					Task.Run(() => chatClient.UpdateWindows(destinationName, departure, seatNumber, clientName));
				}
			}
		}

		public List<TripDTO> ShowTrips() { return _tripRepo.GetAllTrips(); }

		//public void AddTripDto() { _tripRepo.Save(new Trip(20, 2, DateTime.Now, 12)); }

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