using System;
using System.Collections.Generic;
using System.Windows.Forms;
using model;
using services;

namespace client {
	public partial class Form2 : Form, IAppObserver {
		private IAppServices _appService;
		private readonly User _user;
		private List<TripDTO> _data;
		private Form3 form3;

		public Form2(IAppServices appService, User user) {
			_appService = appService;
			_user = user;
			InitializeComponent();
			_data = _appService.ShowTrips();
			foreach (TripDTO tripDto in _data) { table.Rows.Add(tripDto.DestinationName, tripDto.Departure, tripDto.FreeSeats); }
		}

		private void table_CellClick(object sender, DataGridViewCellEventArgs e) {
			string destinationName = table.SelectedRows[0].Cells[0].Value.ToString();
			DateTime departure = DateTime.Parse(table.SelectedRows[0].Cells[1].Value.ToString());
			int? tripId = _appService.GetTripIdByDestinationAndDeparture(destinationName, departure);
			if (tripId != null) {
				form3 = new Form3(_appService, _user, tripId.Value, destinationName, departure);
				form3.ShowDialog();
			}
		}

		public void UpdateWindows(string destinationName, DateTime departure, int seatNumber, string clientName) {
			Console.WriteLine("AppController");
			foreach (TripDTO trip in _data)
				if (trip.Departure == departure && trip.DestinationName == destinationName) {
					trip.FreeSeats--;
					break;
				}

			//table.Refresh();
			if (form3 != null && form3.Departure == departure && form3.Destination == destinationName)
				form3.UpdateWindows(destinationName, departure, seatNumber, clientName);
		}
	}
}