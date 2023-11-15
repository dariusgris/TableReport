using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReport.Models
{
    internal class AffectedArea
    {
        [JsonProperty("area_id")]
        public string? AreaId { get; set; }
        [JsonProperty("area_name")]
        public string? AreaName { get; set; }
        [JsonProperty("total_customers")]
        public int TotalCustomers { get; set; }
        [JsonProperty("affected_customers")]
        public int AffectedCustomers { get; set; }
        [JsonProperty("estimated_recovery_time")]
        public DateTime EstimatedRecoveryTime { get; set; }
        [JsonProperty("reason")]
        public string? Reason { get; set; }
    }
}
