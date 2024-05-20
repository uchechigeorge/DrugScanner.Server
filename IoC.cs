namespace DrugScanner.Server;

/// <summary>
/// The dependency injection container making use of the built .NET CORE service provider
/// </summary>
public static class IoCContainer
{
  /// <summary>
  /// The service provider of this application
  /// </summary>
  public static IServiceProvider? Provider { get; set; }
  /// <summary>
  /// The service provider of this application
  /// </summary>
  public static IWebHostEnvironment? Environment { get; set; }

  /// <summary>
  /// The configuration manager for the application
  /// </summary>
  public static IConfiguration? Configuration { get; set; }
}
