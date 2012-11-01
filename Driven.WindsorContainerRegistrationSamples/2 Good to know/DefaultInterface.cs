using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    [TestFixture]
    public class DefaultInterface
    {
        [Test]
        public void RegisterWith_First_Interface()
        {
            var c = new WindsorContainer();

            c.Register(
               Classes.FromAssembly(Assembly.GetExecutingAssembly()).Pick()
                   .If(t => t.Namespace.EndsWith("SampleCode"))
                   .WithService.FirstInterface()
               );

            Assert.That(c.Resolve<IService>(), Is.InstanceOf<ConcreteServiceA>()); // Registered by first implemented interface 
            Assert.Throws<ComponentNotFoundException>(() => c.Resolve<IConcreteServiceA>()); // Not registered by second interface
        }

        [Test]
        public void RegisterWith_Default_Interface()
        {
            var c = new WindsorContainer();

            c.Register(
               Classes.FromAssembly(Assembly.GetExecutingAssembly()).Pick()
                   .If(t => t.Namespace.EndsWith("SampleCode"))
                   .WithService.DefaultInterfaces()
               );

            Assert.That(c.Resolve<IService>(), Is.InstanceOf<ConcreteServiceA>()); // Registered by first implemented interface 
            Assert.That(c.Resolve<IConcreteServiceA>(), Is.InstanceOf<ConcreteServiceA>()); // Registered by default interface I<class name>

            /*
             
             Hmmm. Why didn't we get a ComponentNotFoundException when resolving for IService? DefaultInterfaces are all interfaces that, 
             after you remove the 'I', the class name *contains* the interface name. So for IService --> ConcreteServiceA contains the string 'Service', 
             so will register with that interface as well as IConcreteServiceA
             
             * */
            
        }

        [Test]
        public void RegisterWith_All_Interfaces()
        {
            var c = new WindsorContainer();

            c.Register(
               Classes.FromAssembly(Assembly.GetExecutingAssembly()).Pick()
                   .If(t => t.Namespace.EndsWith("SampleCode"))
                   .WithService.AllInterfaces()
               );

            Assert.That(c.Resolve<IService>(), Is.InstanceOf<ConcreteServiceA>()); 
            Assert.That(c.Resolve<IConcreteServiceA>(), Is.InstanceOf<ConcreteServiceA>()); 
            
        }

    }
}
