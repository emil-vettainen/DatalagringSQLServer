using Presentation.MAUI.Mvvm.Views;
using Shared.Helper;

namespace Presentation.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(UserDetailPage), typeof(UserDetailPage));
            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
        }

        private async void SignOutBtn_Clicked(object sender, EventArgs e)
        {
            AppState.IsAuthenticated = false;
            AppState.UserId = Guid.Empty;


            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
