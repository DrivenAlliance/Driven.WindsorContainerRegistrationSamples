using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.ComponentActivator;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    public class ConvertingFromObsoleteServiceOverides
    {
        private IWindsorContainer _container;

        [SetUp]
        public void Given()
        {
            _container = new WindsorContainer();

            _container.Register(
                Component.For<IDependency>().ImplementedBy<DependencyA>(),
                Component.For<IDependency>().ImplementedBy<DependencyB>().Named("depB")
                );
        }

        [Test]
        public void RegisteringSingleDependency_Old_ServiceOveride()
        {
            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>()
                                    .ServiceOverrides(ServiceOverride.ForKey("parm1").Eq("depB"))
                );

            var service = _container.Resolve<IService>();

            Assert.That(service, Is.InstanceOf<ConcreteServiceA>());
            Assert.That(service.Dependency, Is.InstanceOf<DependencyB>());

        }

        [Test]
        public void RegisteringSingleDependency_New_DependsOn()
        {

            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>()
                                    .DependsOn(Property.ForKey("parm1").Is("depB"))
                );

            var service = _container.Resolve<IService>();

            Assert.That(service, Is.InstanceOf<ConcreteServiceA>());
            Assert.That(service.Dependency, Is.InstanceOf<DependencyB>());

        }

        [Test]
        public void WatchOutForSubtley_Is_Vs_Eq()
        {
            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>()
                                    .DependsOn(Property.ForKey("parm1").Is("depB")) // <--- Is
                );

            Assert.DoesNotThrow(() => _container.ResolveAll<IService>());


            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceB>()
                                    .DependsOn(Property.ForKey("parm1").Eq("depB.1")) // <--- Eq
                );

            // Is: looks for a service with that key
            // Eq: Tries to use the exact object as it was passed 
            Assert.Throws<ComponentActivatorException>(() => _container.ResolveAll<IService>());

        }

        [Test]
        public void RegisteringMultipleDependencies_Old_ServiceOverride()
        {

            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));
            
            _container.Register(Component.For<IConcreteServiceE>().ImplementedBy<ConcreteServiceE>()

                                    .ServiceOverrides(
                                        ServiceOverride.ForKey("dependencies")
                                            .Eq(new[]
                                                    {
                                                        "depB"
                                                        // there would be more items here
                                                    }
                                               )
                                    )
                );

            var service = _container.Resolve<IConcreteServiceE>();

            Assert.That(service.Dependencies.Count, Is.EqualTo(1));

            Assert.That(service.Dependencies.First(), Is.InstanceOf<DependencyB>());

        }

        [Test]
        public void RegisteringMultipleDependencies_New_DynamicParameters()
        {
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));

            _container.Register(Component.For<IConcreteServiceE>().ImplementedBy<ConcreteServiceE>()
                .DynamicParameters((k, p) =>
                    {
                        var deps = new List<IDependency>
                                    {
                                        k.Resolve<IDependency>("depB")
                                        // resolve and add more items to the list 
                                    };
                        
                        p["dependencies"] = deps;
                    })
                );

            var service = _container.Resolve<IConcreteServiceE>();

            Assert.That(service.Dependencies.Count, Is.EqualTo(1));

            Assert.That(service.Dependencies.First(), Is.InstanceOf<DependencyB>());

        }

    }

}
