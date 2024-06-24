namespace Application.Services
{
    public interface ITransactionService
    {
        Task ExecuteAsync(Func<Task> action);
        Task<T> ExecuteAsync<T>(Func<Task<T>> action);
    }
}
