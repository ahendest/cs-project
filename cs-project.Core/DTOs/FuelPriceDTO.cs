﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_project.Core.DTOs
{
    public class FuelPriceDTO
    {
        public int Id { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public double CurrentPrice { get; set; }
    }
}
