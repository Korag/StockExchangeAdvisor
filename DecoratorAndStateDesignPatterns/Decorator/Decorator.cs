
using DecoratorAndStateDesignPatterns;

namespace DecoratorDesignPattern
{
    //Decorator class
    public abstract class Decorator : AFinalPriceComponent
    {
        SignalModelContext _baseComponent = null;
        protected double _price = 0.0;

        protected Decorator(SignalModelContext baseComponent)
        {
            _baseComponent = baseComponent;
        }

        #region AFinalPriceComponent Members

        double AFinalPriceComponent.GetFinalPrice()
        {
            switch (_baseComponent.currentState.SignalValue)
            {
                case 1:
                    return _baseComponent.GetFinalPrice() - _price;

                case -1:
                    return _baseComponent.GetFinalPrice() + _price;

                default:
                case 0:
                    return _baseComponent.GetFinalPrice();
            }
        }

        #endregion
    }
}