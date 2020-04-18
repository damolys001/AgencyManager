using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactory.Models
{
    public class Salary
    {
        public int SalaryId { get; set; }
        [StringLength(20)]
        public string AgentType { get; set; }
        public decimal Amount { get; set; }
    }
}
