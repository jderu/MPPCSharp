using System;
using System.Collections.Generic;
using System.Data;
using model;
using Npgsql;

namespace persistence.database {
	public class DatabaseTripRepository : AbstractDatabaseRepository<int, Trip>, ITripRepository {
		public DatabaseTripRepository(JdbcUtils dbUtils) : base(dbUtils) { ClassType = GetType(); }

		protected override Trip ReadEntity(NpgsqlDataReader result) {
			int id = result.GetInt32("id");
			int destinationId = result.GetInt32("destinationID");
			DateTime departure = result.GetDateTime("departure");
			int freeSeats = result.GetInt32("freeSeats");
			return new Trip(id, destinationId, departure, freeSeats);
		}

		protected override string FindOneString(int id) { return $"SELECT * from \"Trip\" where ID = {id};"; }

		protected override string FindAllString() { return "SELECT * from \"Trip\";"; }

		protected override string InsertString(Trip entity) {
			return
				$"INSERT INTO \"Trip\" (\"destinationID\", \"departure\", \"freeSeats\") VALUES ({entity.DestinationId},'{entity.Departure}',{entity.FreeSeats});";
		}

		protected override string DeleteString(int id) { return $"DELETE from \"Trip\" where ID = {id};"; }

		protected override string UpdateString(Trip entity) {
			return
				$"UPDATE \"Trip\" SET \"destinationID\"= {entity.DestinationId}, \"departure\"= '{entity.Departure}', \"freeSeats\"= {entity.FreeSeats} where ID = {entity.Id};";
		}

		private string GetAllTripsString() {
			return "SELECT \"name\", \"departure\", \"freeSeats\"" +
			       "FROM \"Trip\" inner join \"Destination\" on \"Trip\".\"destinationID\"=\"Destination\".\"id\"";
		}

		private TripDTO ReadDTO(NpgsqlDataReader result) {
			String destinationName = result.GetString("name");
			DateTime departure = result.GetDateTime("departure");
			int freeSeats = result.GetInt32("freeSeats");
			return new TripDTO(destinationName, departure, freeSeats);
		}

		public List<TripDTO> GetAllTrips() {
			ITripRepository.Log.Info($"{ClassType} GetAllTrips()");
			List<TripDTO> entities = new List<TripDTO>();
			NpgsqlCommand cmd = new NpgsqlCommand(GetAllTripsString(), DbUtils.GetConnection());
			using NpgsqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
				entities.Add(ReadDTO(rdr));
			rdr.Close();

			return entities;
		}

		private String GetTripByDestinationAndDeparture(String destinationName, DateTime departure) {
			return
				$"SELECT \"Trip\".\"id\" as \"id\"from \"Trip\" inner join \"Destination\" on \"Trip\".\"destinationID\"=\"Destination\".\"id\" where \"Destination\".\"name\" = '{destinationName}' and \"Trip\".\"departure\" = '{departure}';";
		}

		public int? GetTripIdByDestinationAndDeparture(string destinationName, DateTime departure) {
			ITripRepository.Log.Info($"{ClassType} GetTripIdByDestinationAndDeparture()");

			NpgsqlCommand cmd =
				new NpgsqlCommand(GetTripByDestinationAndDeparture(destinationName, departure), DbUtils.GetConnection());
			using NpgsqlDataReader rdr = cmd.ExecuteReader();
			if (rdr.Read())
				return rdr.GetInt32("id");
			rdr.Close();

			return null;
		}
	}
}