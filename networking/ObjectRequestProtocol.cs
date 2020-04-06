using System;
using networking.dto;

namespace networking {
	public interface IRequest {
	}


	[Serializable] public class LoginRequest : IRequest {
		private UserDTO _dto;

		public LoginRequest(UserDTO dto) { this._dto = dto; }

		public virtual UserDTO DTO => _dto;
	}

	[Serializable] public class GetBookedTripsRequest : IRequest {
		private NetTripDTO _dto;

		public GetBookedTripsRequest(NetTripDTO dto) { this._dto = dto; }

		public virtual NetTripDTO DTO => _dto;
	}

	[Serializable] public class GetBookedTripRequest : IRequest {
		private NetBookedTripDTO _dto;

		public GetBookedTripRequest(NetBookedTripDTO dto) { this._dto = dto; }

		public virtual NetBookedTripDTO DTO => _dto;
	}

	[Serializable] public class ReserveSeatRequest : IRequest {
		private NetReservedDTO _dto;

		public ReserveSeatRequest(NetReservedDTO dto) { this._dto = dto; }

		public virtual NetReservedDTO DTO => _dto;
	}

	[Serializable] public class GetTripsRequest : IRequest {
	}

	[Serializable] public class GetBookedTripIDRequest : IRequest {
		private NetTripDTO _dto;

		public GetBookedTripIDRequest(NetTripDTO dto) { this._dto = dto; }

		public virtual NetTripDTO DTO => _dto;
	}

	[Serializable] public class LogoutRequest : IRequest {
		private int _dto;

		public LogoutRequest(int dto) { this._dto = dto; }

		public virtual int DTO => _dto;
	}
}