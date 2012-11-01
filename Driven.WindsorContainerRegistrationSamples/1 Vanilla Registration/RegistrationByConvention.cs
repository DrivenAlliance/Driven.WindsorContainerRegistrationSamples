using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Driven.WindsorContainerRegistrationSamples.SampleCode;
using NUnit.Framework;

namespace Driven.WindsorContainerRegistrationSamples
{
    /// <summary>
    /// See here for more: http://docs.castleproject.org/Windsor.Registering-components-by-conventions.ashx
    /// </summary>
    [TestFixture]
    public class RegistrationByConvention
    {
        [Test]
        public void RegisterByFirstInterface()
        {
            var c = new WindsorContainer();

            c.Register(Classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Pick().If(t => t.Namespace.EndsWith("SampleCode"))
                .WithService.FirstInterface()
               );

            Assert.That(c.Resolve<IService>(), Is.InstanceOf<ConcreteServiceA>());
        }


    }
}
