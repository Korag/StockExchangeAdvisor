namespace DecoratorDesignPattern
{
    public class TaxDecorator : Decorator
    {
        public TaxDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
           
        }

        public override double CalculateAdditionalFee()
        {
            throw new System.NotImplementedException();
        }

        public override double CalculateCost()
        {
            double taxFee = _baseComponent.CalculateCost() * 0.18;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:
                    return _baseComponent.CalculateCost() - taxFee;

                case -1:
                    return _baseComponent.CalculateCost() + taxFee;

                default:
                case 0:
                    return _baseComponent.CalculateCost();
            }

            //return _baseComponent.CalculateConst() + 1000;
        }
    }
}
