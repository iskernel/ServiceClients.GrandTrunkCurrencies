using System;

namespace IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Contracts
{
	public class ConversionRate
	{
		public string FromCurrency {get;set;}
		public string ToCurrency {get;set;}
		public decimal? Rate {get;set;}
		public DateTime Date {get;set;}
				
		public ConversionRate()
		{
			
		}
		
		public ConversionRate(string fromCurrency, string toCurrency, 
		                      decimal? rate, DateTime? date)
		{
			FromCurrency = fromCurrency;
			ToCurrency = toCurrency;
			Rate = rate;
			if(date != null)
			{
				Date = date.Value;
			}
			else
			{
				Date = DateTime.Now;
			}

		}
		
		public ConversionRate(string fromCurrency, string toCurrency,
		                      decimal? rate) 
			: this(fromCurrency, toCurrency, rate, DateTime.Today)
		{
					
		}
	}
}
