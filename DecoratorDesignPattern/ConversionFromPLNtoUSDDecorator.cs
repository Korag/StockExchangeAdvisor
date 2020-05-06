using System;
using System.Collections.Generic;
using System.Text;

namespace DecoratorDesignPattern
{
    public class ConversionFromPLNtoUSDDecorator : Decorator
    {
        public ConversionFromPLNtoUSDDecorator(SignalModelContext signalModelContext)
       : base(signalModelContext)
        {
            this._price = 0;
        }
    }
}
