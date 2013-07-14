using System;
using System.Linq;
using NUnit.Framework;

namespace IsKernel.Api.Pool.Currency.Tests
{
	[TestFixture]
	public class GrandTrunkCurrencyConversionServiceTests
	{
		private const string TEST_CURRENCY_NAME_US_DOLLAR = "USD";
		private const string TEST_CURRENCY_NAME_AU_DOLLAR = "AUD";
		
		[Test]
		public void Instance_SanityCheck_IsNotNull()
		{
			Assert.IsNotNull(GrandTrunkCurrencyConversionService.Instance);
		}
		
		[Test, Sequential]
		public void GetSupportCurrencies_NoParameters_SpecificCurrencyIsPresent(
					[Values(TEST_CURRENCY_NAME_US_DOLLAR,
			        		TEST_CURRENCY_NAME_AU_DOLLAR)] string currencyName)
		{
			System.Collections.Generic.List<string> result  
			=	GrandTrunkCurrencyConversionService.Instance
								.GetListOfSupportedCurrencies();
			Assert.IsTrue(result.Contains(currencyName));
		}
		
		[Test, Sequential]
		public void GetSupportCurrencies_DateNowParameter_SpecificCurrencyIsPresent(
					[Values(TEST_CURRENCY_NAME_US_DOLLAR,
			        		TEST_CURRENCY_NAME_AU_DOLLAR)] string currencyName)
		{
			DateTime now = DateTime.Now;
			System.Collections.Generic.List<string> result  
			=	GrandTrunkCurrencyConversionService.Instance
								.GetListOfSupportedCurrencies(now);
			Assert.IsTrue(result.Contains(currencyName));
		}
		
		[Test, Sequential]
		public void GetSupportCurrencies_NullParameter_SpecificCurrencyIsPresent(
					[Values(TEST_CURRENCY_NAME_US_DOLLAR,
			        		TEST_CURRENCY_NAME_AU_DOLLAR)] string currencyName)
		{
			System.Collections.Generic.List<string> result  
			=	GrandTrunkCurrencyConversionService.Instance
								.GetListOfSupportedCurrencies(null);
			Assert.IsTrue(result.Contains(currencyName));
		}
		
		[Test]
		public void GetSupportCurrencies_DateFutureParameter_ThrowsException()
		{
			DateTime futureDate = DateTime.Now.AddYears(1);
			Assert.Throws(typeof(CurrencyConversionException),
			              () => GrandTrunkCurrencyConversionService.Instance
			              		.GetListOfSupportedCurrencies(futureDate) );
		}
		
		[Test]
		public void GetConversionRate_NoDateParameter_DoesNotThrowException()
		{
			Assert.DoesNotThrow( () => GrandTrunkCurrencyConversionService.Instance
									  .GetConversionRate(
			                    		TEST_CURRENCY_NAME_US_DOLLAR,
       		 					 		TEST_CURRENCY_NAME_AU_DOLLAR) );
			                    			
		}
		
		[Test]
		public void GetConversionRate_DateNowParameter_DoesNotThrowException()
		{
			Assert.DoesNotThrow( () => GrandTrunkCurrencyConversionService.Instance
									  .GetConversionRate(TEST_CURRENCY_NAME_US_DOLLAR,
       		 					 						 TEST_CURRENCY_NAME_AU_DOLLAR, 
       		 					 						 DateTime.Now) );			                    			
		}
		
		[Test]
		public void GetConversionRate_NullDateParameter_DoesNotThrowException()
		{
			Assert.DoesNotThrow( () => GrandTrunkCurrencyConversionService.Instance
									  .GetConversionRate(TEST_CURRENCY_NAME_US_DOLLAR,
       		 					 						 TEST_CURRENCY_NAME_AU_DOLLAR, 
       		 					 						 null) );			                    			
		}
		
		[Test]
		public void GetConversionRate_DateFutureParameter_ThrowsException()
		{
			DateTime future = DateTime.Now.AddYears(1);
			Assert.Throws( typeof(CurrencyConversionException),
	                       () => GrandTrunkCurrencyConversionService.Instance
							     .GetConversionRate(TEST_CURRENCY_NAME_US_DOLLAR,
		 					 						TEST_CURRENCY_NAME_AU_DOLLAR, 
		 					 						future) );			                    			
		}
		
		[Test]
		public void GetConversionRate_DateIntervalOkParameters_DoesNotThrowException()
		{
			DateTime start = new DateTime(2010,1,1);
			DateTime stop = new DateTime(2011,1,1);
			Assert.DoesNotThrow( () => GrandTrunkCurrencyConversionService.Instance
									  .GetConversionRate(TEST_CURRENCY_NAME_US_DOLLAR,
       		 					 						 TEST_CURRENCY_NAME_AU_DOLLAR, 
       		 					 						 start,
       		 					 						 stop) );
		}
		
		[Test]
		public void GetConversionRate_DateIntervalStartDateFutureParameters_DoesNotThrowException()
		{
			DateTime start = DateTime.Now.AddYears(1);
			DateTime stop = new DateTime(2011,1,1);
			Assert.Throws(typeof(CurrencyConversionException), 
			              () => GrandTrunkCurrencyConversionService.Instance
									  .GetConversionRate(TEST_CURRENCY_NAME_US_DOLLAR,
       		 					 						 TEST_CURRENCY_NAME_AU_DOLLAR, 
       		 					 						 start,
       		 					 						 stop) );
		}
		
		[Test]
		public void GetConversionRate_DateIntervalStopDateFutureParameters_DoesNotThrowException()
		{
			DateTime start = new DateTime(2011,1,1);
			DateTime stop = DateTime.Now.AddYears(1);
			Assert.Throws(typeof(CurrencyConversionException), 
			              () => GrandTrunkCurrencyConversionService.Instance
									  .GetConversionRate(TEST_CURRENCY_NAME_US_DOLLAR,
       		 					 						 TEST_CURRENCY_NAME_AU_DOLLAR, 
       		 					 						 start,
       		 					 						 stop) );
		}
		
		[Test]
		public void GetConversionRate_DateIntervalStartDateAfterStopDateParameters_DoesNotThrowException()
		{
			DateTime start = new DateTime(2012,1,1);
			DateTime stop = new DateTime(2010,1,1);
			Assert.Throws(typeof(CurrencyConversionException), 
			              () => GrandTrunkCurrencyConversionService.Instance
									  .GetConversionRate(TEST_CURRENCY_NAME_US_DOLLAR,
       		 					 						 TEST_CURRENCY_NAME_AU_DOLLAR, 
       		 					 						 start,
       		 					 						 stop) );
		}
	}
}
