using System;
using System.Collections.Generic;
using System.Text;

namespace DecoratorDesignPattern
{
    public class ConversionFromPLNtoEURDecorator : Decorator
    {
        public ConversionFromPLNtoEURDecorator(SignalModelContext signalModelContext)
     : base(signalModelContext)
        {
            this._price = 0;
        }
    }
}
