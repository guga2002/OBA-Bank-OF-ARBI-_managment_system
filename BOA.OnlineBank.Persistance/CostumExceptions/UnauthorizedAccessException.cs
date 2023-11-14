namespace Bank_Managment_System.CostumExceptions
{
    public class UnauthorizedAccessException:Exception
    {
        public UnauthorizedAccessException(string txt, Exception iner) : base(txt,iner)
        {    
        }
    }
}
