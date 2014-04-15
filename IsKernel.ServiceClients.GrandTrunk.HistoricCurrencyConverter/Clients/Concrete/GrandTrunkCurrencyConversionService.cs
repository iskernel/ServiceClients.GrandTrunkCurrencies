using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Clients.Abstract;
using IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Contracts;
using IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Exceptions;

namespace IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Clients.Concrete
{
	public class GrandTrunkCurrencyConversionService : IGrandTrunkCurrencyConversionService
	{	
		private const string BASE_URL = "http://currencies.apps.grandtrunk.net/";
		private const string CURRENCIES_OPTION = "/currencies";
		private const string CONVERT_LATEST_OPTION = "/getlatest";
		private const string CONVERT_OPTION = "/getrate";
		private const string CONVERT_RANGE = "/getrange";
		
		private const string ERROR_DATE_IN_THE_FUTURE 
				= "You can only obtain the data " +
				  "from the past or present, not the future.";
		private const string ERROR_START_DATE_LATER_THAN_END_DATE 
				= "In a time interval the start date must be before " +
				  "the end date";
		
		private const string RANGE_SEPARATOR = " ";
		private const string OPTION_SEPARATOR = "/";
		
		private readonly RestClient _client;
		
		public GrandTrunkCurrencyConversionService()
		{
			_client = new RestClient(BASE_URL);
		}
		
		private string GetDateFormat(DateTime? date)
		{			
			string dateString = string.Empty;
			if(date.HasValue == true)
			{
				dateString = date.Value.ToString("yyyy-MM-dd");
			}
			return dateString;
		}
				
		public Task<List<string>> GetListOfSupportedCurrenciesAsync()
		{
			var availableCurrencies = GetListOfSupportedCurrenciesAsync(null);
			return availableCurrencies;
		}
		
		public Task<List<string>> GetListOfSupportedCurrenciesAsync(DateTime? date)
		{						
			var taskCompletionSource = new TaskCompletionSource<List<string>>();
			var request = new RestRequest();
			if(date != null)
			{
				if( ( (DateTime)(date) ).CompareTo(DateTime.Now) <= 0 )
				{
					request.Resource = CURRENCIES_OPTION + OPTION_SEPARATOR + GetDateFormat(date);
				}
				else
				{
					throw new HistoricCurrencyConverterException(ERROR_DATE_IN_THE_FUTURE);
				}
			}
			else
			{
				request.Resource = CURRENCIES_OPTION;
			}
			_client.ExecuteAsync(request, response => 
				{
 					var separator = Environment.NewLine.ToCharArray();
					var availableCurrencies = response.Content.Split(separator).ToList();
					taskCompletionSource.SetResult(availableCurrencies);					
				});
			return taskCompletionSource.Task;
		}
	
		public Task<ConversionRate> GetConversionRateAsync(string fromCode, string toCode)
		{
			if(string.IsNullOrWhiteSpace(fromCode) == true)
			{
				throw new HistoricCurrencyConverterException("fromCode cannot be null or whitespace");
			}
			if(string.IsNullOrWhiteSpace(toCode) == true)
			{
				throw new HistoricCurrencyConverterException("toCode cannot be null or whitespace");
			}
			var task = GetConversionRateAsync(fromCode, toCode, null);
			return task;
		}
		
		public Task<ConversionRate> GetConversionRateAsync(string fromCode, string toCode, DateTime? date)
		{
			if(string.IsNullOrWhiteSpace(fromCode) == true)
			{
				throw new HistoricCurrencyConverterException("fromCode cannot be null or whitespace");
			}
			if(string.IsNullOrWhiteSpace(toCode) == true)
			{
				throw new HistoricCurrencyConverterException("toCode cannot be null or whitespace");
			}
			var taskCompletionSource = new TaskCompletionSource<ConversionRate>();
			var request = new RestRequest();
			if(date!=null)
			{
				if(date.Value.CompareTo(DateTime.Now) <= 0 )
				{
					request.Resource = CONVERT_OPTION 
									   + OPTION_SEPARATOR + GetDateFormat(date)
									   + OPTION_SEPARATOR + fromCode 
									   + OPTION_SEPARATOR + toCode;
				}
				else
				{
					throw new HistoricCurrencyConverterException(ERROR_DATE_IN_THE_FUTURE);
				}
			}
			else
			{
				request.Resource = CONVERT_LATEST_OPTION 
								   + OPTION_SEPARATOR + fromCode
								   + OPTION_SEPARATOR + toCode;
			}
			_client.ExecuteAsync(request, response =>
				{
					try
					{
						var result = Decimal.Parse(response.Content);
						var conversionRate = new ConversionRate(fromCode, toCode, result, date);
						taskCompletionSource.SetResult(conversionRate);
					}
					catch(Exception)
					{
						var conversionRate = new ConversionRate(fromCode, toCode, 0.0m, date);
						taskCompletionSource.SetResult(conversionRate);
					}
				});
			return taskCompletionSource.Task;
		}
				
		public Task<List<ConversionRate>> GetConversionRateAsync(string fromCode, string toCode, 
		                                              			 DateTime start, DateTime end)
		{
			if(string.IsNullOrWhiteSpace(fromCode) == true)
			{
				throw new HistoricCurrencyConverterException("fromCode cannot be null or whitespace");
			}
			if(string.IsNullOrWhiteSpace(toCode) == true)
			{
				throw new HistoricCurrencyConverterException("toCode cannot be null or whitespace");
			}
			if( (start.CompareTo(DateTime.Now) > 0 ) || (end.CompareTo(DateTime.Now) > 0) )
			{
				throw new HistoricCurrencyConverterException("start and end date must not be in the future");
			}
			if( start.CompareTo(end) > 0)
			{
				throw new HistoricCurrencyConverterException("The start date must not be later than the end date");
			}			
			const int DATE_INDEX = 0;
			const int RATE_INDEX = 1;
			var taskCompletionSource = new TaskCompletionSource<List<ConversionRate>>();
			var conversionRates = new List<ConversionRate>();
			var request = new RestRequest();
			request.Resource = CONVERT_RANGE 
							   + OPTION_SEPARATOR + GetDateFormat(start)
							   + OPTION_SEPARATOR + GetDateFormat(end)
							   + OPTION_SEPARATOR + fromCode 
							   + OPTION_SEPARATOR + toCode;
			_client.ExecuteAsync(request, response =>
				{
					var separator = Environment.NewLine.ToCharArray();
					var dateAndRateList = response.Content.Split(separator).ToList();
					//Eliminates the last element which is only whitespace
					dateAndRateList.Remove(dateAndRateList.Last());
					var conversionRatesList = new List<ConversionRate>();
					foreach (var element in dateAndRateList) 
					{
						var rangeSeparator = RANGE_SEPARATOR.ToCharArray();
						var dateAndRateArray = element.Split(rangeSeparator);		
						try 
						{
							DateTime? date = DateTime.Parse(dateAndRateArray[DATE_INDEX]);
							decimal? rate = Decimal.Parse(dateAndRateArray[RATE_INDEX]);
							var conversionRate = new ConversionRate(fromCode, toCode, rate, date);	
							conversionRatesList.Add(conversionRate);	
						} 
						catch(Exception exception) 
						{
							taskCompletionSource.SetException(exception);
						}						
					}	
					taskCompletionSource.SetResult(conversionRatesList);							
				});					
			return taskCompletionSource.Task;
		}
		
	}
}
