using Application.Services;
using Infrastructure.Persistence.CampaignDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services
{
    public class TransactionService(CampaignContext context) : ITransactionService
    {
        public async Task ExecuteAsync(Func<Task> action)
        {
            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    await action();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    context.ChangeTracker.Clear();
                    throw;
                }
            });
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    T result = await action();

                    await transaction.CommitAsync();

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    context.ChangeTracker.Clear();
                    throw;
                }
            });
        }
    }
}