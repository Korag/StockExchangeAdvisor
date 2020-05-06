namespace DecoratorDesignPattern
{
    public class CommissionDecorator : Decorator
    {
        public CommissionDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
        }

        public override double CalculateConst()
        {
            return _baseComponent.CalculateConst() + 1000;
        }
    }
}
