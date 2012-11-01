using Castle.MicroKernel.ComponentActivator;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    public class ServiceOverideToDependsOn
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
        public void Old_ServiceOveride()
        {
            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>()
                    .ServiceOverrides(ServiceOverride.ForKey("parm1").Eq("depB"))
                );

            var service = _container.Resolve<IService>();

            Assert.That(service, Is.InstanceOf<ConcreteServiceA>());
            Assert.That(service.Dependency, Is.InstanceOf<DependencyB>());

        }

        [Test]
        public void New_DependsOn()
        {
            
            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>()
                    .DependsOn(Property.ForKey("parm1").Is("depB"))
                );
            
            var service = _container.Resolve<IService>();

            Assert.That(service, Is.InstanceOf<ConcreteServiceA>());
            Assert.That(service.Dependency, Is.InstanceOf<DependencyB>());

        }

        [Test]
        public void Warning_Is_Vs_Eq()
        {
            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>()
                    .DependsOn(Property.ForKey("parm1").Is("depB"))  // <--- Is
                );
            
            Assert.DoesNotThrow(() => _container.ResolveAll<IService>());

            
            _container.Register(Component.For<IService>().ImplementedBy<ConcreteServiceB>()
                    .DependsOn(Property.ForKey("parm1").Eq("depB.1"))  // <--- Eq
                );

            // Is: looks for a service with that key
            // Eq: Tries to use the exact object as it was passed 
            Assert.Throws<ComponentActivatorException>(() => _container.ResolveAll<IService>());

        }
        


    }

    
   
}
