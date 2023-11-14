namespace Bank_Managment_System.Controllers.Mediators.Interfaces
{
    public interface IcomandHandler<T>
    {
        Task<int> Handle(T comand);
    }
}
