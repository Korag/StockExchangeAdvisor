using StrategyDesignPattern;

namespace BuilderDesignPattern.AlgorithmBuilder
{
    public interface IAlgorithmBuilder
    {
        void BuildStrategyPattern();
        void BuildAdditionalStrategyPatternParameters();

        CalculateTechnicalIndicatorContext StrategyContext { get; }
    }
}
