using System;
using System.Windows.Forms;
using MPPCSharp.domain;
using MPPCSharp.repository;
using MPPCSharp.repository.database;
using MPPCSharp.service;

namespace MPPCSharp {
	public partial class Form1 : Form {
		private readonly LoginService _loginService;

		public Form1(LoginService loginService) {
			_loginService = loginService;
			InitializeComponent();
		}


		private void button1_Click(object sender, EventArgs e) {
			string username = usernameTextBox.Text;
			string password = passwordTextBox.Text;
			User user = _loginService.Login(username, password);
			if (user != null) {
				using JdbcUtils jdbcUtilsApp = new JdbcUtils("./AppDatabase.txt");

				IBookedTripRepository bookedTripRepository = new DatabaseBookedTripRepository(jdbcUtilsApp);
				IClientRepository clientRepository = new DatabaseClientRepository(jdbcUtilsApp);
				IDestinationRepository destinationRepository = new DatabaseDestinationRepository(jdbcUtilsApp);
				ITripRepository tripRepository = new DatabaseTripRepository(jdbcUtilsApp);
				AppService appService = new AppService(bookedTripRepository, clientRepository, destinationRepository, tripRepository);

				this.Hide();
				Form2 form2 = new Form2(appService);
				form2.ShowDialog();
			} else { MessageBox.Show("Wrong username and password"); }
		}
	}
}