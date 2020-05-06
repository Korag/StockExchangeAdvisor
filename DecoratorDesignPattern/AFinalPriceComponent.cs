using System;
using System.Collections.Generic;
using System.Text;

namespace DecoratorDesignPattern
{
    //Decorator component
    public abstract class AFinalPriceComponent
    {
        public abstract double GetFinalPrice();
    }
}
