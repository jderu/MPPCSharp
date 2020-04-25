using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppService;
using Grpc.Core;
using model;
using persistence;
using TripDTO = model.TripDTO;

namespace server {
	public class ServerImpl : AppService.AppService.AppServiceBase {
		private readonly IBookedTripRepository _bookedTripRepo;
		private readonly IClientRepository _clientRepo;
		private readonly IDestinationRepository _destinationRepo;
		private readonly ITripRepository _tripRepo;

		private readonly IUserRepository _userRepo;

		private readonly IDictionary<int, IServerStreamWriter<ReserveResponse>> _observers =
			new Dictionary<int, IServerStreamWriter<ReserveResponse>>();


		public ServerImpl(IUserRepository userRepo, IBookedTripRepository bookedTripRepo, IClientRepository clientRepo,
			IDestinationRepository destinationRepo, ITripRepository tripRepo) {
			_userRepo = userRepo;
			_bookedTripRepo = bookedTripRepo;
			_clientRepo = clientRepo;
			_destinationRepo = destinationRepo;
			_tripRepo = tripRepo;
		}

		private DateTime GoogleToSql(Google.Protobuf.WellKnownTypes.Timestamp time) { return time.ToDateTime(); }

		private Google.Protobuf.WellKnownTypes.Timestamp SqlToGoogle(DateTime time) {
			return Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(time.ToUniversalTime().AddHours(2));
		}

		public override Task<SearchResponse> search(SearchRequest request, ServerCallContext context) {
			var searchResponse = new SearchResponse();
			foreach (var bookedTripDTO in _bookedTripRepo.Search(request.DestinationName, GoogleToSql(request.Departure))) {
				var bookedTrip = new AppService.BookedTripDTO();
				bookedTrip.ClientName = bookedTripDTO.ClientName;
				bookedTrip.SeatNumber = bookedTripDTO.SeatNumber;
				bookedTrip.ClientID = bookedTripDTO.ClientId;
				searchResponse.List.Add(bookedTrip);
			}

			return Task.FromResult(searchResponse);
		}

		private async void NotifyUsers(String destinationName, DateTime departure, int seatNumber, String clientName) {
			foreach (var appClient in _observers.Values) {
				var a = new ReserveResponse();
				a.Type = ResponseType.Data;
				a.DestinationName = destinationName;
				try {
					a.Departure = SqlToGoogle(departure);
				}
				catch (Exception e) {
					Console.WriteLine(e);
				}
				a.SeatNumber = seatNumber;
				a.ClientName = clientName;
				await appClient.WriteAsync(a);
			}
		}

		public override async Task reserve(IAsyncStreamReader<ReserveRequest> requestStream,
			IServerStreamWriter<ReserveResponse> responseStream, ServerCallContext context) {
			while (await requestStream.MoveNext()) {
				var value = requestStream.Current;
				if (value.Type == AppService.Type.StartConnection && !_observers.ContainsKey(value.UserID)) {
					_observers[value.UserID] = responseStream;
				} else if (value.Type == AppService.Type.EndConnection && _observers.ContainsKey(value.UserID)) {
					_observers.Remove(value.UserID);
				} else {
					Trip trip = _tripRepo.FindOne(value.TripID);
					if (trip == null) {
						var a = new ReserveResponse();
						a.Type = ResponseType.Error;
						a.ErrorMessage = "No trip with the given id";
						await responseStream.WriteAsync(a);
						return;
					}

					BookedTrip bookedTrip = _bookedTripRepo.FindOne(new BookedTripID(value.TripID, value.SeatNumber));
					if (bookedTrip != null) {
						var a = new ReserveResponse();
						a.Type = ResponseType.Error;
						a.ErrorMessage = "Seat is already booked for the specified trip";
						await responseStream.WriteAsync(a);
						return;
					}

					Client client = _clientRepo.FindByName(value.ClientName);
					if (client == null) {
						_clientRepo.Save(new Client(-1, value.ClientName));
						client = _clientRepo.FindByName(value.ClientName);
					}

					Destination destination = _destinationRepo.FindOne(trip.DestinationId);

					_bookedTripRepo.Save(new BookedTrip(new BookedTripID(value.TripID, value.SeatNumber), client.Id));
					trip.FreeSeats--;
					_tripRepo.Update(trip);
					try {
						NotifyUsers(destination.Name, trip.Departure, value.SeatNumber, value.ClientName);
					}
					catch (Exception e) {
						Console.WriteLine(e);
						throw;
					}
				}
			}
		}

		public override Task<ShowTripsResponse> showTrips(ShowTripsRequest request, ServerCallContext context) {
			var showTripsResponse = new ShowTripsResponse();
			foreach (TripDTO tripDTO in _tripRepo.GetAllTrips()) {
				var trip = new AppService.TripDTO();
				try { 
					trip.Departure = SqlToGoogle(tripDTO.Departure);
				}
				catch (Exception e) {
					Console.WriteLine(e);
				}
				
				trip.DestinationName = tripDTO.DestinationName;
				trip.FreeSeats = tripDTO.FreeSeats;
				showTripsResponse.List.Add(trip);
			}

			return Task.FromResult(showTripsResponse);
		}

		public override Task<GetTripIDByDestinationAndDepartureResponse> getTripIDByDestinationAndDeparture(
			GetTripIDByDestinationAndDepartureRequest request, ServerCallContext context) {
			var getTripIDByDestinationAndDepartureResponse = new GetTripIDByDestinationAndDepartureResponse();
			getTripIDByDestinationAndDepartureResponse.TripID =
				_tripRepo.GetTripIdByDestinationAndDeparture(request.Destination, GoogleToSql(request.Departure)).Value;
			return Task.FromResult(getTripIDByDestinationAndDepartureResponse);
		}

		public override Task<LoginResponse> login(LoginRequest request, ServerCallContext context) {
			User user = _userRepo.FindByUsername(request.Username);
			if (user == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Wrong credentials"));
			if (_observers.ContainsKey(user.Id))
				throw new RpcException(new Status(StatusCode.Unavailable, "User already logged in"));
			if (User.Hash(request.Password) == user.PasswordHash) {
				var login = new LoginResponse();
				login.Id = user.Id;
				login.Username = user.Username;
				login.PasswordHash = user.PasswordHash;
				return Task.FromResult(login);
			}

			throw new RpcException(new Status(StatusCode.NotFound, "Wrong credentials"));
		}

		public override Task<LogoutResponse> logout(LogoutRequest request, ServerCallContext context) {
			if (_observers.ContainsKey(request.UserID)) {
				var logout = new LogoutResponse();
				logout.UserID = request.UserID;
				return Task.FromResult(logout);
			}

			throw new RpcException(new Status(StatusCode.NotFound, "User isn't logged in"));
		}
	}
}