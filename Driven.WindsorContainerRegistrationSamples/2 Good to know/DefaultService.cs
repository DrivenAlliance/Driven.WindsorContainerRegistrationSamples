using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class DefaultService
    {
        [Test]
        public void ChangeDefaultService()
        {
            var c = new WindsorContainer();
            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceB>());

            Assert.That(c.Resolve<IService>(), Is.InstanceOf<ConcreteServiceA>()); // First registered item is returned by default


            c = new WindsorContainer();
            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceB>().IsDefault());  // <===

            Assert.That(c.Resolve<IService>(), Is.InstanceOf<ConcreteServiceB>()); // But we can change that. Container will always return last specified default
        }
    }
}
