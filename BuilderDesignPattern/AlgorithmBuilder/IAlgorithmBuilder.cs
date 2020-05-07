using StrategyDesignPattern;

namespace BuilderDesignPattern.AlgorithmBuilder
{
    public interface IAlgorithmBuilder
    {
        void BuildStrategyPatternParameters();
        void BuildStrategyPattern();

        CalculateTechnicalIndicatorContext StrategyContext { get; }
    }
}
