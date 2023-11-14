using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using ErrorEnumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.OnlineBank.Core.Interfaces.InterfaceRepos
{ 
    public interface IErrorRepos
    {
        bool Error(string context, ErrorEnum lvl);
        Task<List<Error>> GeterrorBytype(GeterrorbytypeRequest en);
        Task<List<Error>> GetErrorBydate(GeterrorBydateRequest en);
        Task<List<Error>> GetAllErrors();
    }
}
