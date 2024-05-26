using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DrugScanner.Server.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugScanner.Server.Models;

#region Table

public class Drug
{
  [Key]
  public int Id { get; set; }

  [StringLength(1024)]
  public string? Name { get; set; }

  [StringLength(1024)]
  public string? Code { get; set; }

  public int StatusId { get; set; }

  [NotMapped]
  public string? Status { get; set; }

  [StringLength(1024)]
  public string? ManufacturedBy { get; set; }

  public DateTime? ManufacturingDate { get; set; }

  public DateTime? ExpirationDate { get; set; }

  [StringLength(1024)]
  public string? BatchNo { get; set; }

  public bool IsScanned { get; set; }

  public int NoOfScans { get; set; }

  public DateTime DateModified { get; set; }

  public DateTime DateCreated { get; set; }

  public DateTime? DateDeleted { get; set; }
}

#endregion

#region Config

public class DrugConfiguration : IEntityTypeConfiguration<Drug>
{
  public void Configure(EntityTypeBuilder<Drug> builder)
  {
    builder.HasQueryFilter(e => e.DateDeleted == null);

    builder.Property(e => e.NoOfScans).HasDefaultValue(0);
    builder.Property(e => e.DateCreated).HasDefaultValueSql("(GETUTCDATE())");
    builder.Property(e => e.DateModified).HasDefaultValueSql("(GETUTCDATE())");
  }
}

#endregion

#region Helpers

public class GetDrugParams : ViewHelper.GetParams
{
  public int? Id { get; set; }
}

#endregion