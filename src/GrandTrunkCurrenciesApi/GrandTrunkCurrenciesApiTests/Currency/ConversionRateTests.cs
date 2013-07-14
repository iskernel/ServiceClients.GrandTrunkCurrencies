using System;
using NUnit.Framework;

namespace IsKernel.Api.Pool.Currency.Tests
{
	[TestFixture]
	public class ConversionRateTests
	{
		private ConversionRate _rate1;
		private ConversionRate _rate2;
		
		private const string DEFAULT_FROM_CURRENCY = "USD";
		private const string DEFAULT_TO_CURRENCY = "AUD";
		private const string OTHER_CURRENCY = "EUR";
		private	const double DEFAULT_RATE = 1.00;
		private	const double OTHER_RATE = 2.00;
		private readonly DateTime DEFAULT_DATE 
			= new DateTime(1990,5,18, 0,0,0);
		private readonly DateTime DEFAULT_DATE_PLUS_ONE_HOUR 
			= new DateTime(1990,5,18, 1,0,0);
		private readonly DateTime OTHER_DATE 
			= new DateTime(1991,5,18, 0, 0, 0);

		
		
		[Test]
		public void Constructor_CurrencyFromLoaded_IsLoaded()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreEqual(_rate1.FromCurrency, DEFAULT_FROM_CURRENCY);
		}
		
		[Test]
		public void Constructor_CurrencyToLoaded_IsLoaded()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreEqual(_rate1.ToCurrency, DEFAULT_TO_CURRENCY);
		}
		
		[Test]
		public void Constructor_RateLoaded_IsLoaded()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreEqual(_rate1.Rate, DEFAULT_RATE);
		}
		
		[Test]
		public void Constructor_DateLoaded_IsLoaded()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreEqual(_rate1.Date, DEFAULT_DATE);
		}
		
		[Test]
		public void Constructor_NoDateProvided_DateIsToday()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE);
			Assert.AreEqual(_rate1.Date, DateTime.Today);
		}
		
		[Test]
		public void Constructor_DateIsNull_DateIsToday()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, null);
			Assert.AreEqual(_rate1.Date, DateTime.Today);
		}
		
		[Test]
		public void Equals_WithHardCopy_AreEqual()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			_rate2 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreEqual(_rate1, _rate2);
		}
		
		[Test]
		public void Equals_WithShalloCopy_AreEqual()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			_rate2 = _rate1;
			Assert.AreEqual(_rate1, _rate2);
		}
		
		[Test]
		public void Equals_DifferentFromCurrency_AreNotEqual()
		{
			_rate1 = new ConversionRate(OTHER_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			_rate2 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreNotEqual(_rate1, _rate2);
		}
		
		[Test]
		public void Equals_DifferentToCurrency_AreNotEqual()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, OTHER_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			_rate2 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreNotEqual(_rate1, _rate2);
		}
		
		[Test]
		public void Equals_DifferentRate_AreNotEqual()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            OTHER_RATE, DEFAULT_DATE);
			_rate2 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			Assert.AreNotEqual(_rate1, _rate2);
		}
		
		[Test]
		public void Equals_DifferentDate_AreNotEqual()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, OTHER_DATE);
			_rate2 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            OTHER_RATE, DEFAULT_DATE);
			Assert.AreNotEqual(_rate1, _rate2);
		}
		
		[Test]
		public void Equals_OtherDateDiffersByLessThanADay_AreEqual()
		{
			_rate1 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            DEFAULT_RATE, DEFAULT_DATE);
			_rate2 = new ConversionRate(DEFAULT_FROM_CURRENCY, DEFAULT_TO_CURRENCY, 
			                            OTHER_RATE, DEFAULT_DATE_PLUS_ONE_HOUR);
			Assert.AreNotEqual(_rate1, _rate2);
		}
	}
}
