using DPMInterfaces;
using System;

namespace CommonTask
{
    public abstract class BaseTask<T> : ITask<T> 
    {
        private ITask<T> _nextTax;

        public ITask<T> Next
        {
            get { return _nextTax; }  
        }

        public abstract void Processess(object filepath);

        public void SetNextTask(ITask<T> _t)
        {
            _nextTax = _t;
        }
    }
}
