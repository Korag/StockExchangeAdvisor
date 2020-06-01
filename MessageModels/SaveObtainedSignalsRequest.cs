using Models;
using System.Collections.Generic;

namespace MessageModels
{
    public class SaveObtainedSignalsRequest
    {
        public List<Signal> Signals { get; set; }

        public SaveObtainedSignalsRequest(List<Signal> signals)
        {
            Signals = signals;
        }
    }
}
