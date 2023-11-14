namespace Bank_Managment_System.Controllers.Mediators.Interfaces
{
    public interface IcomandreportHandler<T,J>
    {
        Task<J> handle(T command);
    }
}
