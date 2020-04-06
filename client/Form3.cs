using System;
using System.Collections.Generic;
using System.Windows.Forms;
using model;
using services;

namespace client {
	public partial class Form3 : Form {
		private readonly int _tripId;
		private readonly AppService _appService;
		private List<BookedTripDTO> _data;

		public Form3(int tripId, List<BookedTripDTO> result, AppService appService) {
			_tripId = tripId;
			_appService = appService;
			InitializeComponent();
			_data = appService.CreateList(result);
			foreach (BookedTripDTO bookedTripDto in _data) { table.Rows.Add(bookedTripDto.SeatNumber, bookedTripDto.ClientName); }
		}

		private void button1_Click(object sender, EventArgs e) {
			string name = clientNameTextBox.Text;
			int seatNumber = int.Parse(seatNumberTextBox.Text);
			try {
				_appService.Reserve(_tripId, name, seatNumber);
				table.Rows[seatNumber - 1].SetValues(seatNumber, name);
			}
			catch (AppServiceException exception) { MessageBox.Show(exception.Message); }
		}
	}
}