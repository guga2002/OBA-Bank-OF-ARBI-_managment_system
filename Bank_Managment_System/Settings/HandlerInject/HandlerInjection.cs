using System.Reflection;

namespace Bank_Managment_System.Settings.HandlerInject
{
    public static class HandlerInjection
    {
        public static void CmdhandlerInject(this IServiceCollection collect, Assembly assembly ,ServiceLifetime time=ServiceLifetime.Scoped)
        {
            var type = assembly.GetTypes().Where(
                io => !io.IsInterface && !io.IsAbstract && io.GetInterfaces().Any() &&
                io.Name.Contains("CMDHandler", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in type)
            {
                var interfaces = item.GetInterfaces().ToList();
                if(!interfaces.Any())
                {
                    throw new Exception("no interface exist");
                }
                else
                {
                    foreach (var interfa in interfaces)
                    {
                        collect.AddTransient(interfa, item);

                    }

                }
            }
        }

    }
}
