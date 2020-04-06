using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
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

		public void Run() {
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
				lock (_server) {
					try { return new LoginResponse(_server.Login(userDTO.Username, userDTO.Password, this)); }
					catch (AppServiceException e) {
						_connected = false;
						return new ErrorResponse(e.Message);
					}
				}
			}

			if (request is LogoutRequest) {
				Console.WriteLine("Logout request ...");
				int userID = ((LogoutRequest) request).DTO;
				lock (_server) {
					try {
						_server.Logout(userID);
						_connected = false;
						return new LogoutResponse();
					}
					catch (AppServiceException e) { return new ErrorResponse(e.Message); }
				}
			}

			if (request is GetTripsRequest) {
				Console.WriteLine("GetTrips request ...");
				lock (_server) {
					try { return new GetTripsResponse(_server.ShowTrips()); }
					catch (AppServiceException e) { return new ErrorResponse(e.Message); }
				}
			}

			if (request is GetBookedTripsRequest) {
				Console.WriteLine("GetBookedTrips request ...");
				NetTripDTO netTripDTO = ((GetBookedTripsRequest) request).DTO;
				lock (_server) {
					try { return new GetBookedTripsResponse(_server.Search(netTripDTO.DestinationName, netTripDTO.Departure)); }
					catch (AppServiceException e) { return new ErrorResponse(e.Message); }
				}
			}

			if (request is GetBookedTripRequest) {
				Console.WriteLine("GetBookedTrip request ...");
				NetBookedTripDTO netTripDTO = ((GetBookedTripRequest) request).DTO;
				lock (_server) {
					try { return new GetBookedTripResponse(_server.FindClientId(netTripDTO.TripId, netTripDTO.SeatNumber)); }
					catch (AppServiceException e) { return new ErrorResponse(e.Message); }
				}
			}

			if (request is GetBookedTripIDRequest) {
				Console.WriteLine("GetBookedTrip request ...");
				NetTripDTO netTripDTO = ((GetBookedTripIDRequest) request).DTO;
				lock (_server) {
					try {
						return new GetBookedTripIDResponse(
							_server.GetTripIdByDestinationAndDeparture(netTripDTO.DestinationName, netTripDTO.Departure));
					}
					catch (AppServiceException e) { return new ErrorResponse(e.Message); }
				}
			}

			if (request is ReserveSeatRequest) {
				Console.WriteLine("GetBookedTrip request ...");
				NetReservedDTO netReservedDTO = ((ReserveSeatRequest) request).DTO;
				lock (_server) {
					try {
						_server.Reserve(netReservedDTO.TripId, netReservedDTO.ClientName, netReservedDTO.SeatNumber);
						return new ReserveSeatResponse();
					}
					catch (AppServiceException e) { return new ErrorResponse(e.Message); }
				}
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