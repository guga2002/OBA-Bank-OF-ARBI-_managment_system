using Bank_Managment_System.ResponseAndRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.OnlineBank.Core.Interfaces.InterfaceRepos
{
    public  interface IoperatorRepos
    {
        Task<int> signUpOperator(OperatorRequest request);
        Task<int> SignInOperator(OperatorSignInRequest req);
        Task<bool> TwostepAuntification(string codeprovidedByUser);
        Task<bool> InicializeValute();
        Task<bool> SignOutOperator();
        Task<int> SignInManager(ManagerSignInRequest req);
        Task<int> SignUpManager(ManagerSignuprequest request);
        Task<bool> signedOutManager();
    }
}
