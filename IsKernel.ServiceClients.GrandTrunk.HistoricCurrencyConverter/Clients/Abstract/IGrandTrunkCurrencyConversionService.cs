using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Contracts;

namespace IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Clients.Abstract
{
	/// <summary>
	/// Historical currency conversion using the GrandTrunk currency 
	/// conversion service.
	/// </summary>
	public interface IGrandTrunkCurrencyConversionService
	{
		/// <summary>
		/// Returns a list of the service's supported currencies at the current moment.
		/// </summary>
		/// <returns>A list of strings containing the supported currencies</returns>
		Task<List<string>> GetListOfSupportedCurrenciesAsync();
		
		/// <summary>
		/// Returns a list of the service's supported currencies at
		/// a specific date.
		/// </summary>
		/// <param name="date">The date for the query</param>
		/// <returns>A list of strings containing the supported currencies</returns>
		Task<List<string>> GetListOfSupportedCurrenciesAsync(DateTime? date);
		
		/// <summary>
		/// Returns a ConversionRate object for two currencies
		/// </summary>
		/// <param name="fromCode">The currency which will be converted</param>
		/// <param name="toCode">The currency to which the conversion 
		/// will take place</param>
		/// <returns>A ConversioRate object</returns>
		Task<ConversionRate> GetConversionRateAsync(string fromCode, string toCode);
		
		/// <summary>
		/// Returns a ConversionRate object for two currencies
		/// </summary>
		/// <param name="fromCode">The currency which will be converted</param>
		/// <param name="toCode">The currency to which the conversion 
		/// will take place</param>
		/// <param name="date">The date for the query</param>
		/// <returns>A ConversioRate object</returns>
		Task<ConversionRate> GetConversionRateAsync(string fromCode, string toCode, DateTime? date);
		
		/// <summary>
		/// Returns a list of ConversionRate objects according to the specified
		/// time interval.
		/// </summary>
		/// <param name="fromCode">The currency which will be converted</param>
		/// <param name="toCode">The currency to which the conversion 
		/// will take place</param>
		/// <param name="start">The start of the time interval</param>
		/// <param name="end">The end of the time interval</param>
		/// <returns>A list of ConversionRate objects according to the
		/// specified time interval.</returns>
		Task<List<ConversionRate>> GetConversionRateAsync(string fromCode, string toCode, 
		                                             	  DateTime start, DateTime end);
	}
}
