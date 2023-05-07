using System;
using System.Threading;

namespace SMO
{
    static class Analyzer
    {
        public static string GetAnalysisReport(double arrivalIntensity, double serviceIntensity,
            int channelCount, TimeSpan operatingTime)
        {
            var theoryIndicators = Indicators.GetIndicators(arrivalIntensity, serviceIntensity, channelCount);

            var server = new Server(channelCount, serviceIntensity);

            var client = new Client(server);
            new Thread(() => client.GenerateRequests(arrivalIntensity)).Start();
            Thread.Sleep(operatingTime);
            client.IsGenerator = false;

            var realArrivalIntensity = server.RequestCount / operatingTime.TotalSeconds;
            var realServiceIntensity = server.ProcessedCount / (operatingTime.TotalSeconds * channelCount);
            var realIndicators = Indicators.GetIndicators(realArrivalIntensity, realServiceIntensity, channelCount);

            var differenceIndicators = new Indicators(
                Math.Abs(theoryIndicators.DowntimeProbability - realIndicators.DowntimeProbability),
                Math.Abs(theoryIndicators.FailureProbability - realIndicators.FailureProbability),
                Math.Abs(theoryIndicators.RelativeThroughput - realIndicators.RelativeThroughput),
                Math.Abs(theoryIndicators.AbsoluteThroughput - realIndicators.AbsoluteThroughput),
                Math.Abs(theoryIndicators.AvgChannelsCount - realIndicators.AvgChannelsCount)
                );

            return
                $"Параметры системы:\n" +
                $"Интенсивность потока запросов: {arrivalIntensity}\n" +
                $"Интенсивность потока обслуживания: {serviceIntensity}\n" +
                $"Количество каналов: {channelCount}\n\n" +
                $"Статистика работы сервера:\n" +
                $"Время работы - {operatingTime.TotalSeconds} сек\n" +
                $"Кол-во запросов - {server.RequestCount}\n" +
                $"Кол-во принятых запросов - {server.ProcessedCount}\n" +
                $"Кол-во отклоненных запросов - {server.RejectedCount}\n\n" +
                $"Теоретические результаты:\n{theoryIndicators}\n" +
                $"Реальные результаты:\n{realIndicators}\n" +
                $"Разность результатов:\n{differenceIndicators}\n" +
                $"-------------------------------------------------------------------------\n";
        }
    }
}
