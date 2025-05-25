using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiCarsBids.Data.Entities;

[Table("tblCategories")]
public class CategoryEntity : BaseEntity<long>
{
    [StringLength(250)]
    public string Name { get; set; } = String.Empty;

    [StringLength(200)]
    public string Image { get; set; } = String.Empty;

    [StringLength(250)]
    public string? Description { get; set; }
}
