using EssayAnalyzer.Api.Models.Foundation.Results;
using Microsoft.EntityFrameworkCore;

namespace EssayAnalyzer.Api.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<Result> Results { get; set; }

    public IQueryable<Result> SelectAllResults() =>
        SelectAll<Result>();
    
    public async ValueTask<Result> InsertResultAsync(Result result) =>
        await InsertAsync(result);

    public async ValueTask<Result> SelectResultByIdAsync(Guid id) =>
        await SelectAsync<Result>(id);
}