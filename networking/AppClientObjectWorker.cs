using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using model;
using networking.dto;
using services;

namespace networking {
	///
	/// <summary> * Created by IntelliJ IDEA.
	/// * User: grigo
	/// * Date: Mar 18, 2009
	/// * Time: 4:04:43 PM </summary>
	/// 
	public class AppClientWorker : IAppObserver //, Runnable
	{
		private IAppServices _server;
		private TcpClient _connection;

		private NetworkStream _stream;
		private IFormatter _formatter;
		private volatile bool _connected;

		public AppClientWorker(IAppServices server, TcpClient connection) {
			this._server = server;
			this._connection = connection;
			try {
				_stream = connection.GetStream();
				_formatter = new BinaryFormatter();
				_connected = true;
			}
			catch (Exception e) { Console.WriteLine(e.StackTrace); }
		}

		public virtual void Run() {
			while (_connected) {
				try {
					object request = _formatter.Deserialize(_stream);
					object response = HandleRequest((IRequest) request);
					if (response != null) { SendResponse((IResponse) response); }
				}
				catch (Exception e) { Console.WriteLine(e.StackTrace); }

				try { Thread.Sleep(1000); }
				catch (Exception e) { Console.WriteLine(e.StackTrace); }
			}

			try {
				_stream.Close();
				_connection.Close();
			}
			catch (Exception e) { Console.WriteLine("Error " + e); }
		}

		private IResponse HandleRequest(IRequest request) {
			IResponse response = null;

			if (request is LoginRequest) {
				Console.WriteLine("Login request ...");
				UserDTO userDTO = ((LoginRequest) request).DTO;
				try {
					User user;
					lock (_server) { user = _server.Login(userDTO.Username, userDTO.Password, this); }

					return new LoginResponse(user);
				}
				catch (AppServiceException e) {
					_connected = false;
					return new ErrorResponse(e.Message);
				}
			}

			if (request is LogoutRequest) {
				Console.WriteLine("Logout request ...");
				int userID = ((LogoutRequest) request).DTO;
				try {
					lock (_server) { _server.Logout(userID); }

					_connected = false;
					return new LogoutResponse();
				}
				catch (AppServiceException e) { return new ErrorResponse(e.Message); }
			}

			if (request is GetTripsRequest) {
				Console.WriteLine("GetTrips request ...");
				try {
					List<TripDTO> trips;
					lock (_server) { trips = _server.ShowTrips(); }

					return new GetTripsResponse(trips);
				}
				catch (AppServiceException e) { return new ErrorResponse(e.Message); }
			}

			if (request is GetBookedTripsRequest) {
				Console.WriteLine("GetBookedTrips request ...");
				NetTripDTO netTripDTO = ((GetBookedTripsRequest) request).DTO;
				try {
					List<BookedTripDTO> trips;
					lock (_server) { trips = _server.Search(netTripDTO.DestinationName, netTripDTO.Departure); }

					return new GetBookedTripsResponse(trips);
				}
				catch (AppServiceException e) { return new ErrorResponse(e.Message); }
			}

			if (request is GetBookedTripRequest) {
				Console.WriteLine("GetBookedTrip request ...");
				NetBookedTripDTO netTripDTO = ((GetBookedTripRequest) request).DTO;
				try {
					BookedTrip trip;
					lock (_server) { trip = _server.FindClientId(netTripDTO.TripId, netTripDTO.SeatNumber); }

					return new GetBookedTripResponse(trip);
				}
				catch (AppServiceException e) { return new ErrorResponse(e.Message); }
			}

			if (request is GetBookedTripIDRequest) {
				Console.WriteLine("GetBookedTrip request ...");
				NetTripDTO netTripDTO = ((GetBookedTripIDRequest) request).DTO;
				try {
					int? id;
					lock (_server) {
						id = _server.GetTripIdByDestinationAndDeparture(netTripDTO.DestinationName, netTripDTO.Departure);
					}

					return new GetBookedTripIDResponse(id);
				}
				catch (AppServiceException e) { return new ErrorResponse(e.Message); }
			}

			if (request is ReserveSeatRequest) {
				Console.WriteLine("GetBookedTrip request ...");
				NetReservedDTO netReservedDTO = ((ReserveSeatRequest) request).DTO;
				try {
					lock (_server) { _server.Reserve(netReservedDTO.TripId, netReservedDTO.ClientName, netReservedDTO.SeatNumber); }

					return new ReserveSeatResponse();
				}
				catch (AppServiceException e) { return new ErrorResponse(e.Message); }
			}

			return response;
		}

		private void SendResponse(IResponse response) {
			Console.WriteLine("sending response " + response);
			_formatter.Serialize(_stream, response);
			_stream.Flush();
		}

		public void UpdateWindows(string destinationName, DateTime departure, int seatNumber, string clientName) {
			try { SendResponse(new ReservedResponse(new NetUpdateDTO(destinationName, departure, seatNumber, clientName))); }
			catch (IOException e) { Console.WriteLine(e.StackTrace); }
		}
	}
}