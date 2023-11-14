
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;

namespace Bank_Managment_System.Services.SystemServices
{
    public class OperatorService : Ioperator, Imanager
    {

        private readonly IoperatorRepos repos;
        public OperatorService(IoperatorRepos repos)
        {
            this.repos = repos;
        }

        #region InicializeValute
        public async Task<bool> InicializeValute()
        {

            return await repos.InicializeValute();
           
        }
        #endregion

        #region 2stepforoperator
        public async Task< bool> TwostepAuntification(string codeprovidedByUser)
        {
            return await repos.TwostepAuntification(codeprovidedByUser);

        }
        #endregion

        #region signUpOperator
        public async Task<int> signUpOperator(OperatorRequest request)//operatoris registracia
        {
            return await repos.signUpOperator(request);
        }
        #endregion

        #region SignInOperator
        public async Task<int> SignInOperator(OperatorSignInRequest req)
        {
            return await repos.SignInOperator(req);
        }
        #endregion

        #region SignOutOperator
        public async Task<bool> SignOutOperator()
        {
            return await repos.SignOutOperator();
        }
        #endregion

        #region SignUpManager
        public async Task< int> SignUpManager(ManagerSignuprequest request)//operatoris registracia
        {

            return await repos.SignUpManager(request);
           
        }
        #endregion

        #region SignInManager
        public async Task<int> SignInManager(ManagerSignInRequest req)
        {
            return await repos.SignInManager(req);
        }
        #endregion

        #region SignedOutManager
        public async Task<bool> signedOutManager()
        {
            return await repos.signedOutManager();
        }
        #endregion

    }
}
