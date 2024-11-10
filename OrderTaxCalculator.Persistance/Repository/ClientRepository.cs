using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    /// <summary>
    /// This method returns state based on client name.
    /// </summary>
    /// <param name="clientName">client name</param>
    /// <returns>Returns the state to which the client belongs.</returns>
    public class ClientRepository: IClientRepository
    { // This enum created for readable purpose. Not required in the appliation
        public enum StateNameEnum
        {
            FL = 1,
            NM = 2,
            NV = 3,
            GA = 4,
            NY = 5,
        }
        public State? GetStateNameForClient(string clientName)
        {
            return new List<Client>
            {
                new Client { ID = 1, ClientName = "Client GA", StateID = 1,State = new State{  ID = 1, StateName = StateNameEnum.GA.ToString() } },
                new Client { ID = 2, ClientName = "Client FL", StateID = 2 ,State = new State{  ID = 1, StateName = StateNameEnum.FL.ToString() } },
                new Client { ID = 3, ClientName = "Client NY", StateID = 3,State = new State{  ID = 1, StateName = StateNameEnum.NY.ToString() } },
                new Client { ID = 4, ClientName = "Client NM", StateID = 4,State = new State{  ID = 1, StateName = StateNameEnum.NM.ToString() } },
                new Client { ID = 5, ClientName = "Client NV", StateID = 5,State = new State{  ID = 1, StateName = StateNameEnum.NV.ToString() } },
            }.Where(x => x.ClientName.ToLower() == clientName.ToLower()).Select(x => x.State).FirstOrDefault();
                }
    }
}
