using cs_project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_project.Core.DTOs
{
    public class TransactionsCreateDTO
    {
        public int PumpId { get; set; }
        public double Liters { get; set; }
        public double PricePerLiter { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Timestamp { get; set; }

        public Pump? Pump { get; set; }
    }
}
