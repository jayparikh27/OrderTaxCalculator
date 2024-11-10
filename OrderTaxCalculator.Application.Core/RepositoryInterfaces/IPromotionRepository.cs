﻿using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.RepositoryInterfaces
{
    public interface IPromotionRepository
    {
        Promotion? GetMaximumPromotionalDiscountByDate(DateTime orderDate);
    }
}