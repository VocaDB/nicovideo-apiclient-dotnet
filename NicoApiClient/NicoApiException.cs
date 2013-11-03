using System;
using System.Runtime.Serialization;

namespace NicoApi {

	/// <summary>
	/// Exception thrown if an API request failed.
	/// </summary>
	public class NicoApiException : Exception {
		public NicoApiException() {}
		public NicoApiException(string message, Exception innerException) : base(message, innerException) {}
		public NicoApiException(string message) : base(message) {}
		protected NicoApiException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}

}
