using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using ErrorEnumi;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface Ierror
    {
         Task<List<Error>> GeterrorBytype(GeterrorbytypeRequest en);
         Task<List<Error>> GetErrorBydate(GeterrorBydateRequest en);
         Task<List<Error>> GetAllErrors();
    }
}
