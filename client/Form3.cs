using System;
using System.Collections.Generic;
using System.Windows.Forms;
using model;
using services;

namespace client {
	public partial class Form3 : Form, IAppObserver {
		private readonly int _tripId;
		private readonly IAppServices _appService;
		private readonly User _result;
		public string Destination;
		public DateTime Departure;
		private List<BookedTripDTO> _data;

		public Form3(IAppServices appService, User result, int tripId, string destination, in DateTime departure) {
			_tripId = tripId;
			_appService = appService;
			_result = result;
			Destination = destination;
			Departure = departure;
			InitializeComponent();
			foreach (BookedTripDTO bookedTripDto in CreateList())
				table.Rows.Add(bookedTripDto.SeatNumber, bookedTripDto.ClientName);
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

		public List<BookedTripDTO> CreateList() {
			List<BookedTripDTO> result = _appService.Search(Destination, Departure);
			List<BookedTripDTO> temporary = new List<BookedTripDTO>(18);
			for (int i = 0; i < 18; i++)
				temporary.Add(new BookedTripDTO(-1, "-", i + 1));
			foreach (var a in result)
				temporary[a.SeatNumber - 1] = a;
			return temporary;
		}

		public void UpdateWindows(string destinationName, DateTime departure, int seatNumber, string clientName) {
			Console.WriteLine("tripController");
			foreach (BookedTripDTO trip in _data)
				if (trip.SeatNumber == seatNumber) {
					trip.ClientName = clientName;
					break;
				}

			//table.Refresh();
		}
	}
}