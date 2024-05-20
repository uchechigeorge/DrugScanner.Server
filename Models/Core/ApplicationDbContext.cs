using System.Reflection;
using Microsoft.EntityFrameworkCore;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Models;

public class ApplicationDbContext : DbContext
{

  #region Constructor

  public ApplicationDbContext()
  {

  }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
  {
  }

  #endregion

  #region Properties

  public virtual DbSet<Drug> Drugs { get; set; }
  public virtual DbSet<NewsFeed> NewsFeeds { get; set; }
  public virtual DbSet<Report> Reports { get; set; }

  #endregion

  #region  Methods

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);

    if (IoCContainer.Configuration != null)
    {
      if ((IoCContainer.Configuration["Db:EnableSensitiveDataLogging"] ?? "").GetBoolean())
      {
        optionsBuilder.EnableSensitiveDataLogging();
      }
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    modelBuilder.HasDefaultSchema("dbo");
  }

  #endregion

}