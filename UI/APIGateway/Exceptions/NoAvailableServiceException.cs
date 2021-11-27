using System;
using System.Runtime.Serialization;

namespace APIGateway.Exceptions
{
	[Serializable]
	internal class NoAvailableServiceException : Exception
	{
		public NoAvailableServiceException()
		{
		}

		public NoAvailableServiceException(string message) : base(message)
		{
		}

		public NoAvailableServiceException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NoAvailableServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}