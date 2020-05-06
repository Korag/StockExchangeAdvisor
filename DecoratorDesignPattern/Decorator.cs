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
            //switch (_baseComponent.currentState.SignalValue)
            //{
            //    case 1:
            //        return _baseComponent.GetFinalPrice() - _price;

            //    case -1:
            //        return _baseComponent.GetFinalPrice() + _price;

            //    default:
            //    case 0:
            //        return _baseComponent.GetFinalPrice();
            //}

            //return _baseComponent.GetFinalPrice() + 100;

             return _baseComponent.CalculateConst();
        }
    }
}