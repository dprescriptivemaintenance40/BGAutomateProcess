using CommonTask;
using ConsoleApp106.DAL;
using DPMInterfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TaskDataModels;
using Tasks.Models;

namespace ReciprocatingTasks
{
    public class ReciprocatingParameterClass
    {
        public static class ReciprocatingTaskCreator
        {
            public static ITask<Assets> ReciprocatingCreate()
            {
                UploadTask t1 = new UploadTask();
                ValidateTask t2 = new ValidateTask();
                PrcessingMissingValuesTask t3 = new PrcessingMissingValuesTask();
                PredictionTask t4 = new PredictionTask();
                t1.SetNextTask(t2);
                t2.SetNextTask(t3);
                t3.SetNextTask(t4);
                return (ITask<Assets>)t1;
            }
        }
        public class UploadTask : BaseTask<Assets>
        {
            public override void Processess(object path)
            {
                string DataCSVPath = (string)path;
                var _Context = new PlantDBContext();

                ////Add batch
                //BatchTable batch = new BatchTable();
                //string batchname = "user";
                //batch.Description=batchname+ "_"+ Guid.NewGuid();
                //DateTime now = DateTime.Now;
                //batch.DateTimeUploaded = now.ToString();
                //batch.EquipmentProcessId = 2;
                //batch.EquipmentTblId = 1;
                //_Context.BatchTables.Add(batch);
                //_Context.SaveChanges();
                ////Change file name
                //string filePath = Path.GetDirectoryName(DataCSVPath);
                //string destinationFileName = filePath +"\\"+ batch.Description+".csv";
                //File.Move(DataCSVPath, destinationFileName);

                string fileName = Path.GetFileNameWithoutExtension(DataCSVPath);
                Asset_FailureMode batch = _Context.Asset_FailureMode.Where(b => b.Description == fileName && b.IsProcessCompleted == 1).FirstOrDefault();

                if (batch != null)
                {
                    ReciprocatingParameter rp = new ReciprocatingParameter();
                    rp.FailureModeId = batch.Id;
                    _Context.ReciprocatingParameters.Add(rp);
                    _Context.SaveChanges();

                    DataTable csvData = new DataTable();
                    List<ReciprocatingStagingTable> StagingTableRecords = new List<ReciprocatingStagingTable>();

                    try
                    {
                        using (TextFieldParser csvReader = new TextFieldParser(DataCSVPath))
                        {
                            csvReader.SetDelimiters(new string[] { "," });
                            csvReader.HasFieldsEnclosedInQuotes = true;
                            string[] colFields = csvReader.ReadFields();
                            //Column headers
                            foreach (string column in colFields)
                            {
                                DataColumn datecolumn = new DataColumn(column);
                                datecolumn.AllowDBNull = true;
                                csvData.Columns.Add(datecolumn);
                            }

                            while (!csvReader.EndOfData)
                            {

                                string[] fieldData = csvReader.ReadFields();
                                //Adding fields
                                StagingTableRecords.Add(new ReciprocatingStagingTable()
                                {
                                    Date = DateTime.Parse(fieldData[0]),
                                    RPId = rp.Id,
                                    TDValve = fieldData[1]
                                });
                            }
                            _Context.BulkInsert(StagingTableRecords);
                        }
                        if (this.Next != null)
                        {
                            batch.DateTimeBatchCompleted = "Batch is validating";
                            _Context.Entry(batch).State = EntityState.Modified;
                            _Context.SaveChangesAsync();
                            this.Next.Processess(rp.Id);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }
        }
        public class ValidateTask : BaseTask<Assets>
        {
            public override void Processess(object RPId)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    //Asset_FailureMode batch = _Context.Asset_FailureMode.Where(r => r.Description == batchDesc).FirstOrDefault();
                    Asset_FailureMode batch = new Asset_FailureMode();
                    List<ReciprocatingStagingTable> stageData = _Context.ReciprocatingStagingTables.Where(r => r.RPId == Convert.ToInt32(RPId))
                                                                    .ToList<ReciprocatingStagingTable>();
                    List<ReciprocatingCleaningTable> cleanData = new List<ReciprocatingCleaningTable>();
                    List<ReciprocatingErrorTable> errorData = new List<ReciprocatingErrorTable>();
                    foreach (var item in stageData)
                    {
                        //Get list of workflow rules declared in the json
                        string json = File.ReadAllText(@"G:\DPMBGProcess\BGAutomateProcess\Tasks\Rules.json");
                        var rules = JsonConvert.DeserializeObject<WorkflowRules[]>(json);
                        var engine = new RulesEngine.RulesEngine(rules);

                        if (float.TryParse(item.TDValve, out _))
                        {
                            var TDValve = new RuleParameter("fieldData", float.Parse(item.TDValve));
                            var TDValveresult = engine.ExecuteAllRulesAsync("ValidationTD1", TDValve).Result;

                            List<int> tdValve = new List<int>();
                            foreach (var res in TDValveresult)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    tdValve.Add(n);
                                }
                            }


                            if (tdValve.Count == TDValveresult.Count)
                            {

                                cleanData.Add(new ReciprocatingCleaningTable()
                                {
                                    RPId = item.RPId,
                                    Date = item.Date,
                                    TDValve = float.Parse(item.TDValve)
                                });
                            }
                            else
                            {
                                errorData.Add(new ReciprocatingErrorTable()
                                {
                                    RPId = item.RPId,
                                    rowAffected = item.Id,
                                    Description = "Data may be zero" + " - " + item.TDValve
                                });
                            }
                        }
                        else
                        {
                            errorData.Add(new ReciprocatingErrorTable()
                            {
                                RPId = item.RPId,
                                rowAffected = item.Id,
                                Description = "Data contains letters" + " - " + item.TDValve
                            });
                        }
                    }

                    _Context.BulkInsert(cleanData);
                    _Context.BulkInsert(errorData);
                    if (this.Next != null)
                    {
                        batch.DateTimeBatchCompleted = "Adding the missing values";
                        _Context.Entry(batch).State = EntityState.Modified;
                        _Context.SaveChangesAsync();
                        this.Next.Processess(RPId);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public class PrcessingMissingValuesTask : BaseTask<Assets>
        {
            public override void Processess(object RPId)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    ReciprocatingParameter rp = _Context.ReciprocatingParameters.Where(r => r.Id == Convert.ToInt32(RPId)).FirstOrDefault();
                    Asset_FailureMode batch = _Context.Asset_FailureMode.Where(r => r.Id == rp.FailureModeId).FirstOrDefault();

                    Asset_Equipment equipment = _Context.Asset_Equipments.Where(b => b.Id == batch.EquipmentId).FirstOrDefault();
                    //List<CentrifugalCleaningTable> cleanData = _Context.CentrifugalCleaningTables.Where(r => r.CPId == batch.Id).ToList<CentrifugalCleaningTable>();

                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python310\python.EXE"; //cmd is full path to python.exe
                    //var script = @"G:\PredictiveMaintenance\ConsoleApp106\Tasks\MissingValuesDB.py {0}";
                    //var batchId = batch.Id;
                    start.Arguments = string.Format(@"G:\DPMBGProcess\BGAutomateProcess\Tasks\MissingValuesDB.py {0} {1}", RPId, equipment.AssetName); //args is path to .py file and any cmd line args
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = true;
                    start.RedirectStandardError = true;
                    using (Process process = Process.Start(start))
                    {
                        using (StreamReader reader = process.StandardOutput, error = process.StandardError)
                        {
                            string result = reader.ReadToEnd();
                            string err = error.ReadToEnd();
                            Console.Write(result, err);
                            //return new string[] { result };
                        }
                    }
                    if (this.Next != null)
                    {
                        batch.DateTimeBatchCompleted = "Predicting the data";
                        _Context.Entry(batch).State = EntityState.Modified;
                        _Context.SaveChangesAsync();
                        this.Next.Processess(RPId);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public class PredictionTask : BaseTask<Assets>
        {
            public override void Processess(object RPId)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    ReciprocatingParameter rp = _Context.ReciprocatingParameters.Where(r => r.Id == Convert.ToInt32(RPId)).FirstOrDefault();
                    Asset_FailureMode batch = _Context.Asset_FailureMode.Where(r => r.Id == rp.FailureModeId).FirstOrDefault();

                    Asset_Equipment equipment = _Context.Asset_Equipments.Where(b => b.Id == batch.EquipmentId).FirstOrDefault();
                    //List<CentrifugalCleaningTable> cleanData = _Context.CentrifugalCleaningTables.Where(r => r.CPId == batch.Id).ToList<CentrifugalCleaningTable>();

                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python310\python.EXE"; //cmd is full path to python.exe
                    //var script = @"G:\PredictiveMaintenance\ConsoleApp106\Tasks\MissingValuesDB.py {0}";
                    //var batchId = batch.Id;
                    start.Arguments = string.Format(@"G:\DPMBGProcess\BGAutomateProcess\Tasks\SeasonalDB.py {0} {1}", RPId, equipment.AssetName); //args is path to .py file and any cmd line args
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = true;
                    start.RedirectStandardError = true;
                    using (Process process = Process.Start(start))
                    {
                        using (StreamReader reader = process.StandardOutput, error = process.StandardError)
                        {
                            string result = reader.ReadToEnd();
                            string err = error.ReadToEnd();
                            Console.Write(result, err);
                            //return new string[] { result };
                        }
                    }
                    batch.IsProcessCompleted = 0;
                    DateTime now = DateTime.Now;
                    batch.DateTimeBatchCompleted = now.ToString();
                    _Context.Entry(batch).State = EntityState.Modified;
                    _Context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
