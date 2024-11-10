using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Persistance.Repository
{
    internal sealed class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        // Since DB in not present, storing order in In-memory
        private readonly List<TEntity> _entities = new List<TEntity>();
        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }
    }
}
