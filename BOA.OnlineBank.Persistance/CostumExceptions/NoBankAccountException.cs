namespace Bank_Managment_System.CostumExceptions
{
    public class NoBankAccountException:Exception
    {
        public NoBankAccountException(string str):base(str)
        {
        } 
    }
}
