using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{

    services.AddDbContext<UserDataContext>(x => x.UseSqlServer(@"Data Source=192.168.50.2;Initial Catalog=user_db_v1;User ID=evettainen;Password=Emil2024!;Trust Server Certificate=True"));

}).Build();