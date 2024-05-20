using Microsoft.EntityFrameworkCore;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Services;

public partial class DrugsService : IDrugsService
{

  #region Methods

  public async Task<ViewQuery<Drug>> GetDrugs(GetDrugParams parameters)
  {

    var sortOptions = new Dictionary<string, string>
      {
        { "Name", "" },
        { "DateModified", "" },
        { "DateCreated", "" },
      };

    // Available column options for search
    var searchColumnsOptions = new Dictionary<string, string>
    {
      { "Name", "" }
    };

    /// Default columns for search
    var defaultSearchColumn = "Name";

    var query = mDbContext.Drugs.AsQueryable();

    if (parameters.Id != null)
    {
      query = query.Where(e => e.Id == parameters.Id);
    }

    if (ViewHelper.HasSearchParameters(parameters.SearchColumns, parameters.SearchStrings))
    {
      var searchBuilder = ViewHelper.GetSearchQueryBuilder(parameters.SearchColumns, parameters.SearchStrings, parameters.SearchOperators, parameters.SearchStack, searchColumnsOptions, defaultSearchColumn);

      var filter = searchBuilder.GetExpression<Drug>();

      if (filter != null)
      {
        query = query.Where(filter);
      }
    }

    int total = await query.CountAsync();

    query = query.OrderBy(ViewHelper.GetSort(parameters.Sort ?? "", sortOptions), ViewHelper.IsDescendingOrder(parameters.Order ?? ""));
    query = query.Skip(parameters.PageSize.GetNaturalInt() * (parameters.Page.GetNaturalInt() - 1)).Take(parameters.PageSize.GetNaturalInt());

    var data = await query.ToListAsync();
    return new ViewQuery<Drug> { Query = query, Data = data, Total = total };
  }

  #endregion

}