using System;
using System.Data;
using MPPCSharp.domain;
using Npgsql;

namespace MPPCSharp.repository.database {
	public class DatabaseClientRepository : AbstractDatabaseRepository<int, Client>, IClientRepository {
		public DatabaseClientRepository(JdbcUtils dbUtils) : base(dbUtils) { ClassType = GetType(); }

		protected override Client ReadEntity(NpgsqlDataReader result) {
			int id = result.GetInt32("id");
			String name = result.GetString("name");
			return new Client(id, name);
		}

		protected override string FindOneString(int id) { return $"SELECT * from \"Client\" where ID = {id};"; }

		protected override string FindAllString() { return $"SELECT * from \"Client\";"; }

		protected override string InsertString(Client entity) { return $"INSERT INTO \"Client\" (name) VALUES ('{entity.Name}');"; }

		protected override string DeleteString(int id) { return $"DELETE from \"Client\" where ID = {id};"; }

		protected override string UpdateString(Client entity) {
			return $"UPDATE \"Client\" SET name= '{entity.Name}' where ID = {entity.Id};";
		}

		public Client FindByName(string name) {
			IClientRepository.Log.Info($"Find one {ClassType} {name}");
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			Client entity = null;
			NpgsqlCommand cmd = new NpgsqlCommand(FindByNameString(name), DbUtils.GetConnection());
			using NpgsqlDataReader rdr = cmd.ExecuteReader();
			if (rdr.Read())
				entity = ReadEntity(rdr);
			rdr.Close();

			return entity;
		}

		private string FindByNameString(string name) { return $"SELECT * from \"Client\" where name = '{name}';"; }
	}
}