using StateDesignPattern;

namespace DecoratorDesignPattern
{
    public abstract class DecoratorComponent
    {
        public abstract double CalculateCost();
        public abstract double CalculateAdditionalFee();

        public abstract void SetFinalPrice(double value);
        public abstract void SetAdditionalFee(double value);

        public abstract double GetFinalPrice();
        public abstract double GetAdditionalFee();

        public abstract ASignalState GetState();
    }
}
