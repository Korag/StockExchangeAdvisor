namespace Models
{
    public class Parameters
    {
        public int CalculatedIndicatorFirstDaysInterval { get; set; }
        public int CalculatedIndicatorSecondDaysInterval { get; set; }

        public double BuyTrigger;
        public double SellTrigger;
        public int NQuotesBackwards;
        public int Period;
    }
}
