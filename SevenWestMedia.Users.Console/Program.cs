using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SevenWestMediaTechInterview.Configuration;
using SevenWestMediaTechInterview.Extensions;

namespace SevenWestMediaTechInterview.Console
{
    /*
     * Please write a console application that outputs:
     *  The users full name for id=42
     *  All the users first names (comma separated) who are 23
     *  The number of genders per Age, displayed from youngest to oldest
     *  e.g
     *  Age : 17 Female: 2 Male: 3
     *  Age : 18 Female: 6 Male: 3
     *  
     *  Considerations
     *  The data source may change.
     *  The endpoint could go down.
     *  The endpoint has known to be slow in the past.
     *  The way source is fetched may change.
     *  The number of records may change (performance).
     *  The functionality may not always be consumed in a console app.     
     */

    public class Program
    {
        private static IUserService _userService;

        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            _userService = host.Services.GetService<IUserService>();

            int targetId = 53;
            await FindAndPrintUser(targetId);

            int targetAge = 23;
            await FindAndPrintAge(targetAge);

            await FindAndPrintGendersByAge();

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutoMapper(typeof(UsersAutoMapperProfile)))
                .ConfigureLogging(logger => logger.AddSimpleConsole())
                .ConfigureUserServices();

            return builder;
        }

        private static async Task FindAndPrintUser(int id)
        {
            var user = await _userService.GetUser(id);
            System.Console.WriteLine($"User {id}: { user?.FullName ?? "No result." }");
        }

        private static async Task FindAndPrintAge(int age)
        {
            var users = await _userService.GetUsers(user => user.Age == age);
            var givenNames = users?.Select(u => u.GivenName) ?? Enumerable.Empty<string>();
            System.Console.WriteLine($"Users aged {age}: { string.Join(", ", givenNames)}");
        }

        private static async Task FindAndPrintGendersByAge()
        {
            var userGenderCounts = await _userService.GetUserGenderCountsByAge();

            if (userGenderCounts.Count == 0)
            {
                System.Console.WriteLine("No users found per gender.");
            }
            else
            {
                foreach (var kvp in userGenderCounts)
                {
                    System.Console.Write($"Age : {kvp.Key} ");
                    var genderMapping = kvp.Value;
                    foreach (var gender in genderMapping.Keys.OrderBy(k => k))
                    {
                        System.Console.Write($"{gender} : {genderMapping[gender]} ");
                    }

                    System.Console.WriteLine();
                }
            }
        }
    }
}
