using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractTester.Models
{
    public class Contract
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public string ContractString { get; set; }
        
        public string VersionNumber { get; set; }

        public DateTime UpdateInst { get; set; }


        public Contract()
        {
            UpdateInst = DateTime.Now;
        }
    }
}
