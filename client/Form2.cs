using System;
using System.Collections.Generic;
using System.Windows.Forms;
using model;
using services;

namespace client {
	public partial class Form2 : Form {
		private readonly AppService _appService;
		private List<TripDTO> _data;

		public Form2(AppService appService) {
			_appService = appService;
			InitializeComponent();
			_data = _appService.ShowTrips();
			foreach (TripDTO tripDto in _data) { table.Rows.Add(tripDto.DestinationName, tripDto.Departure, tripDto.FreeSeats); }
		}

		private void table_CellClick(object sender, DataGridViewCellEventArgs e) {
			string destinationName = table.SelectedRows[0].Cells[0].Value.ToString();
			DateTime departure = DateTime.Parse(table.SelectedRows[0].Cells[1].Value.ToString());
			int? tripId = _appService.GetTripIdByDestinationAndDeparture(destinationName, departure);
			if (tripId != null) {
				List<BookedTripDTO> result = _appService.Search(destinationName, departure);
				Form3 form3 = new Form3(tripId.Value, result, _appService);
				form3.ShowDialog();
			}
		}
	}
}