namespace DecoratorDesignPattern
{
    class ConversionFromPLNtoUSDDecorator : Decorator
    {
        public ConversionFromPLNtoUSDDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
            
        }

        public override double CalculateCost()
        {
            double conversionFee = _baseComponent.CalculateCost() * 0.07;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:
                    return _baseComponent.CalculateCost() - conversionFee;

                case -1:
                    return _baseComponent.CalculateCost() + conversionFee;

                default:
                case 0:
                    return _baseComponent.CalculateCost();
            }

            //return _baseComponent.CalculateConst() + 1000;
        }
    }
}
