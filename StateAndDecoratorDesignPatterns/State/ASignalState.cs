using StateAndDecoratorDesignPattern;

namespace StateDesignPattern
{
    //Abstract state class
    public abstract class ASignalState
    {
        private SignalModelContext _context;

        public SignalModelContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        private int _signalValue = 0;

        public int SignalValue
        {
            get { return _signalValue; }
            set { _signalValue = value; }
        }

        public abstract void SetSignalValue(int signalValue);
    }
}
