using Bank_Managment_System.Helper_Enums;

namespace Bank_Managment_System.ResponseAndRequest
{
    public class withdrawalRequest
    {
            public AuthorizedRequest req { get; set; }
            public decimal Amount{ get; set; }
            public ValuteEnum enm { get; set; }
    } 
}
