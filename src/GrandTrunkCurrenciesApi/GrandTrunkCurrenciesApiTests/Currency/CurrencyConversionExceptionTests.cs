using System;
using NUnit.Framework;

namespace IsKernel.Api.Pool.Currency.Tests
{
	[TestFixture]
	public class CurrencyConversionExceptionTests
	{
		private CurrencyConversionException _exception;
		private const string DEFAULT_MESSAGE = "DEFAULT MESSAGE";
		
		[Test]
		public void Constructor_MessageIsLoaded_IsLoaded()
		{
			_exception = new CurrencyConversionException(DEFAULT_MESSAGE);
			Assert.AreEqual(_exception.Message, DEFAULT_MESSAGE);
		}
	}
}
