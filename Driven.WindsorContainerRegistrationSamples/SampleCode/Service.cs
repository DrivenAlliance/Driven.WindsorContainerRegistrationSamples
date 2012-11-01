namespace Driven.WindsorContainerRegistrationSamples.SampleCode
{
    public interface IService
    {
        IDependency Dependency { get; }
    }

    public interface IConcreteServiceA
    {
    }
    
    public class ConcreteServiceA : IService, IConcreteServiceA
    {
        public IDependency Dependency { get; private set; }

        public ConcreteServiceA(IDependency parm1)
        {
            Dependency = parm1;
        }
    }
    
    public class ConcreteServiceB : IService
    {
        public IDependency Dependency { get; private set; }

        public ConcreteServiceB(IDependency parm1)
        {
            Dependency = parm1;
        }
    }

    public class ConcreteServiceC : IService
    {
        public IDependency Dependency { get; set; }
    }

    public class ConcreteServiceD : IService
    {
        private IDependency _dependency = NullDependency.Instance;

        public IDependency Dependency
        {
            get { return _dependency; }
            set { _dependency = value; }
        }
    }

   
}