using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ContractTester.Models
{
    public class Contract
    {
        public string Id { get; set; }
        public string Description { get; set; }

        [DisplayName("Contract")]
        public string ContractString { get; set; }

        [DisplayName("Version Number")]
        public string VersionNumber { get; set; }

        [DisplayName("Update Time")]
        public DateTime UpdateInst { get; set; }


        public Contract()
        {
            UpdateInst = DateTime.Now;
        }
    }
}
