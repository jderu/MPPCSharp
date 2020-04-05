using System;
using System.Collections.Generic;
using MPPCSharp.domain;
using Npgsql;

namespace MPPCSharp.repository.database {
	public abstract class AbstractDatabaseRepository<TId, TE> : ICrudRepository<TId, TE> where TE : Entity<TId> {
		protected readonly JdbcUtils DbUtils;
		protected Type ClassType { get; set; }

		protected AbstractDatabaseRepository(JdbcUtils dbUtils) { DbUtils = dbUtils; }

		protected abstract TE ReadEntity(NpgsqlDataReader result);

		protected abstract string FindOneString(TId id);

		public TE FindOne(TId id) {
			ICrudRepository<TId, TE>.Log.Info($"{ClassType} FindOne()");
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			TE entity = null;
			NpgsqlCommand cmd = new NpgsqlCommand(FindOneString(id), DbUtils.GetConnection());
			using NpgsqlDataReader rdr = cmd.ExecuteReader();
			if (rdr.Read())
				entity = ReadEntity(rdr);
			rdr.Close();

			return entity;
		}

		protected abstract string FindAllString();

		public List<TE> FindAll() {
			ICrudRepository<TId, TE>.Log.Info($"{ClassType} FindAll()");
			List<TE> entities = new List<TE>();
			NpgsqlCommand cmd = new NpgsqlCommand(FindAllString(), DbUtils.GetConnection());

			using NpgsqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
				entities.Add(ReadEntity(rdr));
			rdr.Close();
			return entities;
		}

		protected abstract string InsertString(TE entity);

		public TE Save(TE entity) {
			ICrudRepository<TId, TE>.Log.Info($"{ClassType} Save()");
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			NpgsqlCommand cmd = new NpgsqlCommand(InsertString(entity), DbUtils.GetConnection());
			if (cmd.ExecuteNonQuery() == -1)
				return entity;
			return null;
		}

		protected abstract string DeleteString(TId id);

		public TE Delete(TId id) {
			ICrudRepository<TId, TE>.Log.Info($"{ClassType} Delete()");
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			TE entity = FindOne(id);
			if (entity != null) {
				NpgsqlCommand cmd = new NpgsqlCommand(DeleteString(id), DbUtils.GetConnection());
				cmd.ExecuteNonQuery();
				return entity;
			}

			return null;
		}

		protected abstract string UpdateString(TE entity);

		public TE Update(TE entity) {
			ICrudRepository<TId, TE>.Log.Info($"{ClassType} Update()");
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			NpgsqlCommand cmd = new NpgsqlCommand(UpdateString(entity), DbUtils.GetConnection());
			if (cmd.ExecuteNonQuery() <= 0)
				return entity;
			return null;
		}
	}
}