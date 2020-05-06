namespace DecoratorDesignPattern
{
    public class CommissionDecorator : Decorator
    {
        public CommissionDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
        }

        public override double CalculateCost()
        {
            double commissionFee = CalculateAdditionalFee();
            SetAdditionalFee(commissionFee);

            double finalPrice;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:

                    finalPrice =_baseComponent.CalculateCost() - commissionFee;
                    SetFinalPrice(finalPrice);
                    return GetFinalPrice();

                case -1:
                    finalPrice = _baseComponent.CalculateCost() + commissionFee;
                    SetFinalPrice(finalPrice);
                    return GetFinalPrice();

                default:
                case 0:
                    return GetFinalPrice();          
            }
        }

        public override double CalculateAdditionalFee()
        {
            return _baseComponent.CalculateCost() * 0.05;
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
