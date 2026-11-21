using System.Reflection;
using Autofac;

namespace AmplaData.Web.Modules
{
    public class ControllerInjectionModule : Autofac.Module
    {
        private readonly Assembly assembly;

        public ControllerInjectionModule(Assembly assembly)
        {
            this.assembly = assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // ASP.NET Core discovers controllers automatically.
            // No RegisterControllers() equivalent is needed here.
        }
    }
}