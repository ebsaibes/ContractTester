using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using ContractTester.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContractTester.Service
{
	public class TesterService
	{
		private Contract Contract;
		private string RawMessage;

		public Dictionary<string, string> contractDictionary { get; private set; }
		public Dictionary<string, string> messageKeyDictionary { get; private set; }

		public bool IsMessageValid()
		{
            List<bool> testCases = new List<bool>();
			TestSetup();
            if (messageKeyDictionary.Count > 0)
            {
                testCases.Add(AreAllElementsInMessage());
                testCases.Add(AreAllMessageValuesMatchDataTypes());
                return testCases.All(x => x);
            }

            return false;
        }


		public void TestSetup()
        {
			//Convert The Contract to a Dictionary
			contractDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Contract.ContractString);
            JsonValue jsonObject;
            if (TryParseJSON(RawMessage, out jsonObject))
            {
                messageKeyDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonObject.ToString());
            }
            else
            {
                messageKeyDictionary = new Dictionary<string, string>();
            }
			
		}

		/// <summary>
		/// Check that all properties of the message are contained in the contractKeyDictionary
		/// </summary>
		/// <returns></returns>
		public bool AreAllElementsInMessage()
		{
			foreach (KeyValuePair<string, string> kv in messageKeyDictionary)
			{
				if (!contractDictionary.Keys.Contains(kv.Key))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Check that all the values in the messaget 
		/// </summary>
		/// <returns></returns>
		public bool AreAllMessageValuesMatchDataTypes()
		{

			var propertyName = "";
			var dataTypeToCheckMessageValue = "";
            var messageValue = "";

            //loop through each property in the message Dictionary - trying to parse the values into the data type in the contract
			foreach (KeyValuePair<string, string> kv in messageKeyDictionary)
			{
                //if the contract doesn't supply the datatype for the parameter -> then the message doesn't match the contract.
                //TODO - we can refine this logic later (Required vs. Not Required)
				if (!contractDictionary.TryGetValue(kv.Key, out dataTypeToCheckMessageValue))
				{
					return false;
				}

				if (!string.IsNullOrEmpty(dataTypeToCheckMessageValue))
				{
                    if (dataTypeToCheckMessageValue == "Int32")
                    {
                        if (!Int32.TryParse(kv.Value, out int result))
                        {
                            return false;
                        }
                    }

                    if (dataTypeToCheckMessageValue == "Int64")
                    {
                        if (!Int64.TryParse(kv.Value, out Int64 result))
                        {
                            return false;
                        }
                    }

                    if (dataTypeToCheckMessageValue == "Guid")
                    {
                        if (!Guid.TryParse(kv.Value, out Guid result))
                        {
                            return false;
                        }
                    }
                }				
			}

			return true;

		}

        private bool TryParseJSON(string json, out JsonValue jsonObject)
        {
            try
            {
                jsonObject = JsonValue.Parse(RawMessage);
                return true;
            }
            catch
            {
                jsonObject = null;
                return false;
            }
        }

        public TesterService(Contract contract, string message)
		{
			this.Contract = contract;
			this.RawMessage = message;
		}
	}
}
