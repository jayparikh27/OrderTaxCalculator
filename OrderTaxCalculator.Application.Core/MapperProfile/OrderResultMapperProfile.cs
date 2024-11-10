using AutoMapper;
using OrderTaxCalculator.Application.Core.DTOs;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.MapperProfile
{
    internal class OrderResultMapperProfile:Profile
    {
        public OrderResultMapperProfile()
        {
            CreateMap<OrderResult, OrderResultDTO>().ReverseMap();
        }
    }
}
