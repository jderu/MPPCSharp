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
			// Application.SetHighDpiMode(HighDpiMode.SystemAware);
			// Application.EnableVisualStyles();
			// Application.SetCompatibleTextRenderingDefault(false);
			//
			// IAppServices server = new AppProxyService("127.0.0.1", 55555);
			// Application.Run(new Form1(server));
		}
	}
}