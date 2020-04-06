using System;

namespace networking.dto {
	[Serializable]public class UserDTO {
		string _username;
		string _password;

		public UserDTO(string username, string password) {
			_username = username;
			_password = password;
		}

		public string Username { get => _username; set => _username = value; }
		public string Password { get => _password; set => _password = value; }
	}
}