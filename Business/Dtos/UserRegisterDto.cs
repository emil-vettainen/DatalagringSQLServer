namespace Business.Dtos;

public class UserRegisterDto
{
    //public Guid Id { get; set; } = new Guid();
    //public DateTime Created { get; set; } = DateTime.Now;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!; 
    public string City { get; set; } = null!;   
    //public int AddressId { get; set; }
    public string RoleName { get; set; } = null!;
    //public int RoleId { get; set; }
}