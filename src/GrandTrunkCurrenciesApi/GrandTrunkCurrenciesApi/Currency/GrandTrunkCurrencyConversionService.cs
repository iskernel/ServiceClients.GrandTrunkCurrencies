using System;
using System.Collections.Generic;
using RestSharp;

namespace IsKernel.Api.Pool.Currency
{
	/// <summary>
	/// Historical currency conversion using the GrandTrunk currency 
	/// conversion service.
	/// </summary>
	public class GrandTrunkCurrencyConversionService
	{	
		//The service's URL
		private const string BASE_URL = "http://currencies.apps.grandtrunk.net/";
		
		//Error messages
		private const string ERROR_DATE_IN_THE_FUTURE 
				= "You can only obtain the data " +
				  "from the past or present, not the future.";
		private const string ERROR_START_DATE_LATER_THAN_END_DATE 
				= "In a time interval the start date must be before " +
				  "the end date";
		//Service options
		private const string CURRENCIES_OPTION = "/currencies";
		private const string CONVERT_LATEST_OPTION = "/getlatest";
		private const string CONVERT_OPTION = "/getrate";
		private const string CONVERT_RANGE = "/getrange";
		
		//Separators 
		private const string DATE_SEPARATOR = "-";
		private const string RANGE_SEPARATOR = " ";
		private const string OPTION_SEPARATOR = "/";
		
		//Singleton's instance
		private static GrandTrunkCurrencyConversionService _instance = null;
		//Client for RESTful operations
		private RestClient _client;
		
		/// <summary>
		/// Private singleton constructor
		/// </summary>
		private GrandTrunkCurrencyConversionService()
		{
			_client = new RestClient(BASE_URL);
		}
		
		/// <summary>
		/// Converts a date into a string having the format yyyy-[m]m-[d]d
		/// </summary>
		/// <param name="date">The date to be converted</param>
		/// <returns>A string in the format yyyy-mm-dd</returns>
		private string GetDateFormat(DateTime? date)
		{			
			string dateString = string.Empty;
			if(date!=null)
			{
				DateTime convertedDate = (DateTime) (date);
				dateString = convertedDate.Year + DATE_SEPARATOR
				 			 + convertedDate.Month + DATE_SEPARATOR 
							 + convertedDate.Day;
			}
			return dateString;
		}
		
