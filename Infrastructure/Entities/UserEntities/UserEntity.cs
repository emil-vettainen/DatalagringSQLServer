using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.UserEntities;

public class UserEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Created { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Modified { get; set; }

    [Required]
    [ForeignKey(nameof(RoleEntity))]
    public int RoleId { get; set; }

    [Required]
    [ForeignKey(nameof(AddressEntity))]
    public int AddressId { get; set; }

    public virtual RoleEntity Role { get; set; } = null!;

    public virtual AddressEntity Address { get; set; } = null!;

    public virtual ProfileEntity Profile { get; set; } = null!;

    public virtual AuthenticationEntity Authentication { get; set; } = null!;
}


