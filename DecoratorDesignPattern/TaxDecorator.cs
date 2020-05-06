using System;
using System.Collections.Generic;
using System.Text;

namespace DecoratorDesignPattern
{
    public class TaxDecorator : Decorator
    {
        public TaxDecorator(SignalModelContext signalModelContext)
       : base(signalModelContext)
        {
            this._price = 0;
        }
    }
}
