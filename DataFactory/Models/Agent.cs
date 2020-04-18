using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataFactory.Models
{
    public class Agent
    {
        public int AgentId { get; set; }
        [StringLength(15)]
        [Index(IsUnique = true)]
        public string AgentCode { get; set; }
        [StringLength(50)]
        public string AgentName { get; set; }
        [StringLength(20)]
        public string AgentType { get; set; }
        public DateTime StartDate { get; set; }
    }
}
