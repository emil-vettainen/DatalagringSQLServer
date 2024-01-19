using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.UserEntities;

public class ProfileEntity
{
    [Key]
    [ForeignKey(nameof(UserEntity))]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}


