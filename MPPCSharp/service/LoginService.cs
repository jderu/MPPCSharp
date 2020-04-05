using System;
using MPPCSharp.domain;
using MPPCSharp.repository;

namespace MPPCSharp.service {
	public class LoginService {
		private IUserRepository _users;
		public LoginService(IUserRepository users) { _users = users; }

		public User Login(String username, String password) {
			User user = _users.FindByUsername(username);
			if (user == null || User.Hash(password) == user.PasswordHash)
				return user;
			return null;
		}
	}
}