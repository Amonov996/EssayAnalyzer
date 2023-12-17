using EssayAnalyzer.Api.Models.Foundation.Essays;
using Microsoft.EntityFrameworkCore;

namespace EssayAnalyzer.Api.Brokers.Storages;

public partial class StorageBroker
{
    private static void AddEssayConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Essay>()
            .HasOne(user => user.User)
            .WithMany(essay => essay.Essays)
            .HasForeignKey(essay => essay.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}