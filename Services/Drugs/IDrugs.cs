using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Services;

public interface IDrugsService
{

  Task<ViewQuery<Drug>> GetDrugs(GetDrugParams parameters);

}