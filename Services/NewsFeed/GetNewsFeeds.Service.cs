using Microsoft.EntityFrameworkCore;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Services;

public partial class NewsFeedsService : INewsFeedsService
{

  #region Methods

  public async Task<ViewQuery<NewsFeed>> GetNewsFeeds(GetNewsFeedParams parameters)
  {

    var sortOptions = new Dictionary<string, string>
      {
        { "Title", "" },
        { "DateModified", "" },
        { "DateCreated", "" },
      };

    // Available column options for search
    var searchColumnsOptions = new Dictionary<string, string>
    {
      { "Title", "" }
    };

    /// Default columns for search
    var defaultSearchColumn = "Title";

    var query = mDbContext.NewsFeeds.AsQueryable();

    if (parameters.Id != null)
    {
      query = query.Where(e => e.Id == parameters.Id);
    }

    if (ViewHelper.HasSearchParameters(parameters.SearchColumns, parameters.SearchStrings))
    {
      var searchBuilder = ViewHelper.GetSearchQueryBuilder(parameters.SearchColumns, parameters.SearchStrings, parameters.SearchOperators, parameters.SearchStack, searchColumnsOptions, defaultSearchColumn);

      var filter = searchBuilder.GetExpression<NewsFeed>();

      if (filter != null)
      {
        query = query.Where(filter);
      }
    }

    int total = await query.CountAsync();

    query = query.OrderBy(ViewHelper.GetSort(parameters.Sort ?? "", sortOptions), ViewHelper.IsDescendingOrder(parameters.Order ?? ""));
    query = query.Skip(parameters.PageSize.GetNaturalInt() * (parameters.Page.GetNaturalInt() - 1)).Take(parameters.PageSize.GetNaturalInt());

    var data = await query.ToListAsync();
    return new ViewQuery<NewsFeed> { Query = query, Data = data, Total = total };
  }

  #endregion

}