		/// <summary>
		/// The singleton instance of the service
		/// </summary>
		public static GrandTrunkCurrencyConversionService Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new GrandTrunkCurrencyConversionService();
				}
				return _instance;
			}
		}
		
		/// <summary>
		/// Returns a list of the service's supported currencies at
		/// the current moment.
		/// </summary>
		/// <returns>A list of strings containing the supported currencies</returns>
		public List<string> GetListOfSupportedCurrencies()
		{
			List<string> availableCurrencies = GetListOfSupportedCurrencies(null);
			return availableCurrencies;
		}
		
		/// <summary>
		/// Returns a list of the service's supported currencies at
		/// a specific date.
		/// </summary>
		/// <param name="date">The date for the query</param>
		/// <returns>A list of strings containing the supported currencies</returns>
		public List<string> GetListOfSupportedCurrencies(DateTime? date)
		{			
			List<string> availableCurrencies = new List<string>();
			RestRequest request = new RestRequest();
			if(date!=null)
			{
				if( ( (DateTime)(date) ).CompareTo(DateTime.Now) <= 0 )
				{
					request.Resource = CURRENCIES_OPTION + OPTION_SEPARATOR 
									  + GetDateFormat(date);
				}
				else
				{
					throw new CurrencyConversionException(ERROR_DATE_IN_THE_FUTURE);
				}
			}
			else
			{
				request.Resource = CURRENCIES_OPTION;
			}
			RestResponse response = (RestResponse)_client.Execute(request);			
			char[] separator = Environment.NewLine.ToCharArray();
			availableCurrencies.AddRange( response.Content.Split(separator) );
			return availableCurrencies;
		}
	
		/// <summary>
		/// Returns a ConversionRate object for two currencies
		/// </summary>
		/// <param name="fromCode">The currency which will be converted</param>
		/// <param name="toCode">The currency to which the conversion 
		/// will take place</param>
		/// <returns>A ConversioRate object</returns>
		public ConversionRate GetConversionRate(string fromCode, string toCode)
		{
			return GetConversionRate(fromCode, toCode, null);
		}
		
		/// <summary>
		/// Returns a ConversionRate object for two currencies
		/// </summary>
		/// <param name="fromCode">The currency which will be converted</param>
		/// <param name="toCode">The currency to which the conversion 
		/// will take place</param>
		/// <param name="date">The date for the query</param>
		/// <returns>A ConversioRate object</returns>
		public ConversionRate GetConversionRate(string fromCode, string toCode, 
		                                        DateTime? date)
		{
			RestRequest request = new RestRequest();
			if(date!=null)
			{
				if( ( (DateTime)(date) ).CompareTo(DateTime.Now) <= 0 )
				{
					request.Resource = CONVERT_OPTION 
									   + OPTION_SEPARATOR + GetDateFormat(date)
									   + OPTION_SEPARATOR + fromCode 
									   + OPTION_SEPARATOR + toCode;
				}
				else
				{
					throw new CurrencyConversionException(ERROR_DATE_IN_THE_FUTURE);
				}
			}
			else
			{
				request.Resource = CONVERT_LATEST_OPTION 
								   + OPTION_SEPARATOR + fromCode
								   + OPTION_SEPARATOR + toCode;
			}
			RestResponse response = (RestResponse)_client.Execute(request);
			decimal result = Decimal.Parse(response.Content);
			ConversionRate conversioNRate = new ConversionRate(fromCode, toCode, 
			                                                   result, date);
			return conversioNRate;
		}
		
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
		public List<ConversionRate> GetConversionRate(string fromCode, string toCode, 
		                                              DateTime start, DateTime end)
		{
			const int DATE_INDEX = 0;
			const int RATE_INDEX = 1;
			List<ConversionRate> conversionRates = new List<ConversionRate>();
			if( ( start.CompareTo(DateTime.Now) <= 0 ) &&
			    ( end.CompareTo(DateTime.Now) <= 0 ) )
			{
				if(start.CompareTo(end) <= 0)
				{
					RestRequest request = new RestRequest();
					request.Resource = CONVERT_RANGE 
									   + OPTION_SEPARATOR + GetDateFormat(start)
									   + OPTION_SEPARATOR + GetDateFormat(end)
									   + OPTION_SEPARATOR + fromCode 
									   + OPTION_SEPARATOR + toCode;
					RestResponse response = (RestResponse)_client.Execute(request);	
					string result = response.Content;
					char[] separator = Environment.NewLine.ToCharArray();
					List<string> dateAndRate = new List<string>();
					dateAndRate.AddRange( result.Split(separator) );
		
					foreach (string element in dateAndRate) 
					{
						if(element.Equals(String.Empty)==false)
						{
							string[] dateAndRateElements 
								= element.Split(RANGE_SEPARATOR.ToCharArray());
							DateTime date 
								= DateTime.Parse(dateAndRateElements[DATE_INDEX]);
							decimal rate 
								= Decimal.Parse(dateAndRateElements[RATE_INDEX]);
							ConversionRate conversionRate
								= new ConversionRate(fromCode, toCode, rate, date);
							conversionRates.Add(conversionRate);
						}
					}
				}
				else
				{
					throw new CurrencyConversionException(
						ERROR_START_DATE_LATER_THAN_END_DATE);
				}
			}
			else
			{
				throw new CurrencyConversionException(ERROR_DATE_IN_THE_FUTURE);
			}
			return conversionRates;
		}
		
	}
}
