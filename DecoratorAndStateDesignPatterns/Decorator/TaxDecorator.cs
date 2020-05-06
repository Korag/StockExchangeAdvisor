using DecoratorAndStateDesignPatterns;

namespace DecoratorDesignPattern
{
    public class TaxDecorator : Decorator
    {
        public TaxDecorator(SignalModelContext baseComponent)
       : base(baseComponent)
        {
            this._price = 0;
        }
    }
}
