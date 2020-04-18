using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactory.Models
{
    public class PerformanceBonusCategory
    {
        public int PerformanceBonusCategoryId { get; set; }
        public decimal MinimumPremium { get; set; }
        public decimal MaximumPremium { get; set; }
        public decimal Bonus { get; set; }
    }
}
