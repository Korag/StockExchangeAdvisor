using StateDesignPattern;

namespace DecoratorDesignPattern
{
    //Decorator class
    public abstract class Decorator : DecoratorComponent
    {
        protected DecoratorComponent _baseComponent = null;

        protected Decorator(DecoratorComponent baseComponent)
        {
            _baseComponent = baseComponent;
        }

        public override double CalculateCost()
        {
             return _baseComponent.CalculateCost();
        }

        public override ASignalState GetState()
        {
            return _baseComponent.GetState();
        }

        public override double CalculateAdditionalFee()
        {
            return _baseComponent.CalculateAdditionalFee();
        }

        public override void SetFinalPrice(double value)
        {
            _baseComponent.SetFinalPrice(value);
        }

        public override void SetAdditionalFee(double value)
        {
            _baseComponent.SetAdditionalFee(value);
        }

        public override double GetFinalPrice()
        {
            return _baseComponent.GetFinalPrice();
        }

        public override double GetAdditionalFee()
        {
            return _baseComponent.GetAdditionalFee();
        }
    }
}