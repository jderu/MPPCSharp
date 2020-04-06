using System;

namespace services {
	public interface IAppObserver {
		void UpdateWindows(String destinationName, DateTime departure, int seatNumber, String clientName);
	}
}