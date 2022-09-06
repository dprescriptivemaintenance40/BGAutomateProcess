using ConsoleApp106.DAL;
using DPMInterfaces;
using Plant.Models.Plant;
using System;
using System.IO;
using System.Linq;
using TaskDataModels;
using static CentrifugalTasks.CentrifugalParameter;
using static ReciprocatingTasks.ReciprocatingParameter;
using static ScrewTasks.ScrewParameter;

namespace ConsoleApp106
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            string filePath = @"G:\DPMBGProcess\BGAutomateProcess\Tasks\DataFiles";
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
            var _Context = new PlantDBContext();
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);

            //Che cking the file is uploaded or not in database
            string fileName = Path.GetFileNameWithoutExtension(e.FullPath);
            FailureMode batch = _Context.FailureMode.Where(b => b.Description == fileName && b.IsProcessCompleted == 1).FirstOrDefault();
            if (batch != null)
            {
                Equipment equipment = _Context.Equipments.Where(b => b.Id == batch.TagNumberId ).FirstOrDefault();
                if(equipment.AssetName== "ScrewCompressor")
                {
                    ITask<Assets> s = ScrewTaskCreator.ScrewCreate();
                    s.Processess(e.FullPath);
                }
                else if (equipment.AssetName == "CentrifugalCompressor" || equipment.AssetName == "CentrifugalPump")
                {
                    ITask<Assets> c = CentrifugalTaskCreator.CentrifugalCreate();
                    c.Processess(e.FullPath);
                }
                else if (equipment.AssetName == "ReciprocatingCompressor" || equipment.AssetName == "ReciprocatingPump" || equipment.AssetName == "Rotary Pump")
                {
                    ITask<Assets> r = ReciprocatingTaskCreator.ReciprocatingCreate();
                    r.Processess(e.FullPath);
                }
            }
        }
    }
}
