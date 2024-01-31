using Business.Services.ProductServices;
using Business.Services.UserServices;
using Infrastructure.Contexts;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presentation.MAUI.Mvvm.ViewModels;
using Presentation.MAUI.Mvvm.Views;
using Shared.Interfaces;
using Shared.Utilis;

namespace Presentation.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font-Awesome-Solid-900.OTF", "FAS");
                });

            builder.Services.AddDbContext<UserDataContext>(x => x.UseSqlServer(@"Data Source=192.168.50.2;Initial Catalog=user_db_v1;User ID=evettainen;Password=Emil2024!;Trust Server Certificate=True"));
            
            builder.Services.AddScoped<UserService>();

            builder.Services.AddSingleton<IErrorLogger>(new ErrorLogger(@"C:\CSharp\DatalagringSQLServer.log.txt"));
          
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<AuthenticationRepository>();
            builder.Services.AddScoped<ProfileRepository>();
            builder.Services.AddScoped<AddressRepository>();
            builder.Services.AddScoped<RoleRepository>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();

            builder.Services.AddTransient<UserDetailPage>();
            builder.Services.AddTransient<UserDetailViewModel>();



            builder.Services.AddDbContext<ProductDataContext>(x => x.UseSqlServer(@"Data Source=192.168.50.2;Initial Catalog=productcatalog_db_v1;Persist Security Info=True;User ID=evettainen;Password=Emil2024!;Encrypt=True;Trust Server Certificate=True"));
            builder.Services.AddScoped<ProductService>();

            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<ManufactureRepository>();
            builder.Services.AddScoped<ProductPriceRepository>();
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<ProductInfoRepository>();

            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddTransient<ProductViewModel>();

            builder.Services.AddTransient<AddProductPage>();
            builder.Services.AddTransient<AddProductViewModel>();

            builder.Services.AddTransient<ProductDetailPage>();
            builder.Services.AddTransient<ProductDetailViewModel>();

            builder.Services.AddTransient<EditProductPage>();
            builder.Services.AddTransient<EditProductViewModel>();

            builder.Logging.AddDebug();
            return builder.Build();
        }
    }
}
