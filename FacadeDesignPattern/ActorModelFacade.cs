using BuilderDesignPattern.AlgorithmBuilder;

namespace FacadeDesignPattern
{
    public class ActorModelFacade
    {
        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }

        public ActorModelFacade()
        {
            _algorithmBuilder = new ActorModelBuilder();
            _algorithmManufacter.Construct(_algorithmBuilder);
        }
    }
}
