namespace StateDesignPattern
{
    public abstract class SignalState
    {
        private SignalStateContext _context;

        public SignalStateContext Context
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
        public abstract void SaveSignalToFile(string fileURL);
    }
}
