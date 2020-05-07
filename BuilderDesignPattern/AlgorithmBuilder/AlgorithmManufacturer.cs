namespace BuilderDesignPattern.AlgorithmBuilder
{
    public class AlgorithmManufacturer
    {
        public void Construct(IAlgorithmBuilder algorithmBuilder)
        {
            algorithmBuilder.BuildStrategyPatternParameters();
            algorithmBuilder.BuildStrategyPattern();
        }
    }
}
