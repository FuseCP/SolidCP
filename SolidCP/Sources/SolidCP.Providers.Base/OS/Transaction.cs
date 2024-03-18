using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
    public class Transaction
    {
        public Transaction() { }

        Action _task, _undoTask;
        void Run()
        {
            if (_task != null && _undoTask != null)
            {
                try
                {
                    _task();
                }
                catch
                {
                    _undoTask();
                    throw;
                }
            }
        }
        public void Do(Action task)
        {
            _task = task;
            Run();
        }
        public void Undo(Action task)
        {
            _undoTask = task;
            Run();
        }
    }
}
