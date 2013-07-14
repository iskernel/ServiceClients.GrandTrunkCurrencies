using System;
using System.Runtime.Serialization;

namespace IsKernel.Api.Pool.Currency
{
	/// <summary>
	/// Desctiption of CurrencyException.
	/// </summary>
	public class CurrencyConversionException : Exception, ISerializable
	{
		public CurrencyConversionException()
		{
		}

	 	public CurrencyConversionException(string message) 
	 		: base(message)
		{
		}

		public CurrencyConversionException(string message, Exception innerException) 
			: base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected CurrencyConversionException(SerializationInfo info, 
		                                      StreamingContext context)
			: base(info, context)
		{
		}
	}
}