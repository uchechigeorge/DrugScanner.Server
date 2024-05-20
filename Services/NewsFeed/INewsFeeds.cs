using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Services;

public interface INewsFeedsService
{

  Task<ViewQuery<NewsFeed>> GetNewsFeeds(GetNewsFeedParams parameters);

}