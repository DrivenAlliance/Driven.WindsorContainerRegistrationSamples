using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class RetrievingFromContainerDuringRegistration
    {
        [Test]
        public void CallContainerDuringRegistration()
        {
            // We don't want to resolve things from the container whilst registration is still happening because this puts a dependency on the
            // order in which things are registered.

            // If you step through this code in the debugger you will see that k.Resolve<> is only called at the point 
            // at which Resolve<IService> is called

            var c = new WindsorContainer();
            c.Register(Component
                           .For<IService>()
                           .ImplementedBy<ConcreteServiceA>()
                           .DynamicParameters((k, p) => p["dep1"] = k.Resolve<IDependency>("dependencyA")));
            
            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>().Named("dependencyA"));


            Assert.That(c.Resolve<IService>(), Is.InstanceOf<ConcreteServiceA>());
            Assert.That(c.Resolve<IService>().Dependency, Is.InstanceOf<DependencyA>());

        }

    }
}
