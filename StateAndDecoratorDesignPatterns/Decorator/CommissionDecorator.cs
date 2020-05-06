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
            double commissionFee = _baseComponent.CalculateConst() * 0.05;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:
                    return _baseComponent.CalculateConst() - commissionFee;

                case -1:
                    return _baseComponent.CalculateConst() + commissionFee;

                default:
                case 0:
                    return _baseComponent.CalculateConst();
            }

            //return _baseComponent.CalculateConst() + 1000;
        }
    }
}
