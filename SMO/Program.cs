using System;
using System.Text;
using System.IO;

namespace SMO
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "../../../../results.txt";
            using var writer = new StreamWriter(path, false, Encoding.UTF8);

            var time = TimeSpan.FromSeconds(10);
            writer.WriteLine(Analyzer.GetAnalysisReport(100, 10, 5, time));
            writer.Write(Analyzer.GetAnalysisReport(100, 30, 5, time));
            writer.Write(Analyzer.GetAnalysisReport(100, 10, 10, time));
            writer.Write(Analyzer.GetAnalysisReport(100, 30, 10, time));

        }
    }
}
