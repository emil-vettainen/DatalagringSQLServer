using System.ComponentModel;

namespace Presentation.MAUI.Mvvm.Models.UserModels;

public class UserDetailsModel : INotifyPropertyChanged
{
    public Guid Id { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }

    private string _FirstName = null!;
    public string FirstName
    {
        get { return _FirstName; }
        set
        {
            _FirstName = value;
            RaisePropertyChanged(nameof(FirstName));
        }
    }
    private string _LastName = null!;
    public string LastName
    {
        get { return _LastName; }
        set
        {
            _LastName = value;
            RaisePropertyChanged(nameof(LastName));
        }
    }
    private string _Email = null!;
    public string Email
    {
        get { return _Email; }
        set
        {
            _Email = value;
            RaisePropertyChanged(nameof(Email));
        }
    }
   
    private string? _StreetName;
    public string? StreetName
    {
        get { return _StreetName; }
        set
        {
            _StreetName = value;
            RaisePropertyChanged(nameof(StreetName));
        }
    }
    private string? _PostalCode;
    public string? PostalCode
    {
        get { return _PostalCode; }
        set
        {
            _PostalCode = value;
            RaisePropertyChanged(nameof(PostalCode));
        }
    }
    private string? _City;
    public string? City
    {
        get { return _City; }
        set
        {
            _City = value;
            RaisePropertyChanged(nameof(City));
        }
    }
    private string _RoleName = null!;
    public string RoleName
    {
        get { return _RoleName; }
        set
        {
            _RoleName = value;
            RaisePropertyChanged(nameof(RoleName));
        }
    }

    private int _selectedRoleIndex;
    public int SlecectedRoleIndex
    {
        get { return _selectedRoleIndex; }
        set
        {
            _selectedRoleIndex = value;
            RaisePropertyChanged(nameof(SlecectedRoleIndex));
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
