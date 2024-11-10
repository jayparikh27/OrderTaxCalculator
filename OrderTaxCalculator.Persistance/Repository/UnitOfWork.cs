using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Persistance.Repository
{
    internal class UnitOfWork : IUnitOfWork
    {
        //private readonly ApplicationDbContext _applicationDbContext;
        //public UnitOfWork(ApplicationDbContext applicationDbContext)
        //{
        //    _applicationDbContext = applicationDbContext;
        //}
        public void SaveChanges()
        {
            // Call DB context save changes method
        }

    }
}
