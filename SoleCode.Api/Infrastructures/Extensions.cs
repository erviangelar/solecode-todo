using SoleCode.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace SoleCode.Api.Infrastructures
{
    public static class Extensions
    {
        public static string AppVersion = "1.0.0";

        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<EntityDbContext>();
            if (context == null) throw new InvalidOperationException("Context Is Null");
            context.Database.Migrate();

            return builder;
        }

        public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<EntityDbContext>();
            if (context == null) throw new InvalidOperationException("Context Is Null");
            InitDataDummy(context);
            return builder;
        }

        private static void InitDataDummy(EntityDbContext context)
        {
            if (!File.Exists(@"Dummy/Init-Dummy.txt")) return;
            var dStrings = File.ReadAllLines(@"Dummy/Init-Dummy.txt");
            foreach (var dString in dStrings)
            {
                var fields = dString.Split(";");
                if (fields.Length <= 1) continue;
                var user = context.User.FirstOrDefault(x => x.Username == fields[1]);
                if (user == null)
                {
                    context.User.Add(new User
                    {
                        UID = fields[0] == string.Empty ? Guid.NewGuid() : new Guid(fields[0]),
                        Username = fields[1],
                        Password = fields[2]
                    });
                }
            }
            context.SaveChanges();
        }
        public static IApplicationBuilder UseFailureMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FailureMiddleware>();
        }
    }
}
