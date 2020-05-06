namespace DecoratorDesignPattern
{
    class ConversionFromPLNtoUSDDecorator : Decorator
    {
        public ConversionFromPLNtoUSDDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
            
        }
    }
}
