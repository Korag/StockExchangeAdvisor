using DecoratorAndStateDesignPatterns;

namespace DecoratorDesignPattern
{
    class ConversionFromPLNtoUSDDecorator : Decorator
    {
        public ConversionFromPLNtoUSDDecorator(SignalModelContext baseComponent)
       : base(baseComponent)
        {
            this._price = 0;
        }
    }
}
