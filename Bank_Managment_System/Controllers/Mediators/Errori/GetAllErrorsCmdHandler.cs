using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Errori
{
    public class GetAllErrorsCmdHandler: IcomandhandlerList<Models.SystemModels.Error, object>
    {
        private readonly Ierror err;
        public GetAllErrorsCmdHandler(Ierror err)
        {
            this.err = err;
        }
        public async Task<List<Models.SystemModels.Error>> Handle(object command)
        {
            return await err.GetAllErrors();
        }
    }
}
