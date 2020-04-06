using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using model;
using networking.dto;
using services;

namespace networking {
	///
	/// <summary> * Created by IntelliJ IDEA.
	/// * User: grigo
	/// * Date: Mar 18, 2009
	/// * Time: 4:36:34 PM </summary>
	/// 
	public class ChatServerProxy : IAppServices {
		private string _host;
		private int _port;

		private IAppObserver _client;

		private NetworkStream _stream;

		private IFormatter _formatter;
		private TcpClient _connection;

		private Queue<IResponse> _responses;
		private volatile bool _finished;
		private EventWaitHandle _waitHandle;

		public ChatServerProxy(string host, int port) {
			this._host = host;
			this._port = port;
			_responses = new Queue<IResponse>();
		}


		private void CloseConnection() {
			_finished = true;
			try {
				_stream.Close();
				//output.close();
				_connection.Close();
				_waitHandle.Close();
				_client = null;
			}
			catch (Exception e) { Console.WriteLine(e.StackTrace); }
		}

		private void SendRequest(IRequest request) {
			try {
				_formatter.Serialize(_stream, request);
				_stream.Flush();
			}
			catch (Exception e) { throw new AppServiceException("Error sending object " + e); }
		}

		private IResponse ReadResponse() {
			IResponse response = null;
			try {
				_waitHandle.WaitOne();
				lock (_responses) {
					//Monitor.Wait(responses); 
					response = _responses.Dequeue();
				}
			}
			catch (Exception e) { Console.WriteLine(e.StackTrace); }

			return response;
		}

		private void InitializeConnection() {
			try {
				_connection = new TcpClient(_host, _port);
				_stream = _connection.GetStream();
				_formatter = new BinaryFormatter();
				_finished = false;
				_waitHandle = new AutoResetEvent(false);
				StartReader();
			}
			catch (Exception e) { Console.WriteLine(e.StackTrace); }
		}

		private void StartReader() {
			Thread tw = new Thread(Run);
			tw.Start();
		}


		private void HandleUpdate(IUpdateResponse update) {
			if (update is ReservedResponse) {
				NetUpdateDTO netUpdateDTO = update as NetUpdateDTO;
				_client.UpdateWindows(netUpdateDTO.DestinationName, netUpdateDTO.Departure, netUpdateDTO.SeatNumber,
					netUpdateDTO.ClientName);
			}
		}

		public virtual void Run() {
			while (!_finished) {
				try {
					object response = _formatter.Deserialize(_stream);
					Console.WriteLine("response received " + response);
					if (response is IUpdateResponse updateResponse) { HandleUpdate(updateResponse); } else {
						lock (_responses) { _responses.Enqueue((IResponse) response); }

						_waitHandle.Set();
					}
				}
				catch (Exception e) { Console.WriteLine("Reading error " + e); }
			}
		}

		public List<BookedTripDTO> Search(string destinationName, DateTime departure) {
			SendRequest(new GetBookedTripsRequest(new NetTripDTO(destinationName, departure)));
			IResponse response = ReadResponse();
			if (response is ErrorResponse errorResponse)
				throw new AppServiceException(errorResponse.Message);

			return ((GetBookedTripsResponse) response).DTO;
		}

		public BookedTrip FindClientId(int tripId, int seatNumber) {
			SendRequest(new GetBookedTripRequest(new NetBookedTripDTO(tripId, seatNumber)));
			IResponse response = ReadResponse();
			if (response is ErrorResponse errorResponse)
				throw new AppServiceException(errorResponse.Message);
			return ((GetBookedTripResponse) response).DTO;
		}

		public void Reserve(int tripId, string clientName, int seatNumber) {
			SendRequest(new ReserveSeatRequest(new NetReservedDTO(tripId, clientName, seatNumber)));
			IResponse response = ReadResponse();
			if (response is ErrorResponse errorResponse)
				throw new AppServiceException(errorResponse.Message);
		}

		public List<TripDTO> ShowTrips() {
			SendRequest(new GetTripsRequest());
			IResponse response = ReadResponse();
			if (response is ErrorResponse errorResponse)
				throw new AppServiceException(errorResponse.Message);
			return ((GetTripsResponse) response).DTO;
		}

		public int? GetTripIdByDestinationAndDeparture(string destination, DateTime departure) {
			SendRequest(new GetBookedTripIDRequest(new NetTripDTO(destination, departure)));
			IResponse response = ReadResponse();
			if (response is ErrorResponse errorResponse)
				throw new AppServiceException(errorResponse.Message);
			return ((GetBookedTripIDResponse) response).DTO;
		}

		public User Login(string username, string password, IAppObserver client) {
			InitializeConnection();
			SendRequest(new LoginRequest(new UserDTO(username, password)));
			IResponse response = ReadResponse();
			if (response is LoginResponse loginResponse) {
				this._client = client;
				return loginResponse.DTO;
			}

			CloseConnection();
			throw new AppServiceException(((ErrorResponse) response).Message);
		}

		public void Logout(int userId) {
			SendRequest(new LogoutRequest(userId));
			IResponse response = ReadResponse();
			CloseConnection();
			if (response is ErrorResponse errorResponse)
				throw new AppServiceException(errorResponse.Message);
		}
	}
}