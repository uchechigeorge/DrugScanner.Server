using DrugScanner.Server.Models;

namespace DrugScanner.Server.Services;

public partial class ReportsService(ApplicationDbContext dbContext) : IReportsService
{

  private readonly ApplicationDbContext mDbContext = dbContext;
}