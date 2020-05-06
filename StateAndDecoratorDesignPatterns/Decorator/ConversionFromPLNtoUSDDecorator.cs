namespace DecoratorDesignPattern
{
    class ConversionFromPLNtoUSDDecorator : Decorator
    {
        public ConversionFromPLNtoUSDDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
            
        }

        public override double CalculateConst()
        {
            double conversionFee = _baseComponent.CalculateConst() * 0.07;

            switch (_baseComponent.GetState().SignalValue)
            {
                case 1:
                    return _baseComponent.CalculateConst() - conversionFee;

                case -1:
                    return _baseComponent.CalculateConst() + conversionFee;

                default:
                case 0:
                    return _baseComponent.CalculateConst();
            }

            //return _baseComponent.CalculateConst() + 1000;
        }
    }
}
