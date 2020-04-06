using System;
using System.Collections.Generic;
using System.Data;
using model;
using Npgsql;

namespace persistence.database {
	public class DatabaseBookedTripRepository : AbstractDatabaseRepository<BookedTripID, BookedTrip>, IBookedTripRepository {
		public DatabaseBookedTripRepository(JdbcUtils dbUtils) : base(dbUtils) { ClassType = GetType(); }

		protected override BookedTrip ReadEntity(NpgsqlDataReader result) {
			int tripId = result.GetInt32("tripID");
			int seatNumber = result.GetInt32("seatNumber");
			int clientId = result.GetInt32("clientID");
			return new BookedTrip(tripId, seatNumber, clientId);
		}

		protected override string FindOneString(BookedTripID id) {
			return $"SELECT * from \"BookedTrip\" where \"tripID\" = {id.TripId} and \"seatNumber\" = {id.SeatNumber};";
		}

		protected override string FindAllString() { return "SELECT * from \"BookedTrip\";"; }

		protected override string InsertString(BookedTrip entity) {
			return
				$"INSERT INTO \"BookedTrip\" (\"tripID\", \"seatNumber\", \"clientID\") VALUES ({entity.TripId},{entity.SeatNumber},{entity.ClientId});";
		}

		protected override string DeleteString(BookedTripID id) {
			return $"DELETE from \"BookedTrip\" where \"tripID\" = {id.TripId} and \"seatNumber\" = {id.SeatNumber};";
		}


		protected override string UpdateString(BookedTrip entity) {
			return
				$"UPDATE \"BookedTrip\" SET \"clientID\"= {entity.ClientId} where \"tripID\" = {entity.TripId} and \"seatNumber\" = {entity.SeatNumber};";
		}

		private String FindByDestinationAndDeparture(string destinationName, DateTime departure) {
			return
				$"SELECT \"Client\".\"id\", \"Client\".\"name\", \"BookedTrip\".\"seatNumber\" from \"BookedTrip\" inner join \"Client\" on \"BookedTrip\".\"clientID\" = \"Client\".\"id\" inner join \"Trip\" on \"BookedTrip\".\"tripID\" = \"Trip\".\"id\" inner join \"Destination\" on \"Trip\".\"destinationID\"=\"Destination\".\"id\" where \"Destination\".\"name\" = '{destinationName}' and \"Trip\".\"departure\" = '{departure}';";
		}

		private BookedTripDTO ReadDTO(NpgsqlDataReader result) {
			int clientId = result.GetInt32("id");
			String clientName = result.GetString("name");
			int seatNumber = result.GetInt32("seatNumber");
			return new BookedTripDTO(clientId, clientName, seatNumber);
		}

		public List<BookedTripDTO> Search(string destinationName, DateTime departure) {
			IBookedTripRepository.Log.Info($"{GetType()} Search()");

			List<BookedTripDTO> entities = new List<BookedTripDTO>();
			NpgsqlCommand cmd = new NpgsqlCommand(FindByDestinationAndDeparture(destinationName, departure), DbUtils.GetConnection());
			using NpgsqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
				entities.Add(ReadDTO(rdr));
			rdr.Close();
			return entities;
		}
	}
}