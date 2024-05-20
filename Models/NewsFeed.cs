using System.ComponentModel.DataAnnotations;
using DrugScanner.Server.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugScanner.Server.Models;

#region Table

public class NewsFeed
{
  [Key]
  public int Id { get; set; }

  [StringLength(1024)]
  public string? Title { get; set; }

  [StringLength(1024)]
  public string? ImageUrl { get; set; }

  public string? Description { get; set; }

  public DateTime DateModified { get; set; }

  public DateTime DateCreated { get; set; }

  public DateTime? DateDeleted { get; set; }
}

#endregion

#region Config

public class NewsFeedConfiguration : IEntityTypeConfiguration<NewsFeed>
{
  public void Configure(EntityTypeBuilder<NewsFeed> builder)
  {
    builder.HasQueryFilter(e => e.DateDeleted == null);

    builder.Property(e => e.DateCreated).HasDefaultValueSql("(GETUTCDATE())");
    builder.Property(e => e.DateModified).HasDefaultValueSql("(GETUTCDATE())");
  }
}

#endregion

#region Helpers

public class GetNewsFeedParams : ViewHelper.GetParams
{
  public int? Id { get; set; }
}

#endregion