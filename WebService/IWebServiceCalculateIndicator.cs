using WebServicesModels;

namespace WebService
{
    public interface IWebServiceCalculateIndicator
    {
        int GetAmountOfGeneratedSignalsFiles();
        int CalculateTechnicalIndicator(IndicatorCalculationElementsWIndicatorType data);
        string GetObtainedSignals(int id);
    }
}
