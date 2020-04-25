using System;
using Grpc.Core;
using persistence;
using persistence.database;


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

			var serviceImpl = new ServerImpl(userRepo, bookedTripRepo, clientRepo, destinationRepo, tripRepo);

			var server = new Server {
				Services = {AppService.AppService.BindService(serviceImpl)},
				Ports = {new ServerPort("localhost", 50050, ServerCredentials.Insecure)}
			};
			
			server.Start();

			Console.WriteLine("RouteGuide server listening on port " + 50050);
			Console.WriteLine("Press any key to stop the server...");
			Console.ReadKey();

			server.ShutdownAsync().Wait();
		}
	}
}