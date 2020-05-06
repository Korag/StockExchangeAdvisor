using DecoratorAndStateDesignPatterns;

namespace DecoratorDesignPattern
{
    public class ConversionFromPLNtoEURDecorator : Decorator
    {
        public ConversionFromPLNtoEURDecorator(SignalModelContext baseComponent)
     : base(baseComponent)
        {
            this._price = 0;
        }
    }
}
