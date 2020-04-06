using System;
using System.Data;
using model;
using Npgsql;

namespace persistence.database {
	public class DatabaseUserRepository : AbstractDatabaseRepository<int, User>, IUserRepository {
		public DatabaseUserRepository(JdbcUtils dbUtils) : base(dbUtils) { ClassType = GetType(); }

		protected override User ReadEntity(NpgsqlDataReader result) {
			int id = result.GetInt32("id");
			String username = result.GetString("username");
			String passwordHash = result.GetString("passwordHash");
			return new User(id, username, passwordHash);
		}

		protected override string FindOneString(int id) { return $"SELECT * from \"User\" where ID = {id};"; }

		protected override string FindAllString() { return $"SELECT * from \"User\";"; }

		protected override string InsertString(User entity) {
			return $"INSERT INTO \"User\" (username, \"passwordHash\") VALUES ('{entity.Username}','{entity.PasswordHash}');";
		}

		protected override string DeleteString(int id) { return "DELETE from \"User\" where ID = " + id + ";"; }

		protected override string UpdateString(User entity) {
			return "UPDATE \"User\" SET " + "username= '" + entity.Username + "passwordHash= '" + entity.PasswordHash +
			       "' where ID = " + entity.Id + ";";
		}

		private string FindByUsernameString(String username) { return $"SELECT * from \"User\" where username = '{username}';"; }

		public User FindByUsername(string username) {
			IUserRepository.Log.Info("Find one " + ClassType + " " + username);
			if (username == null)
				throw new ArgumentNullException(nameof(username));
			User entity = null;
			NpgsqlCommand cmd = new NpgsqlCommand(FindByUsernameString(username), DbUtils.GetConnection());
			using NpgsqlDataReader rdr = cmd.ExecuteReader();
			if (rdr.Read())
				entity = ReadEntity(rdr);

			return entity;
		}
	}
}