using System;
using System.Threading;

namespace SMO
{
    class Client
    {
        public event EventHandler<ProcEventArgs> Request;
        public bool IsGenerator = true;

        public Client(Server server) => Request += server.Proc;

        protected virtual void OnProc(ProcEventArgs e) => Request?.Invoke(this, e);

        public void GenerateRequests(double arrivalIntensity)
        {
            IsGenerator = true;
            var id = 0;

            while (IsGenerator)
            {
                Console.WriteLine($"Отправлена заявка с номером {id}");
                var args = new ProcEventArgs(id);
                OnProc(args);
                Thread.Sleep(TimeSpan.FromSeconds(1 / arrivalIntensity));
                id++;
            }
        }
    }
}
