using System.Reflection;
namespace Bank_Managment_System.Settings.Reflections.RepositInject
{
    public static class ReposInject
    {
        public static  void ReposInjecti(this IServiceCollection collection, Assembly asembly,ServiceLifetime life=ServiceLifetime.Scoped)
        {
            var loadrepos = Assembly.Load("BOA.OnlineBank.Infrastructure");
            var loaderrorlog = Assembly.Load("BOA.OnlineBank.Presenatation");
            if (loadrepos == null || loaderrorlog == null)
            {
                throw new Exception(" no  Library exist");
            }

            var repos = loadrepos.GetTypes().Where
                (io => !io.IsGenericTypeDefinition && !io.IsAbstract && !io.IsInterface &&
                io.GetInterfaces().Any() && io.Name.Contains("Repositorie", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var baserep in repos)
            {
                var interfaces = baserep.GetInterfaces().ToList();
                if(!interfaces.Any())
                {
                    throw new Exception("No interface Exist");
                }
                foreach (var Interf in interfaces)
                {
                    collection.AddScoped(Interf, baserep);
                }
            }

            var reposerrorlog=loaderrorlog.GetTypes().Where
                (io => !io.IsGenericTypeDefinition && !io.IsAbstract && !io.IsInterface &&
                io.GetInterfaces().Any() && io.Name.Contains("Repositorie", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in reposerrorlog)
            {
                var interfaces = item.GetInterfaces().ToList();
                if (!interfaces.Any())
                {
                    throw new Exception("No interface Exist");
                }
                foreach (var Interf in interfaces)
                {
                    collection.AddScoped(Interf, item);
                }
            }

        }
       
    }
}
