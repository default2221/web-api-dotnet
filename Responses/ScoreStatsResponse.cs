namespace WebAPi.Responses
{
    public class ScoreStatsResponse
    {
        public Decimal AverageDaily { get; set; }

        public Decimal AverageMonthly { get; set; }

        public Decimal MaximumDaily { get; set; }

        public Decimal MaximumWeekly { get; set; }

        public Decimal MaximumMonthly { get; set; }
    }
}
