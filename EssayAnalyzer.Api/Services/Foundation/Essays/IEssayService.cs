using EssayAnalyzer.Api.Models.Foundation.Essays;

namespace EssayAnalyzer.Api.Services.Foundation.Essays;

public partial interface IEssayService
{
  public ValueTask<Essay> AddEssayAsync(Essay essay);
  public ValueTask<IQueryable<Essay>> RetrieveEssaysAsync();
  public ValueTask<Essay> RetrieveEssayByIdAsync(Guid id);
  public ValueTask<Essay> RemoveEssayByIdAsync(Guid id);
}