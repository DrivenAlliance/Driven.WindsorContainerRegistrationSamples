using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class ContainerDefaults
    {
        [Test]
        public void AvoidSpecifyingDefaults_Singleton()
        {
            var c = new WindsorContainer();
            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>());

            var item1 = c.Resolve<IService>();
            var item2 = c.Resolve<IService>();

            Assert.That(item1, Is.SameAs(item2));  // Default lifestyle is singleton
        }

        [Test]
        public void AvoidSpecifyingDefaults_FullName()
        {
            var c = new WindsorContainer();
            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceA>());

            var item = c.Resolve<IService>(typeof(ConcreteServiceA).FullName);
            
            Assert.That(item, Is.InstanceOf<ConcreteServiceA>()); // Default name is type's FullName 
        }

        [Test]
        public void Avoid_ExcludingInterfaces()
        {
            var c = new WindsorContainer();
            
            c.Register(Classes.FromThisAssembly()
                .Pick().If(t => !t.IsInterface) // here we exclude interfaces
                .WithService.FirstInterface()
                );

            var totalComponentsExcludingInterfaces = c.Kernel.ConfigurationStore.GetComponents().Length;

            c = new WindsorContainer();

            c.Register(Classes.FromThisAssembly()
                .Pick() // here we don' exclude interfaces
                .WithService.FirstInterface()
                );

            var totalComponents = c.Kernel.ConfigurationStore.GetComponents().Length;


            Assert.That(totalComponents, Is.EqualTo(totalComponentsExcludingInterfaces)); // excluding interfaces has no effect
        }
    }
}
