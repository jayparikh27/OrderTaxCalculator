using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.RepositoryInterfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
       void Add(TEntity entity);

    }
}
