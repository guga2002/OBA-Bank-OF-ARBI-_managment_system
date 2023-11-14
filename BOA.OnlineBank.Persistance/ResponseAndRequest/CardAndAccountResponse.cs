using Bank_Managment_System.Helper_Enums;

namespace Bank_Managment_System.ResponseAndRequest
{
    public class CardAndAccountResponse
    { 

       

        public string Iban { get; set; }
        public decimal Balance { get; set; }
        public ValuteEnum Valute { get; set; }
        public decimal? AmountWithdrawed { get; set; }
        public DateTime? LastWithdrawDate { get; set; }

        public List<Cardrespons> cardresp { get; set; }
        public List<tRansresponse> trans { get; set; } 
    }
}
