using DrugScanner.Server.Models;

namespace DrugScanner.Server.Services;

public partial class DrugsService(ApplicationDbContext dbContext) : IDrugsService
{

  private readonly ApplicationDbContext mDbContext = dbContext;
}