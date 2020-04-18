using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactory.Models
{
    public class MonthlyContestCategory
    {
        public int MonthlyContestCategoryId { get; set; }
        public decimal MinimumPremium { get; set; }
        public decimal MaximumPremium { get; set; }
        public decimal Bonus { get; set; }
    }
}
