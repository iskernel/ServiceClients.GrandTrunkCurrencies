using System;
using System.Linq;
using IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Clients.Abstract;
using IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Clients.Concrete;
using IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.Exceptions;
using NUnit.Framework;

namespace IsKernel.ServiceClients.GrandTrunk.HistoricCurrencyConverter.GrandTrunkCurrencuConversionService.Tests
{
	[TestFixture]
	public class GrandTrunkCurrencyConversionServiceTests
	{
		private const string TEST_CURRENCY_NAME_US_DOLLAR = "USD";
		private const string TEST_CURRENCY_NAME_AU_DOLLAR = "AUD";
		
		private IGrandTrunkCurrencyConversionService _service;
		
		[SetUp]
		public void Setup()
		{
			_service = new GrandTrunkCurrencyConversionService();
		}
		
		[Test, Sequential]
		public void GetSupportCurrenciesAsync_NoParameters_SpecificCurrencyIsPresent(
					[Values(TEST_CURRENCY_NAME_US_DOLLAR,
			        		TEST_CURRENCY_NAME_AU_DOLLAR)] string currencyName)
		{
			var result = _service.GetListOfSupportedCurrenciesAsync().Result;
			Assert.IsTrue(result.Contains(currencyName));
		}
		
		[Test, Sequential]
		public void GetSupportCurrenciesAsync_DateNowParameter_SpecificCurrencyIsPresent(
					[Values(TEST_CURRENCY_NAME_US_DOLLAR,
			        		TEST_CURRENCY_NAME_AU_DOLLAR)] string currencyName)
		{
			var now = DateTime.Now;
			var result = _service.GetListOfSupportedCurrenciesAsync(now).Result;
			Assert.IsTrue(result.Contains(currencyName));
		}
		
		[Test, Sequential]
		public void GetSupportCurrenciesAsync_NullParameter_SpecificCurrencyIsPresent(
					[Values(TEST_CURRENCY_NAME_US_DOLLAR,
			        		TEST_CURRENCY_NAME_AU_DOLLAR)] string currencyName)
		{
			var result = _service.GetListOfSupportedCurrenciesAsync(null).Result;
			Assert.IsTrue(result.Contains(currencyName));
		}
		
		[Test]
		public void GetSupportCurrenciesAsync_DateFutureParameter_ThrowsException()
		{
			DateTime futureDate = DateTime.Now.AddYears(1);
			Assert.That( () => _service.GetListOfSupportedCurrenciesAsync(futureDate).Result, 
						Throws.InstanceOf<HistoricCurrencyConverterException>());
		}
		
		[Test]
		public void GetConversionRateAsync_NoDateParameter_DoesNotThrowException()
		{
			var conversionRate = _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
	 									    					 TEST_CURRENCY_NAME_AU_DOLLAR).Result;
			Assert.IsTrue(true);			                    			
		}
		
		[Test]
		public void GetConversionRateAsync_DateNowParameter_DoesNotThrowException()
		{
			var conversionRate = _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
	 						 	 	  	    					 TEST_CURRENCY_NAME_AU_DOLLAR, 
	 						 		  	    					 DateTime.Now).Result;
			Assert.IsTrue(true);			                    			
		}
		
		[Test]
		public void GetConversionRateAsync_NullDateParameter_DoesNotThrowException()
		{
			var conversionRate = _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
	 						 			    					 TEST_CURRENCY_NAME_AU_DOLLAR, 
	 						 			    					 null).Result;
			Assert.IsTrue(true);			                    					                    			
		}
		
		[Test]
		public void GetConversionRateAsync_DateFutureParameter_ThrowsException()
		{
			DateTime future = DateTime.Now.AddYears(1);
			Assert.That( () => _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
		 					 								   TEST_CURRENCY_NAME_AU_DOLLAR, 
		 					 								   future).Result, 
						Throws.InstanceOf<HistoricCurrencyConverterException>());                    			
		}
		
		[Test]
		public void GetConversionRateAsync_DateIntervalOkParameters_DoesNotThrowException()
		{
			var start = new DateTime(2010,1,1);
			var stop = new DateTime(2011,1,1);
			var result = _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
 					 						 			 TEST_CURRENCY_NAME_AU_DOLLAR, 
 					 						 			 start,
 					 						 			 stop).Result;
			Assert.IsTrue(true);
		}
		
		[Test]
		public void GetConversionRateAsync_DateIntervalStartDateFutureParameters_ThrowsException()
		{
			var start = DateTime.Now.AddYears(1);
			var stop = new DateTime(2011,1,1);
			Assert.That( () => _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
					 								   TEST_CURRENCY_NAME_AU_DOLLAR, 
					 								   start, stop).Result, 
			Throws.InstanceOf<HistoricCurrencyConverterException>());  
		}
		
		[Test]
		public void GetConversionRateAsync_DateIntervalStopDateFutureParameters_ThrowsException()
		{
			DateTime start = new DateTime(2011,1,1);
			DateTime stop = DateTime.Now.AddYears(1);
			Assert.That( () => _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
					 								   TEST_CURRENCY_NAME_AU_DOLLAR, 
					 								   start, stop).Result, 
			Throws.InstanceOf<HistoricCurrencyConverterException>());  
			Assert.IsTrue(true);
		}
		
		[Test]
		public void GetConversionRateAsync_DateIntervalStartDateAfterStopDateParameters_ThrowsException()
		{
			DateTime start = new DateTime(2012,1,1);
			DateTime stop = new DateTime(2010,1,1);
			Assert.That( () => _service.GetConversionRateAsync(TEST_CURRENCY_NAME_US_DOLLAR,
					 								   TEST_CURRENCY_NAME_AU_DOLLAR, 
					 								   start, stop).Result, 
			Throws.InstanceOf<HistoricCurrencyConverterException>());  
		}
	}
}
