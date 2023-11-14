namespace Bank_Managment_System.Controllers.Mediators.Interfaces
{
    public interface IcomandHandleForStringReturn<T>
    {
        Task<string> handle(T comand);
    }
}
