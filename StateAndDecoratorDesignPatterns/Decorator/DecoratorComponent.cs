using StateDesignPattern;

namespace DecoratorDesignPattern
{
    public abstract class DecoratorComponent
    {
        public abstract double CalculateConst();
        public abstract ASignalState GetState();
    }
}
