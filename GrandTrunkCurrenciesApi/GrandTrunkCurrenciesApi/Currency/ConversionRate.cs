using System;

namespace IsKernel.Api.Pool.Currency
{
	/// <summary>
	/// Description of ConversionRate.
	/// </summary>
	public class ConversionRate
	{
		private string _fromCurrency;		
		private string _toCurrency;		
		private double _rate;	
		private readonly DateTime _date;
				
		public ConversionRate(string fromCurrency, string toCurrency, 
		                      double rate, DateTime? date)
		{
			_fromCurrency = fromCurrency;
			_toCurrency = toCurrency;
			_rate = rate;
			if(date!=null)
			{
				_date = (DateTime)date;
			}
			else
			{
				_date = DateTime.Today;
			}
		}
		
		public ConversionRate(string fromCurrency, string toCurrency,
		                      double rate) 
			: this(fromCurrency, toCurrency, rate, DateTime.Today)
		{
					
		}
		
		public double Rate 
		{
			get { return _rate; }
		}
		
		public DateTime Date 
		{
			get { return _date; }
		}
		
		public string FromCurrency 
		{
			get { return _fromCurrency; }
		}
		
		public string ToCurrency 
		{
			get { return _toCurrency; }
		}
		
		public override string ToString()
		{
			return string.Format("[ConversionRate FromCurrency={0}, " +
			                     "ToCurrency={1}, Rate={2}, Date={3}]", 
			                     _fromCurrency, _toCurrency, _rate, _date);
		}

		public override bool Equals(object obj)
		{
			ConversionRate other = obj as ConversionRate;
			if (other == null)
			{
				return false;
			}
			bool result =  (this.FromCurrency.Equals(other.FromCurrency) ) 
						&& (this.ToCurrency.Equals(other.ToCurrency) )
						&& (this.Rate == other.Rate) 
						&& (this.Date.Year == other.Date.Year) 
						&& (this.Date.Month == other.Date.Month)
						&& (this.Date.Day == other.Date.Day);
			return result;			
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (_fromCurrency != null)
				{
					hashCode += 1000000007 * _fromCurrency.GetHashCode();
				}
				if (_toCurrency != null)
				{
					hashCode += 1000000009 * _toCurrency.GetHashCode();
				}
				hashCode += 1000000021 * _rate.GetHashCode();
				hashCode += 1000000033 * _date.GetHashCode();
			}
			return hashCode;
		}



	}
}
