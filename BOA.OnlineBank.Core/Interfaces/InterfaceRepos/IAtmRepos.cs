using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.ResponseAndRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.OnlineBank.Core.Interfaces.InterfaceRepos
{
    public  interface IAtmRepos
    {
        Task<string> CheckBalance(AuthorizedRequest req);
        Task<decimal> Withdrawing(AuthorizedRequest req, decimal Amount, ValuteEnum enm);
        Task<int> ChangePinCode(ChangePinRequest req);
    }
}
