using System;
using System.Collections.Generic;
using model;
using networking.dto;

namespace networking {
	public interface IResponse {
	}

	[Serializable] public class OkResponse : IResponse {
	}

	[Serializable] public class ErrorResponse : IResponse {
		private string _message;

		public ErrorResponse(string message) { this._message = message; }

		public virtual string Message => _message;
	}

	public interface IUpdateResponse : IResponse {
	}

	[Serializable] public class ReservedResponse : IUpdateResponse {
		private NetUpdateDTO _netUpdateDTO;

		public ReservedResponse(NetUpdateDTO netUpdateDTO) { this._netUpdateDTO = netUpdateDTO; }

		public virtual NetUpdateDTO NetUpdateDTO => _netUpdateDTO;
	}

	[Serializable] public class LoginResponse : IResponse {
		private User _dto;

		public LoginResponse(User dto) { this._dto = dto; }

		public virtual User DTO => _dto;
	}

	[Serializable] public class LogoutResponse : IResponse {
	}

	[Serializable] public class GetTripsResponse : IResponse {
		private List<TripDTO> _dto;

		public GetTripsResponse(List<TripDTO> dto) { this._dto = dto; }

		public virtual List<TripDTO> DTO => _dto;
	}

	[Serializable] public class GetBookedTripsResponse : IResponse {
		private List<BookedTripDTO> _dto;

		public GetBookedTripsResponse(List<BookedTripDTO> dto) { this._dto = dto; }

		public virtual List<BookedTripDTO> DTO => _dto;
	}

	[Serializable] public class GetBookedTripIDResponse : IResponse {
		private int? _dto;

		public GetBookedTripIDResponse(int? dto) { this._dto = dto; }

		public virtual int? DTO => _dto;
	}

	[Serializable] public class ReserveSeatResponse : IResponse {
	}
}