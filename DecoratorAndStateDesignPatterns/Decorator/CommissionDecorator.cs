using DecoratorAndStateDesignPatterns;

namespace DecoratorDesignPattern
{
    public class CommissionDecorator : Decorator
    {
        public CommissionDecorator(SignalModelContext baseComponent)
       : base(baseComponent)
        {
            this._price = 100;
        }
    }
}
