using System.Reflection;

namespace Bank_Managment_System.Settings.Reflections.ServiceInject
{
    public static class ServiceInject
    {
        public static void InjectService(this IServiceCollection collect,Assembly asembly,ServiceLifetime life=ServiceLifetime.Scoped)
        {
            var listof = Assembly.Load(new AssemblyName("BOA.OnlineBank.Core"));

            if (listof == null)
            {
                Console.WriteLine(" no  module load");
            }
            else
            {
                var getclasses = listof.GetTypes().Where(
                    io => !io.IsAbstract && !io.IsGenericTypeDefinition && !io.IsAbstract
                    && io.GetInterfaces().Any() && io.Name.Contains("Service", StringComparison.OrdinalIgnoreCase)).ToList();

                if (!getclasses.Any())
                {
                    throw new Exception("No such c base classes exist");
                }
                foreach (var item in getclasses)
                {
                    var inter = item.GetInterfaces().ToList();
                    if (!inter.Any())
                    {
                        throw new Exception(" Interface Not exist!!");
                    }
                    foreach (var interfac in inter)
                    {
                        collect.AddScoped(interfac, item);

                    }
                }
            }
        }
    }
}
