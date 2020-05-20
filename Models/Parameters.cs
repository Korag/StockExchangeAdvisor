namespace Models
{
    public class Parameters
    {
        public int CalculatedIndicatorFirstDaysInterval { get; set; }
        public int CalculatedIndicatorSecondDaysInterval { get; set; }

        public double BuyTrigger { get; set; }
        public double SellTrigger { get; set; }
        public int NQuotesBackwards { get; set; }
        public int Period { get; set; }
    }
}
