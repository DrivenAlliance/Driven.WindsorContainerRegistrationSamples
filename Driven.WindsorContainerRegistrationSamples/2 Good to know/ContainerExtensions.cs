using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.Lib;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class ContainerExtensions
    {
        [Test]
        public void ReplaceDefaultRegistrationsWithSimplifiedExtention()
        {
            // By adding an extention method we can remove a lot of repetitive code (but we do lose the nice fluent readability at the same time)

            var c = new WindsorContainer();
            c.RegisterSingleton<IService, ConcreteServiceA>();

            // means the same as...
            c = new WindsorContainer();
            c.Register(Component
                           .For<IService>()
                           .ImplementedBy<ConcreteServiceA>()
                           .Named(typeof (ConcreteServiceA).FullName)
                           .LifestyleSingleton());
            
            // there is also a Transient and PerWebRequest options

        }

        [Test]
        public void SimpleRegistrationWithName()
        {
            var c = new WindsorContainer();
            c.RegisterSingleton<IDependency, DependencyA>()
             .RegisterSingleton<IService, ConcreteServiceA>("my service name A")
             .RegisterSingleton<IService, ConcreteServiceB>("my service name B");

            Assert.That(c.Resolve<IService>("my service name A"), Is.InstanceOf<ConcreteServiceA>());

        }
    
    }
}
