using BuilderDesignPattern.AlgorithmBuilder;

namespace FacadeDesignPattern
{
    public class RabbitMQFacade
    {
        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }

        public RabbitMQFacade()
        {
            _algorithmBuilder = new RabbitMQBuilder();
            _algorithmManufacter.Construct(_algorithmBuilder);
        }
    }
}
