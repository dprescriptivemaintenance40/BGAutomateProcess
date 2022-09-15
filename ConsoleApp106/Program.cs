using ConsoleApp106.DAL;
using DPMInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using TaskDataModels;
using Tasks.Models;
using static CentrifugalTasks.CentrifugalParameterClass;
using static ReciprocatingTasks.ReciprocatingParameterClass;
using static ScrewTasks.ScrewParameterClass;

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
            Asset_FailureMode batch = _Context.Asset_FailureMode.Where(b => b.Description == fileName && b.IsProcessCompleted == 1).FirstOrDefault();
            if (batch != null)
            {
                Asset_Equipment equipment = _Context.Asset_Equipments.Where(b => b.Id == batch.EquipmentId).FirstOrDefault();
                batch.DateTimeBatchCompleted = "Batch is uploading";
                _Context.Entry(batch).State = EntityState.Modified;
                _Context.SaveChangesAsync();
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
                else if (equipment.AssetName == "ReciprocatingCompressor" || equipment.AssetName == "ReciprocatingPump" || equipment.AssetName == "RotaryPump")
                {
                    ITask<Assets> r = ReciprocatingTaskCreator.ReciprocatingCreate();
                    r.Processess(e.FullPath);
                }
            }
        }
    }
}
