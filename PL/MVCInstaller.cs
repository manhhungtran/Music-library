using System.Web.Mvc;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.WebHost;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PL.Helpers.Auth;

namespace PL
{
    public class MvcInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<SignInManager>()
                    .ImplementedBy<SignInManager>()
                    .LifestyleTransient(),

                Classes.FromThisAssembly()
                    .BasedOn<IController>()
                    .LifestyleTransient()
                    );
        }
    }
}
