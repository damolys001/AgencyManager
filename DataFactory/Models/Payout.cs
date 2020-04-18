using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactory.Models
{
    public class Payout
    {
        public long PayoutId { get; set; }
        public string AgentName { get; set; }
        [StringLength(20)]
        public string AgentCode { get; set; }
        public decimal PreviousLifeProduction { get; set; }
        public decimal PreviousNonLifeProduction { get; set; }
        public decimal LifeProduction { get; set; }
        public decimal NonLifeProduction { get; set; }
        public decimal Savings { get; set; }
        public decimal FixedPay { get; set; }
        public decimal QuickStart { get; set; }
        public decimal Bonus { get; set; }
        public decimal Contest { get; set; }
    }
}
