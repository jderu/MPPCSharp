using System;
using System.Windows.Forms;
using model;
using persistence;
using persistence.database;
using services;

namespace client {
	public partial class Form1 : Form, IAppObserver {
		private IAppServices _service;
		private Form2 form2;

		public Form1(IAppServices service) {
			_service = service;
			InitializeComponent();
		}


		private void button1_Click(object sender, EventArgs e) {
			string username = usernameTextBox.Text;
			string password = passwordTextBox.Text;
			try {
				User user = _service.Login(username, password, this);
				if (user != null) {
					this.Hide();
					form2 = new Form2(_service, user);
					form2.ShowDialog();
				} else { MessageBox.Show("Wrong username and password"); }
			}
			catch (AppServiceException ex) { MessageBox.Show(ex.Message); }
		}

		public void UpdateWindows(string destinationName, DateTime departure, int seatNumber, string clientName) {
			Console.WriteLine("loginController");
			form2.UpdateWindows(destinationName, departure, seatNumber, clientName);
		}
	}
}