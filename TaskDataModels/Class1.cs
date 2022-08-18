using DPMInterfaces;
using System;
using System.IO;

namespace TaskDataModels
{
    public class CompressorEquipment : IEquipment
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
    
}
