using System;

namespace SMO
{
    class ProcEventArgs : EventArgs
    {
        public readonly int ID;
        public ProcEventArgs(int id) : base() => ID = id;
    }
}
