
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;

namespace Bank_Managment_System.Services.ErrorAndLogger
{
    public class ErrorService: Ierror
    {
        private readonly IErrorRepos repos;

        public ErrorService(IErrorRepos rep)
        {
            repos = rep;
        }

        #region GeterrorBytype
        public async Task<List<Error>> GeterrorBytype(GeterrorbytypeRequest enn)
        {
            return await repos.GeterrorBytype(enn);
        }
        #endregion

        #region GetAllErrors
        public async Task< List<Error>> GetAllErrors()
        {
            return await repos.GetAllErrors();
        }
        #endregion

        #region GetErrorBydate
        public async Task<List<Error>> GetErrorBydate(GeterrorBydateRequest en)
        {
            return await repos.GetErrorBydate(en);
        }
        #endregion

    }
}
