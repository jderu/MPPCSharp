using System;

namespace MPPCSharp.service {
	public class AppServiceException : Exception {
		public AppServiceException() { }

		public AppServiceException(string message) : base(message) { }
	}
}