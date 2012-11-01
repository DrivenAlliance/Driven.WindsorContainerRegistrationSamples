using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class ResolvingProperties
    {
        [Test]
        public void CanInjectProperties()
        {
            var c = new WindsorContainer();

            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());
            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceC>());

            var service = c.Resolve<IService>();

            Assert.That(service.Dependency, Is.InstanceOf<DependencyA>());
        }

        [Test]
        public void UsingTheNullObjectPattern()
        {
            /*
             
             The NullObject pattern is useful when you have an optional dependency. You provide a default 'null' implementation 
             that just ignores all calls to it. This can be replaced using property injection. This keeps your code cleaner and 
             avoids the need for null checks all over the place
              
             * */

            var c = new WindsorContainer();

            c.Register(Component.For<IService>().ImplementedBy<ConcreteServiceD>().LifestyleTransient());

            Assert.That(c.Resolve<IService>().Dependency, Is.InstanceOf<NullDependency>());

            c.Register(Component.For<IDependency>().ImplementedBy<DependencyA>());
            
            Assert.That(c.Resolve<IService>().Dependency, Is.InstanceOf<DependencyA>());
        }

    }
}
