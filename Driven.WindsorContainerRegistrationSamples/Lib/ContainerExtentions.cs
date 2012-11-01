using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Driven.WindsorContainerRegistrationSamples.Lib
{
    public static class ContainerExtensions
    {
        public static IWindsorContainer RegisterSingleton<TService, TImpl>(this IWindsorContainer container)
            where TService : class
            where TImpl : TService
        {
            return register<TService, TImpl>(container, LifestyleType.Singleton);
        }

        public static IWindsorContainer RegisterSingleton<TService, TImpl>(this IWindsorContainer container, string named)
            where TService : class
            where TImpl : TService
        {
            return register<TService, TImpl>(container, LifestyleType.Singleton, named);
        }

        public static IWindsorContainer RegisterTransient<TService, TImpl>(this IWindsorContainer container)
            where TService : class
            where TImpl : TService
        {
            return register<TService, TImpl>(container, LifestyleType.Transient);
        }

        public static IWindsorContainer RegisterPerWebRequest<TService, TImpl>(this IWindsorContainer container)
            where TService : class
            where TImpl : TService
        {
            return register<TService, TImpl>(container, LifestyleType.PerWebRequest);
        }



        static IWindsorContainer register<TService, TImpl>(IWindsorContainer container, LifestyleType lifestyle, string named = "")
            where TService : class
            where TImpl : TService
        {
            return container.Register(
                named == string.Empty ?
                Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.Is(lifestyle) :
                Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.Is(lifestyle).Named(named)
                );
        }
    }
}