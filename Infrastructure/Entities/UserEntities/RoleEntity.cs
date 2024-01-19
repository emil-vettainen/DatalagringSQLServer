using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.UserEntities;

[Index(nameof(RoleName), IsUnique = true)]
public class RoleEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserEntity> Roles { get; set; } = new HashSet<UserEntity>();
}


