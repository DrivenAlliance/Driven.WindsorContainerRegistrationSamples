namespace Driven.WindsorContainerRegistrationSamples.SampleCode
{
    public interface IDependency { }

    public class DependencyA : IDependency { }

    public class DependencyB : IDependency { }

    public class NullDependency : IDependency
    {
        private static IDependency _nullDependency = new NullDependency();

        public static IDependency Instance
        {
            get { return _nullDependency; }
        }
    }
}