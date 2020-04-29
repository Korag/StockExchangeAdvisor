using BuilderDesignPattern.AlgorithmBuilder;

namespace FacadeDesignPattern
{
    public class WebServicesFacade
    {
        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }

        public WebServicesFacade()
        {
            _algorithmBuilder = new WebServicesBuilder();
            _algorithmManufacter.Construct(_algorithmBuilder);
        }
    }
}
