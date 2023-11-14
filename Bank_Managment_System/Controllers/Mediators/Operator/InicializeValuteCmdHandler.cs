using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Operator
{
    public class InicializeValuteCmdHandler:IcomandHandlerValute
    {
        private readonly Ioperator operato;

        public InicializeValuteCmdHandler(Ioperator oper)
        {
            operato = oper;
        }
        public async Task<bool> handle()
        {
            return await operato.InicializeValute();
        }
    }
}
