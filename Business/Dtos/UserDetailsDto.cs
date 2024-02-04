namespace Business.Dtos;

public class UserDetailsDto
{
    public Guid Id { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified {  get; set; }
    public string RoleName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Email { get; set; } = null!;
}
