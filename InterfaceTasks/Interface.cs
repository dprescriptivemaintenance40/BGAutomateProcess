using System;

namespace DPMInterfaces
{
    public interface ITask<T>  
    {
        public ITask<T> Next { get;  }
        void SetNextTask(ITask<T> _t);
        void Process();
    }
    public interface IEquipment
    {
         string Name { get; set; }
    }
}
