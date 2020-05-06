using StateDesignPattern;

namespace DecoratorDesignPattern
{
    public class DecoratorConcreteComponent : DecoratorComponent
    {
        public double Close;
        public ASignalState CurrentState = null;

        public double FinalPrice;
        public double AdditionalFee;

        public override double CalculateCost()
        {
            return FinalPrice;
        }

        public override double CalculateAdditionalFee()
        {
            return AdditionalFee;
        }

        public override ASignalState GetState()
        {
            return CurrentState;
        }

        public override void SetFinalPrice(double value)
        {
            this.FinalPrice = value;
        }

        public override void SetAdditionalFee(double value)
        {
            this.AdditionalFee += value;
        }

        public override double GetFinalPrice()
        {
            return FinalPrice;
        }

        public override double GetAdditionalFee()
        {
            return AdditionalFee;
        }
    }
}
