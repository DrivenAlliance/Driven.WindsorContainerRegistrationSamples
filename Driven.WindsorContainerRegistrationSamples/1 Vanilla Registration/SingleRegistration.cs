using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class SingleRegistration
    {
        [Test]
        public void CanRegisterSimpleService()
        {
            var c = new WindsorContainer();
            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());

            Assert.That(c.Resolve<IDependency>(), Is.InstanceOf<DependencyA>());
        }

        [Test]
        public void CanAutoWire()
        {
            var c = new WindsorContainer();
            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>());

            var service = c.Resolve<IService>();

            Assert.That(service, Is.InstanceOf<ConcreteServiceA>());
            Assert.That(service.Dependency, Is.InstanceOf<DependencyA>());
        }
    }
}
