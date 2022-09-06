using DPMInterfaces;
using System;
using System.IO;

namespace TaskDataModels
{
    public class Assets : IEquipment
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
    //public class CentrifugalEquipment : IEquipment
    //{
    //    public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //    public void OnChanged(object sender, FileSystemEventArgs e)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //public class ReciprocatingEquipment : IEquipment
    //{
    //    public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //    public void OnChanged(object sender, FileSystemEventArgs e)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
