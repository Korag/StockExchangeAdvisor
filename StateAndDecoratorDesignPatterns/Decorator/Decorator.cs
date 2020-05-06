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

        public override double CalculateConst()
        {
             return _baseComponent.CalculateConst();
        }

        public override ASignalState GetState()
        {
            return _baseComponent.GetState();
        }
    }
}