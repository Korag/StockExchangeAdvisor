namespace DecoratorDesignPattern
{
    public class TaxDecorator : Decorator
    {
        public TaxDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
           
        }

        public override double CalculateConst()
        {
            double taxFee = _baseComponent.CalculateConst() * 0.18;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:
                    return _baseComponent.CalculateConst() - taxFee;

                case -1:
                    return _baseComponent.CalculateConst() + taxFee;

                default:
                case 0:
                    return _baseComponent.CalculateConst();
            }

            //return _baseComponent.CalculateConst() + 1000;
        }
    }
}
