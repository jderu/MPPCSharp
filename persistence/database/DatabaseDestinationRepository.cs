using System;
using System.Data;
using model;
using Npgsql;

namespace persistence.database {
	public class DatabaseDestinationRepository : AbstractDatabaseRepository<int, Destination>, IDestinationRepository {
		public DatabaseDestinationRepository(JdbcUtils dbUtils) : base(dbUtils) { ClassType = GetType(); }

		protected override Destination ReadEntity(NpgsqlDataReader result) {
			int id = result.GetInt32("id");
			String name = result.GetString("name");
			return new Destination(id, name);
		}

		protected override string FindOneString(int id) { return $"SELECT * from \"Destination\" where ID = {id};"; }

		protected override string FindAllString() { return "SELECT * from \"Destination\";"; }

		protected override string InsertString(Destination entity) {
			return $"INSERT INTO \"Destination\" (name) VALUES ('{entity.Name}');";
		}

		protected override string DeleteString(int id) { return $"DELETE from \"Destination\" where ID = {id};"; }

		protected override string UpdateString(Destination entity) {
			return $"UPDATE \"Destination\" SET name= '{entity.Name}' where ID = {entity.Id};";
		}
	}
}