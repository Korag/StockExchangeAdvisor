using System;
using System.Collections.Generic;
using System.Text;

namespace DecoratorDesignPattern
{
    public class CommissionDecorator : Decorator
    {
        public CommissionDecorator(SignalModelContext signalModelContext)
       : base(signalModelContext)
        {
            this._price = 0;
        }
    }
}
