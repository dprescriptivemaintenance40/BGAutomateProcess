using DPMInterfaces;
using System;
using TaskDataModels;
using static Tasks.Compressor;

namespace ConsoleApp106
{
    class Program
    {
        static void Main(string[] args)
        {
            ITask<CompressorEquipment> t = TaskCreator.Create();
           
            t.Process();
            Console.WriteLine("Hello World!");
        }
    }
}
