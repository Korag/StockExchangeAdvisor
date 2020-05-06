namespace DecoratorDesignPattern
{
    public class ConversionFromPLNtoEURDecorator : Decorator
    {
        public ConversionFromPLNtoEURDecorator(DecoratorComponent baseComponent) : base(baseComponent)
        {
            
        }

        public override double CalculateCost()
        {
            double conversionFee = CalculateAdditionalFee();
            SetAdditionalFee(conversionFee);

            double finalPrice;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:

                    finalPrice = _baseComponent.CalculateCost() - conversionFee;
                    SetFinalPrice(finalPrice);
                    return GetFinalPrice();

                case -1:
                    finalPrice = _baseComponent.CalculateCost() + conversionFee;
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
            return _baseComponent.CalculateCost() * 0.03;
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
