using System;

namespace services {
	public class AppServiceException : Exception {
		public AppServiceException() : base() { }

		public AppServiceException(String msg) : base(msg) { }

		public AppServiceException(String msg, Exception ex) : base(msg, ex) { }
	}
}