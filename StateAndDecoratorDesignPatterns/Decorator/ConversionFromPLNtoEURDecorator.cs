namespace DecoratorDesignPattern
{
    public class ConversionFromPLNtoEURDecorator : Decorator
    {
        public ConversionFromPLNtoEURDecorator(DecoratorComponent baseComponent)
     : base(baseComponent)
        {
            
        }
    }
}
