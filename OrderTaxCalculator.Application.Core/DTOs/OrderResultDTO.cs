﻿using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.DTOs
{
    public class OrderResultDTO
    {
        public decimal PreTaxTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalCost { get; set; }
    }
}
