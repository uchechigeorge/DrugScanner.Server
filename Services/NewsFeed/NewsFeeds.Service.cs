using DrugScanner.Server.Models;

namespace DrugScanner.Server.Services;

public partial class NewsFeedsService(ApplicationDbContext dbContext) : INewsFeedsService
{

  private readonly ApplicationDbContext mDbContext = dbContext;
}