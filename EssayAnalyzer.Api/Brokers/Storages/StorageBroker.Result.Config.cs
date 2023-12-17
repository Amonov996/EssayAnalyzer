using EssayAnalyzer.Api.Models.Foundation.Results;
using Microsoft.EntityFrameworkCore;

namespace EssayAnalyzer.Api.Brokers.Storages;

public partial class StorageBroker
{
    private static void AddResultConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Result>()
            .HasOne(result => result.Essay)
            .WithOne(essay => essay.Result)
            .OnDelete(DeleteBehavior.Cascade);    
    }
}