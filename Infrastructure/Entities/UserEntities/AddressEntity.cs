using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.UserEntities;

public class AddressEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? StreetName { get; set; }

    [Column(TypeName = "char(6)")]
    public string? PostalCode { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? City { get; set; }

    public virtual ICollection<UserEntity> Addresses { get; set; } = new HashSet<UserEntity>();
}


