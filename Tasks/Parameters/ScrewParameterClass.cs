﻿using CommonTask;
using ConsoleApp106.DAL;
using ConsoleApp106.Model;
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
using System.Threading;
using TaskDataModels;
using Tasks.Models;

namespace ScrewTasks
{
    public class ScrewParameterClass
    {
        public static class ScrewTaskCreator
        {
            public static ITask<Assets> ScrewCreate()
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
                    ScrewParameter sp = new ScrewParameter();
                    sp.FailureModeId = batch.Id;
                    _Context.ScrewParameters.Add(sp);
                    _Context.SaveChanges();

                    DataTable csvData = new DataTable();
                    List<ScrewStagingTable> StagingTableRecords = new List<ScrewStagingTable>();

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
                                StagingTableRecords.Add(new ScrewStagingTable()
                                {
                                    Date = DateTime.Parse(fieldData[0]),
                                    SPId = sp.Id,
                                    TD1 = fieldData[1],
                                    TD2 = fieldData[2],
                                    DT1 = fieldData[3],
                                    DT2 = fieldData[4],
                                    PR1 = fieldData[5],
                                    PR2 = fieldData[6]
                                });
                            }
                            _Context.BulkInsert(StagingTableRecords);
                        }
                        if (this.Next != null)
                        {
                            batch.DateTimeBatchCompleted = "Batch is validating";
                            _Context.Entry(batch).State = EntityState.Modified;
                            _Context.SaveChangesAsync();
                            this.Next.Processess(sp.Id);
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
            public override void Processess(object SPId)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    //Asset_FailureMode batch = _Context.Asset_FailureMode.Where(r => r.Id == Convert.ToInt32(SPId)).FirstOrDefault();
                    Asset_FailureMode batch = new Asset_FailureMode();
                    List<ScrewStagingTable> stageData = _Context.ScrewStagingTables.Where(r => r.SPId == Convert.ToInt32(SPId))
                                                                    .ToList<ScrewStagingTable>();
                    List<ScrewCleaningTable> cleanData = new List<ScrewCleaningTable>();
                    List<ScrewErrorTable> errorData = new List<ScrewErrorTable>();
                    foreach (var item in stageData)
                    {
                        //Get list of workflow rules declared in the json
                        string json = File.ReadAllText(@"G:\DPMBGProcess\BGAutomateProcess\Tasks\Rules.json");
                        var rules = JsonConvert.DeserializeObject<WorkflowRules[]>(json);
                        var engine = new RulesEngine.RulesEngine(rules);

                        if (float.TryParse(item.TD1, out _) && float.TryParse(item.TD2, out _) && float.TryParse(item.DT1, out _) 
                            && float.TryParse(item.DT2, out _) && float.TryParse(item.PR1, out _) && float.TryParse(item.PR2, out _))
                        {
                            var TD1 = new RuleParameter("fieldData", float.Parse(item.TD1));
                            var TD2 = new RuleParameter("fieldData", float.Parse(item.TD2));
                            var DT1 = new RuleParameter("fieldData", float.Parse(item.DT1));
                            var DT2 = new RuleParameter("fieldData", float.Parse(item.DT2));
                            var PR1 = new RuleParameter("fieldData", float.Parse(item.PR1));
                            var PR2 = new RuleParameter("fieldData", float.Parse(item.PR2));
                            var TD1result = engine.ExecuteAllRulesAsync("ValidationTD1", TD1).Result;
                            var TD2result = engine.ExecuteAllRulesAsync("ValidationTD2", TD2).Result;
                            var DT1result = engine.ExecuteAllRulesAsync("ValidationDT1", DT1).Result;
                            var DT2result = engine.ExecuteAllRulesAsync("ValidationDT2", DT2).Result;
                            var PR1result = engine.ExecuteAllRulesAsync("ValidationPR1", PR1).Result;
                            var PR2result = engine.ExecuteAllRulesAsync("ValidationPR2", PR2).Result;

                            List<int> td1 = new List<int>();
                            List<int> td2 = new List<int>();
                            List<int> dt1 = new List<int>();
                            List<int> dt2 = new List<int>();
                            List<int> pr1 = new List<int>();
                            List<int> pr2 = new List<int>();
                            foreach (var res in TD1result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;  
                                    td1.Add(n);
                                }
                            }
                            foreach (var res in TD2result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    td2.Add(n);
                                }
                            }
                            foreach (var res in DT1result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    dt1.Add(n);
                                }
                            }
                            foreach (var res in DT2result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    dt2.Add(n);
                                }
                            }
                            foreach (var res in PR1result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    pr1.Add(n);
                                }
                            }
                            foreach (var res in PR2result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    pr2.Add(n);
                                }
                            }

                            if (td1.Count == TD1result.Count && td2.Count == TD2result.Count && dt1.Count == DT1result.Count && dt2.Count == DT2result.Count  &&
                                  pr1.Count == PR1result.Count && pr2.Count == PR2result.Count)
                            {

                                cleanData.Add(new ScrewCleaningTable()
                                {
                                    SPId = item.SPId,
                                    Date = item.Date,
                                    TD1 = float.Parse(item.TD1),
                                    TD2 = float.Parse(item.TD2),
                                    DT1 = float.Parse(item.DT1),
                                    DT2 = float.Parse(item.DT2),
                                    PR1 = float.Parse(item.PR1),
                                    PR2 = float.Parse(item.PR2)
                                });
                            }
                            else
                            {
                                errorData.Add(new ScrewErrorTable()
                                {
                                    SPId = item.SPId,
                                    rowAffected = item.Id,
                                    Description = "Data may be zero" + " - " + item.TD1 + " " + item.TD2 + " " +
                                                  item.DT1 + " " + item.DT2 + " " + item.PR1 + " " + item.PR2
                                });
                            }
                        }
                        else
                        {
                            errorData.Add(new ScrewErrorTable()
                            {
                                SPId = item.SPId,
                                rowAffected = item.Id,
                                Description = "Data contains letters" + " - " + item.TD1 + " " + item.TD2 + " " +
                                                  item.DT1 + " " + item.DT2 + " " + item.PR1 + " " + item.PR2
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
                        this.Next.Processess(SPId);
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
            public override void Processess(object SPId)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    ScrewParameter sp = _Context.ScrewParameters.Where(r => r.Id == Convert.ToInt32(SPId)).FirstOrDefault();
                    Asset_FailureMode batch = _Context.Asset_FailureMode.Where(r => r.Id == sp.FailureModeId).FirstOrDefault();
                    //List<ScrewCleaningTable> cleanData = _Context.ScrewCleaningTables.Where(r => r.SPId == batch.Id).ToList<ScrewCleaningTable>();
                    Asset_Equipment equipment = _Context.Asset_Equipments.Where(b => b.Id == batch.EquipmentId).FirstOrDefault();

                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python310\python.EXE"; //cmd is full path to python.exe
                    //var script = @"G:\PredictiveMaintenance\ConsoleApp106\Tasks\MissingValuesDB.py {0}";
                    //var batchId = batch.Id;
                    start.Arguments = string.Format(@"G:\DPMBGProcess\BGAutomateProcess\Tasks\MissingValuesDB.py {0} {1}", SPId, equipment.AssetName); //args is path to .py file and any cmd line args
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
                        this.Next.Processess(SPId);
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
            public override void Processess(object SPId)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    ScrewParameter sp = _Context.ScrewParameters.Where(r => r.Id == Convert.ToInt32(SPId)).FirstOrDefault();
                    Asset_FailureMode batch = _Context.Asset_FailureMode.Where(r => r.Id == sp.FailureModeId).FirstOrDefault();
                    //List<ScrewCleaningTable> cleanData = _Context.ScrewCleaningTables.Where(r => r.SPId == batch.Id).ToList<ScrewCleaningTable>();
                    Asset_Equipment equipment = _Context.Asset_Equipments.Where(b => b.Id == batch.EquipmentId).FirstOrDefault();

                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python310\python.EXE"; //cmd is full path to python.exe
                    //var script = @"G:\PredictiveMaintenance\ConsoleApp106\Tasks\MissingValuesDB.py {0}";
                    //var batchId = batch.Id;
                    start.Arguments = string.Format(@"G:\DPMBGProcess\BGAutomateProcess\Tasks\SeasonalDB.py {0} {1}", SPId, equipment.AssetName); //args is path to .py file and any cmd line args
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
