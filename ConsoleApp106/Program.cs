using DPMInterfaces;
using System;
using System.IO;
using TaskDataModels;
using static Tasks.Compressor;

namespace ConsoleApp106
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            string filePath = @"G:\DPMBGProcess\ConsoleApp106\Tasks\DataFiles";
            watcher.Path = filePath;
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Filter = "*.csv";
            watcher.EnableRaisingEvents = true;
            
            
            // will track changes in sub-folders as well
            watcher.IncludeSubdirectories = true;
            watcher.Created += OnCreated;
            
            new System.Threading.AutoResetEvent(false).WaitOne();

        }
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            ITask<CompressorEquipment> t = TaskCreator.Create();
            t.Processess(e.FullPath);
            Console.WriteLine("Hello World!");
        }
    }
}
