namespace StateDesignPattern
{
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

        private string _factor;

        public virtual string Factor
        {
            get { return _factor; }
        }

        public abstract void SetSignalValue(int signalValue);
        public abstract void SaveSignalToFile(string fileURL);
    }
}
