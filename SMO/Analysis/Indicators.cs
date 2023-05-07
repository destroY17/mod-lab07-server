using System;

namespace SMO
{
    class Indicators
    {
        public readonly double DowntimeProbability;
        public readonly double FailureProbability;
        public readonly double RelativeThroughput;
        public readonly double AbsoluteThroughput;
        public readonly double AvgChannelsCount;

        public Indicators(
            double downtimeProbability,
            double failureProbability,
            double relativeThroughput,
            double absoluteThroughput,
            double avgChannelCount)
        {
            DowntimeProbability = downtimeProbability;
            FailureProbability = failureProbability;
            RelativeThroughput = relativeThroughput;
            AbsoluteThroughput = absoluteThroughput;
            AvgChannelsCount = avgChannelCount;
        }

        public static Indicators GetIndicators(double arrivalIntensity, double serviceIntensity, int channelCount)
        {
            var reducedRate = arrivalIntensity / serviceIntensity;

            double downTimeProbability = 0;
            for (int i = 0; i < channelCount + 1; i++)
                downTimeProbability += Math.Pow(reducedRate, i) / Factorial(i);
            downTimeProbability = 1 / downTimeProbability;

            var failureProbability = Math.Pow(reducedRate, channelCount) *
                downTimeProbability / Factorial(channelCount);

            var relativeThroughput = 1 - failureProbability;
            var absoluteThroughput = arrivalIntensity * relativeThroughput;
            var avgChannelCount = absoluteThroughput / serviceIntensity;

            return new Indicators(
                downTimeProbability,
                failureProbability,
                relativeThroughput,
                absoluteThroughput,
                avgChannelCount
                );
        }

        private static int Factorial(int n) =>
            (n == 0) ? 1 : n * Factorial(n - 1);

        public override string ToString()
        {
            return
                $"Вероятность простоя системы: {DowntimeProbability}\n" +
                $"Вероятность отказа системы: {FailureProbability}\n" +
                $"Относительная пропускная способность: {RelativeThroughput}\n" +
                $"Абсолютная пропускная способность: {AbsoluteThroughput}\n" +
                $"Среднее число занятых каналов: {AvgChannelsCount}\n";
        }
    }
}
