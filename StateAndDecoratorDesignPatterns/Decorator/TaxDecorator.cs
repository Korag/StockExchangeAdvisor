namespace DecoratorDesignPattern
{
    public class TaxDecorator : Decorator
    {
        public TaxDecorator(DecoratorComponent baseComponent) : base(baseComponent)
        {
           
        }

        public override double CalculateCost()
        {
            double taxFee = CalculateAdditionalFee();
            SetAdditionalFee(taxFee);

            double finalPrice;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:

                    finalPrice = _baseComponent.CalculateCost() - taxFee;
                    SetFinalPrice(finalPrice);
                    return GetFinalPrice();

                case -1:
                    finalPrice = _baseComponent.CalculateCost() + taxFee;
                    SetFinalPrice(finalPrice);
                    return GetFinalPrice();

                default:
                case 0:
                    finalPrice = _baseComponent.CalculateCost();
                    SetFinalPrice(finalPrice);
                    return GetFinalPrice();
            }
        }

        public override double CalculateAdditionalFee()
        {
            return _baseComponent.CalculateCost() * 0.18;
        }

        public override void SetFinalPrice(double value)
        {
            _baseComponent.SetFinalPrice(value);
        }

        public override void SetAdditionalFee(double value)
        {
            _baseComponent.SetAdditionalFee(value);
        }

        public override double GetFinalPrice()
        {
            return _baseComponent.GetFinalPrice();
        }

        public override double GetAdditionalFee()
        {
            return _baseComponent.GetAdditionalFee();
        }
    }
}
