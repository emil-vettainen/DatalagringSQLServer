using Presentation.MAUI.Mvvm.Views;
using Shared.Helper;

namespace Presentation.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
            Routing.RegisterRoute(nameof(EditProductPage), typeof(EditProductPage));
        }

        private async void SignOutBtn_Clicked(object sender, EventArgs e)
        {
            AppState.IsAuthenticated = false;
            AppState.UserId = Guid.Empty;
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}