using System;
using System.Net.Sockets;
using System.Threading;
using networking;
using persistence;
using persistence.database;
using services;

namespace server {
	class StartServer {
		static void Main(string[] args) {
			using JdbcUtils jdbcUtilsUser = new JdbcUtils("./UserDatabase.txt");
			using JdbcUtils jdbcUtilsApp = new JdbcUtils("./AppDatabase.txt");

			IUserRepository userRepo = new DatabaseUserRepository(jdbcUtilsUser);
			IBookedTripRepository bookedTripRepo = new DatabaseBookedTripRepository(jdbcUtilsApp);
			IClientRepository clientRepo = new DatabaseClientRepository(jdbcUtilsApp);
			IDestinationRepository destinationRepo = new DatabaseDestinationRepository(jdbcUtilsApp);
			ITripRepository tripRepo = new DatabaseTripRepository(jdbcUtilsApp);

			IAppServices serviceImpl = new Server(userRepo, bookedTripRepo, clientRepo, destinationRepo, tripRepo);

			SerialChatServer server = new SerialChatServer("127.0.0.1", 55555, serviceImpl);
			server.Start();
			Console.WriteLine("Server started ...");
		}
	}

	public class SerialChatServer : ConcurrentServer {
		private IAppServices server;
		private AppClientWorker worker;

		public SerialChatServer(string host, int port, IAppServices server) : base(host, port) {
			this.server = server;
			Console.WriteLine("SerialChatServer...");
		}

		protected override Thread CreateWorker(TcpClient client) {
			worker = new AppClientWorker(server, client);
			return new Thread(worker.Run);
		}
	}
}