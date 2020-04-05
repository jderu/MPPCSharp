using System;
using System.Data;
using Npgsql;

namespace MPPCSharp.repository {
	public class JdbcUtils : IDisposable {
		private readonly string _filePath;
		private NpgsqlConnection _instance;

		public JdbcUtils(string filePath) { _filePath = filePath; }

		private NpgsqlConnection GetNewConnection() {
			System.IO.StreamReader file = new System.IO.StreamReader(_filePath);
			string props = file.ReadLine();
			file.Close();
			NpgsqlConnection con = new NpgsqlConnection(props);
			con.Open();
			return con;
		}

		public NpgsqlConnection GetConnection() {
			if (_instance == null || _instance.State == ConnectionState.Closed)
				_instance = GetNewConnection();
			return _instance;
		}

		public void Dispose() { _instance?.Dispose(); }
	}
}