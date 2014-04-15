using System;
using System.Runtime.Serialization;

namespace IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Exceptions
{
	/// <summary>
	/// Description of CurrencyException.
	/// </summary>
	public class HistoricCurrencyConverterException : Exception, ISerializable
	{
		public HistoricCurrencyConverterException()
		{
		}

	 	public HistoricCurrencyConverterException(string message) 
	 		: base(message)
		{
		}

		public HistoricCurrencyConverterException(string message, Exception innerException) 
			: base(message, innerException)
		{
		}

		protected HistoricCurrencyConverterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}