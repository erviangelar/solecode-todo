using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoleCode.Api.Entities;

namespace SoleCode.Test
{
    public class InitTest : IDisposable
    {
        public EntityDbContext _context { get; set; }
        public InitTest()
        {
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);
                if (jObject == null) throw new Exception("failed load config");
                var variables = jObject
                    .GetValue("profiles")
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
            _context = DataDbContextInstantiate();
        }

        protected static EntityDbContext DataDbContextInstantiate()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<EntityDbContext>();
            var options = dbContextOptionsBuilder.UseInMemoryDatabase("FAKE").Options;
            var context = new EntityDbContext(options);
            context.Database.EnsureDeleted();
            return context;
        }

        public void Dispose()
        {
            //_context.Dispose();
        }
    }
}
