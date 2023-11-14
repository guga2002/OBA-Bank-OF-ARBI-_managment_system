using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Report
{
    public class UserstatsCmdHandler : IcomandreportHandler<UserStatrequest, UserStatsResponse>
    {
        private readonly Ireport rep;
        public UserstatsCmdHandler(Ireport re)
        {
            rep = re;
        }
        public async Task<UserStatsResponse> handle(UserStatrequest command)
        {
            return await rep.Userstats(command);
        }
    }
}
