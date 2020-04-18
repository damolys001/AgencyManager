using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactory.Models
{
    public class Production
    {
        [StringLength(20)]
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string Spoke { get; set; }
        public decimal Premium { get; set; }
        [StringLength(15)]
        public string TransactionType { get; set; }
    }
}
