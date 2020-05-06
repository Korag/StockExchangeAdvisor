namespace DecoratorDesignPattern
{
    public class TaxDecorator : Decorator
    {
        public TaxDecorator(DecoratorComponent baseComponent)
       : base(baseComponent)
        {
           
        }
    }
}
