using System;
using System.Data.Entity;
using BL.AppInfrastructure;
using BL.Repositories.UserAccount;
using BL.Services;
using BL.Services.User;
using BrockAllen.MembershipReboot;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DAL;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using UserAccount = DAL.Entities.UserAccount;

namespace BL.Base
{
    public class BussinessLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                Component.For<Func<DbContext>>()
                    .Instance(() => new AppDbContext())
                    .LifestyleTransient(),

                Component.For<IUnitOfWorkProvider>()
                    .ImplementedBy<AppUnitOfWorkProvider>()
                    .LifestyleSingleton(),

                Component.For<IUnitOfWorkRegistry>()
                    .Instance(new HttpContextUnitOfWorkRegistry(new ThreadLocalUnitOfWorkRegistry()))
                    .LifestyleSingleton(),

                Component.For<IUserAccountRepository<UserAccount>>()
                    .ImplementedBy<UserAccountManager>()
                    .LifestyleTransient(),

                Component.For<UserAccountService<UserAccount>>()
                    .ImplementedBy<UserAccountService<UserAccount>>()
                    .DependsOn(Dependency.OnComponent<IUserAccountRepository<UserAccount>, UserAccountManager>())
                    .LifestyleTransient(),

                 Component.For<AuthenticationWrapper>()
                    .ImplementedBy<AuthenticationWrapper>()
                    .DependsOn(Dependency.OnComponent<UserAccountService<UserAccount>, UserAccountService<UserAccount>>())
                    .LifestyleTransient(),

                Component.For(typeof(IRepository<,>))
                    .ImplementedBy(typeof(EntityFrameworkRepository<,>))
                    .LifestyleTransient(),

                Classes.FromAssemblyContaining<AppUnitOfWork>()
                    .BasedOn(typeof(AppQuery<>))
                    .LifestyleTransient(),

                Classes.FromAssemblyContaining<AppUnitOfWork>()
                    .BasedOn(typeof(IRepository<,>))
                    .LifestyleTransient(),

                Classes.FromThisAssembly()
                    .BasedOn<MusicLibraryService>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),

                Classes.FromThisAssembly()
                    .InNamespace("BL.Facades")
                    .LifestyleTransient()
            );

            // add collection subresolver in order to resolve IEnumerable in Price Calculator Resolver
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
        }
    }
}
