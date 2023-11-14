using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Operator
{
    public class TwostepForOperatorCmdHandler:IcomandHandler<string>
    {
        private readonly Ioperator operatori;

        public TwostepForOperatorCmdHandler(Ioperator oper)
        {
            operatori = oper;
        }
        public async Task< int> Handle(string comand)
        {
            if (await operatori.TwostepAuntification(comand))
                return 1;

            return -1;
        }
    }
}
