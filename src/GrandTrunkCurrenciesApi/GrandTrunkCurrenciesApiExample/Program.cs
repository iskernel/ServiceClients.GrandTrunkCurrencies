using System;
using System.Collections.Generic;
using IsKernel.Api.Pool.Currency;

namespace GrandTrunkCurrenciesApiExample
{
	class Program
	{		
		public static void Main(string[] args)
		{
			DateTime olderDate = new DateTime(1997,3,1);
			DateTime startDate = new DateTime(2011,3,1);
			DateTime stopDate  = new DateTime(2011,5,1);
			
			//List of supported currencies at this momment
			Console.WriteLine("List of supported currencies at this moment: ");		
			List<string> listOfCurrencies
				= GrandTrunkCurrencyConversionService
				 .Instance.GetListOfSupportedCurrencies();
			DisplayList(listOfCurrencies);
			
			//List of supported currencies in 01-03-1997
			Console.WriteLine("List of supported currencies in 01-03-1997: ");
			List<string> olderListOfCurrencies
				= GrandTrunkCurrencyConversionService
				 .Instance.GetListOfSupportedCurrencies(olderDate);
			DisplayList(olderListOfCurrencies);
			
			//The USD (US Dollar) - AUD (Australian Dollar) rate today
			Console.WriteLine("The USD-AUD rate today");
			ConversionRate usdToAud = GrandTrunkCurrencyConversionService
									  .Instance
									  .GetConversionRate("USD","AUD");
			Console.WriteLine("In {0} : 1 {1} = {3} {2}",
			                  usdToAud.Date, 
			                  usdToAud.FromCurrency,
			                  usdToAud.ToCurrency,
			                  usdToAud.Rate);
			
			//The USD (US Dollar) - AUD (Australian Dollar) rate in 01-03-1997
			Console.WriteLine("The USD-AUD rate in 01-03-1997");
			ConversionRate olderUsdToAud = GrandTrunkCurrencyConversionService
										  .Instance
									  	  .GetConversionRate("USD","AUD", 
				                   							 olderDate);
			Console.WriteLine("In {0} : 1 {1} = {3} {2}",
			                  olderUsdToAud.Date, 
			                  olderUsdToAud.FromCurrency,
			                  olderUsdToAud.ToCurrency,
			                  olderUsdToAud.Rate);
			
			//The USD (US Dollar) - AUD (Australian Dollar) rate between 
			//01.03.2011 - 01.05.2011
			Console.WriteLine("The USD-AUD rate between 01-03-2011 and " +
			                  "01-05-2011");
			List<ConversionRate> conversionRateList
				= GrandTrunkCurrencyConversionService.Instance
			  	  .GetConversionRate("USD","AUD", startDate, stopDate);
			DisplayConversionList(conversionRateList);
			
			Console.Read();
		}
		
		private static void DisplayList(List<string> list)
		{
			foreach (string element in list) 
			{
				Console.WriteLine(element);
			}
		}
		
		private static void DisplayConversionList(List<ConversionRate> list)
		{
			foreach (ConversionRate element in list) 
			{
				Console.WriteLine("In {0} : 1 {1} = {3} {2}",
			                  	  element.Date, 
			                  	  element.FromCurrency,
			                  	  element.ToCurrency,
			                  	  element.Rate);
			}
		}
	}
}