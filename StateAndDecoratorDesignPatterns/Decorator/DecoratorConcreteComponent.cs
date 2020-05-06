using StateDesignPattern;

namespace DecoratorDesignPattern
{
    public class DecoratorConcreteComponent : DecoratorComponent
    {
        public double Close;
        public ASignalState CurrentState = null;

        public override double CalculateConst()
        {
            return Close;
        }

        public override ASignalState GetState()
        {
            return CurrentState;
        }
    }
}
