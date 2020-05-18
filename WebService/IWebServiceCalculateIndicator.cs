namespace WebService
{
    public interface IWebServiceCalculateIndicator
    {
        int GetAmountOfGeneratedSignalsFiles();
        int CalculateTechnicalIndicator(Utility.IndicatorCalculationElementsWIndicatorType data);
        string GetObtainedSignals(int id);
    }
}
