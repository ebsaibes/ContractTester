using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContractTester.Models
{
    public class Contract : IValidatableObject
    {
        public string Id { get; set; }
        
        [Required]
        public string Description { get; set; }

        [DisplayName("Contract")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string ContractString { get; set; }

        [DisplayName("Version Number")]
        [Required]
        public string VersionNumber { get; set; }

        [DisplayName("Update Time")]
        [Required]
        public DateTime UpdateInst { get; set; }


        public Contract()
        {
            UpdateInst = DateTime.Now;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Contract contract = (Contract)validationContext.ObjectInstance;

            try
            {
                JsonValue.Parse(contract.ContractString);
                var contractDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(contract.ContractString);

                if (!contractDictionary.Keys.Contains("ID") || contractDictionary["ID"] != "Guid")
                {
                    results.Add(new ValidationResult("Contract must include an ID (Guid)."));
                }

                if (!contractDictionary.Keys.Contains("Timestamp") || contractDictionary["Timestamp"] != "String")
                {
                    results.Add(new ValidationResult("Contract must include a Timestamp (String)"));
                }
            }
            catch (Exception ex)
            {
                results.Add(new ValidationResult("Contract must be valid JSON."));
            }

            return results;
        }
    }
}
