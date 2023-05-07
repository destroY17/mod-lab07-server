using System;
using System.Threading;

namespace SMO
{
    class Server
    {
        public int RequestCount { get; private set; } = 0;
        public int ProcessedCount { get; private set; } = 0;
        public int RejectedCount { get; private set; } = 0;

        private int _channelCount;
        private ServiceChannel[] _channels;
        private double _serviceIntensity;

        private object _locker = new object();

        public Server(int channelCount, double serviceIntensity)
        {
            _channelCount = channelCount;
            _serviceIntensity = serviceIntensity;

            _channels = new ServiceChannel[_channelCount];
            for (int i = 0; i < _channelCount; i++)
                _channels[i] = new ServiceChannel();
        }

        public void Proc(object sender, ProcEventArgs e)
        {
            lock (_locker)
            {
                Console.WriteLine($"Поступила заявка с номером {e.ID}");
                RequestCount++;

                for (int i = 0; i < _channelCount; i++)
                {
                    if (!_channels[i].InUse)
                    {
                        Console.WriteLine($"Заявка с номером {e.ID} принята");

                        _channels[i].InUse = true;
                        _channels[i].Thread = new Thread(() => RequestProcessing(_channels[i]));
                        _channels[i].Thread.Start();
                        ProcessedCount++;

                        return;
                    }
                }
                Console.WriteLine($"Заявка с номером {e.ID} отклонена");
                RejectedCount++;
            }
        }

        private void RequestProcessing(ServiceChannel processingChannel)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1 / _serviceIntensity));
            processingChannel.InUse = false;
        }
    }
}