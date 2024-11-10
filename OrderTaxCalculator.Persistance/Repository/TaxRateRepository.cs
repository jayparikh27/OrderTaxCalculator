using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Domain.Entities;
namespace OrderTaxCalculator.Persistance.Repository
{
    internal class TaxRateRepository : ITaxRateRepository
    {
        /// <summary>
        ///  This method will return taxrate based on states.
        /// </summary>
        /// <param name="state">state.</param>
        /// <returns>>List of TaxRate entity.</returns>
        // This enum created for readable purpose. Not required in the appliation
        public enum StateNameEnum
        {
            FL = 1,
            NM = 2,
            NV=3,
            GA =4,
            NY =5,
        }

        public TaxRate? GetTaxRatesByState(string state)
        {
            return new List<TaxRate>()  {
            new TaxRate { ID=1, StateID = (int)StateNameEnum.FL, BaseTaxRate =10, LuxuryTaxMultiplier = 2, ApplyDiscountBeforeTax = true, State = new State{  ID = 1, StateName = StateNameEnum.FL.ToString()} },
            new TaxRate {ID=2, StateID = (int)StateNameEnum.NM, BaseTaxRate = 10, LuxuryTaxMultiplier = 2, ApplyDiscountBeforeTax = true, State = new State{  ID = 2, StateName = StateNameEnum.NM.ToString() } },
            new TaxRate {ID=3, StateID =  (int)StateNameEnum.NV, BaseTaxRate =10, LuxuryTaxMultiplier = 1, ApplyDiscountBeforeTax = true, State = new State{  ID = 3, StateName = StateNameEnum.NV.ToString() } },
            new TaxRate {ID=4, StateID = (int)StateNameEnum.GA, BaseTaxRate =10, LuxuryTaxMultiplier = 1, ApplyDiscountBeforeTax = false, State = new State{  ID = 4, StateName = StateNameEnum.GA.ToString() } },
            new TaxRate {ID=5, StateID =  (int)StateNameEnum.NY, BaseTaxRate = 10, LuxuryTaxMultiplier =1, ApplyDiscountBeforeTax = false, State = new State{  ID = 5, StateName = StateNameEnum.NY.ToString() } },
        }.FirstOrDefault(x =>x.State.StateName.ToLower()==state.ToLower());
        }
    }
}
