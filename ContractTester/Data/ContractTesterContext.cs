using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContractTester.Models
{
    public class ContractTesterContext : DbContext
    {
        public ContractTesterContext (DbContextOptions<ContractTesterContext> options)
            : base(options)
        {
        }

        public DbSet<ContractTester.Models.Contract> Contract { get; set; }
    }
}
