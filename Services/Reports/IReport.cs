using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Services;

public interface IReportsService
{

  Task<ViewQuery<Report>> GetReports(GetReportParams parameters);

}