using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ContractTester.Models;
using Contract = ContractTester.Models.Contract;

namespace ContractTester.ViewModels
{
    public class TestMessageViewModel
    {
        public Contract contract { get; set; }
        public bool TestMessageIsValid { get; set; }
        public string TestMessage { get; set; }

        public TestMessageViewModel() { }
    }

}
