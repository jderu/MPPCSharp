using System;
using System.Windows.Forms;
using persistence;
using persistence.database;
using services;

namespace client {
	static class Program {
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread] static void Main() {
			IChatServices server = new ChatServerProxy("127.0.0.1", 55555);
			ChatClientCtrl ctrl=new ChatClientCtrl(server);
			LoginWindow win=new LoginWindow(ctrl);
			Application.Run(win);
			
			
			
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