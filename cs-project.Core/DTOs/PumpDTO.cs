using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_project.Core.DTOs
{
    public class PumpDTO
    {
        public string FuelType { get; set; } = string.Empty;
        public double CurrentVolume { get; set; }
        public string Status { get; set; } = "Idle";
        public required object Id { get; set; }
    }
}
