using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.UserEntities;

[Index(nameof(Email), IsUnique = true)]
public class AuthenticationEntity
{
    [Key]
    [ForeignKey(nameof(UserEntity))]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Email { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string PasswordKey { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}


