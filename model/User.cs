using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace model {
	[Serializable] public class User : Entity<int> {
		public string Username { get; set; }
		public string PasswordHash { get; set; }

		public User(int id, string username, string passwordHash) : base(id) {
			Username = username;
			PasswordHash = passwordHash;
		}

		public static string Hash(string password) {
			using SHA256 hash = SHA256.Create();
			return string.Concat(values: hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(item => item.ToString("x2")));
		}
	}
}