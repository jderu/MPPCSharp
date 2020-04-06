using System;
using System.IO;
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
	/// * Time: 4:04:43 PM </summary>
	/// 
	public class ChatClientWorker : IAppObserver //, Runnable
	{
		private IAppServices _server;
		private TcpClient _connection;

		private NetworkStream _stream;
		private IFormatter _formatter;
		private volatile bool _connected;

		public ChatClientWorker(IAppServices server, TcpClient connection) {
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

		public virtual void MessageReceived(Message message) {
			MessageDTO mdto = DTOUtils.getDTO(message);
			Console.WriteLine("Message received  " + message);
			try { SendResponse(new NewMessageResponse(mdto)); }
			catch (Exception e) { throw new ChatException("Sending error: " + e); }
		}

		public virtual void FriendLoggedIn(User friend) {
			UserDto udto = DTOUtils.getDTO(friend);
			Console.WriteLine("Friend logged in " + friend);
			try { SendResponse(new FriendLoggedInResponse(udto)); }
			catch (Exception e) { Console.WriteLine(e.StackTrace); }
		}

		public virtual void FriendLoggedOut(User friend) {
			UserDto udto = DTOUtils.getDTO(friend);
			Console.WriteLine("Friend logged out " + friend);
			try { SendResponse(new FriendLoggedOutResponse(udto)); }
			catch (Exception e) { Console.WriteLine(e.StackTrace); }
		}

		private IResponse HandleRequest(IRequest request) {
			IResponse response = null;
			if (request is LoginRequest) {
				Console.WriteLine("Login request ...");
				LoginRequest logReq = (LoginRequest) request;
				UserDto udto = logReq.User;
				User user = DTOUtils.getFromDTO(udto);
				try {
					lock (_server) { _server.login(user, this); }

					return new OkResponse();
				}
				catch (ChatException e) {
					_connected = false;
					return new ErrorResponse(e.Message);
				}
			}

			if (request is LogoutRequest) {
				Console.WriteLine("Logout request");
				LogoutRequest logReq = (LogoutRequest) request;
				UserDto udto = logReq.User;
				User user = DTOUtils.getFromDTO(udto);
				try {
					lock (_server) { _server.logout(user, this); }

					_connected = false;
					return new OkResponse();
				}
				catch (ChatException e) { return new ErrorResponse(e.Message); }
			}

			if (request is SendMessageRequest) {
				Console.WriteLine("SendMessageRequest ...");
				SendMessageRequest senReq = (SendMessageRequest) request;
				MessageDTO mdto = senReq.Message;
				Message message = DTOUtils.getFromDTO(mdto);
				try {
					lock (_server) { _server.sendMessage(message); }

					return new OkResponse();
				}
				catch (ChatException e) { return new ErrorResponse(e.Message); }
			}

			if (request is GetLoggedFriendsRequest) {
				Console.WriteLine("GetLoggedFriends Request ...");
				GetLoggedFriendsRequest getReq = (GetLoggedFriendsRequest) request;
				UserDto udto = getReq.User;
				User user = DTOUtils.getFromDTO(udto);
				try {
					User[] friends;
					lock (_server) { friends = _server.getLoggedFriends(user); }

					UserDto[] frDTO = DTOUtils.getDTO(friends);
					return new GetLoggedFriendsResponse(frDTO);
				}
				catch (ChatException e) { return new ErrorResponse(e.Message); }
			}

			return response;
		}

		private void SendResponse(IResponse response) {
			Console.WriteLine("sending response " + response);
			_formatter.Serialize(_stream, response);
			_stream.Flush();
		}

		public void UpdateWindows(string destinationName, DateTime departure, int seatNumber, string clientName) {
			IResponse resp = new Response.Builder().type(ResponseType.RESERVED)
				.data(new NetUpdateDTO(destinationName, departure, seatNumber, clientName)).build();
			try { SendResponse(new ReservedResponse(new NetUpdateDTO(destinationName, departure, seatNumber, clientName))); }
			catch (IOException e) { Console.WriteLine(e.StackTrace); }
		}
	}
}