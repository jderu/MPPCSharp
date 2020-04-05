using System;
using System.Windows.Forms;
using MPPCSharp.repository;
using MPPCSharp.repository.database;
using MPPCSharp.service;

namespace MPPCSharp {
	static class Program {
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread] static void Main() {
			using JdbcUtils jdbcUtilsUser = new JdbcUtils("./UserDatabase.txt");
			
			IUserRepository userRepository = new DatabaseUserRepository(jdbcUtilsUser);
			LoginService loginService = new LoginService(userRepository);
			
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1(loginService));
		}
	}
}