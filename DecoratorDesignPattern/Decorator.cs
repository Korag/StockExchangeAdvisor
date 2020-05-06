
using System;
using System.Collections.Generic;
using System.Text;

namespace DecoratorDesignPattern
{
    //Decorator class
    public abstract class Decorator : AFinalPriceComponent
    {
        SignalModelContext _signalContext = null;
        protected double _price = 0.0;

        protected Decorator(SignalModelContext signalContext)
        {
           _signalContext = signalContext;
        }

        #region SignalModelContext Members

        double GetFinalPrice()
        {
            return _price + _signalContext.GetFinalPrice();
        }

        #endregion
    }
}
