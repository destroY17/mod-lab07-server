using System.Threading;

namespace SMO
{
    class ServiceChannel
    {
        public Thread Thread;
        public bool InUse = false;
    }
}
