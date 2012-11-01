using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.Lib;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class MixingLifestyles
    {
        [Test]
        public void SingletonDependenciesOfTransientServices()
        {
            var c = new WindsorContainer();

            c.RegisterTransient<IService, ConcreteServiceA>();
            c.RegisterSingleton<IDependency, DependencyA>();

            var s1 = c.Resolve<IService>();
            var s2 = c.Resolve<IService>();

            Assert.That(s1, Is.Not.SameAs(s2));
            Assert.That(s1.Dependency, Is.SameAs(s2.Dependency)); // Dependency 'behaves' like a singleton (as expected)
        }
        
        [Test]
        public void TransientDependenciesOfSingletonServices()
        {
            var c = new WindsorContainer();

            c.RegisterSingleton<IService, ConcreteServiceA>();
            c.RegisterTransient<IDependency, DependencyA>();

            var s1 = c.Resolve<IService>();
            var s2 = c.Resolve<IService>();

            Assert.That(s1, Is.SameAs(s2));
            Assert.That(s1.Dependency, Is.SameAs(s2.Dependency)); // Dependency 'behaves' like a singleton. Oops.
        }
       
    }
}
