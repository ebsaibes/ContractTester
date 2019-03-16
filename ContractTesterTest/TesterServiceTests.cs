using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using ContractTester.Models;
using ContractTester.Service;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace ContractTesterTest
{
	public class TesterServiceTest
	{
		private Contract testContract;
		private string SampleMessage;

		public TesterServiceTest()
		{
			testContract = new Contract
			{
				ContractString = @"{""Name"":""String"",""Age"":""Int32""}"
			};
			SampleMessage = @"{""Name"":""Emil"",""Age"":""31""}";
		}


		[Fact]
		public void DeserializeContractToTupleTest()
		{
			Dictionary<string, string> testDictionary =
				JsonConvert.DeserializeObject<Dictionary<string, string>>(testContract.ContractString);

			Assert.NotEmpty(testDictionary);

		}

		[Fact]
		public void AreAllElementsInMessageTest_MessageValid()
		{
			TesterService testService = new TesterService(testContract, SampleMessage);
            testService.TestSetup();
			Assert.True(testService.AreAllElementsInMessage());

		}

		[Fact]
		public void AreAllElementsCorrectDataType_MessageValid()
		{
			TesterService testerService = new TesterService(testContract, SampleMessage);
            testerService.TestSetup();
			Assert.True(testerService.AreAllMessageValuesMatchDataTypes());

		}
	}
}