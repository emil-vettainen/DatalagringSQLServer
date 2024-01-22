using Business.Services.UserServices;
using Infrastructure.Contexts;
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

            builder.Services.AddDbContext<UserDataContexts>(x => x.UseSqlServer(@"Data Source=192.168.50.2;Initial Catalog=user_db_v1;User ID=evettainen;Password=Emil2024!;Trust Server Certificate=True"));
            
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


            builder.Logging.AddDebug();
            return builder.Build();
        }
    }
}
