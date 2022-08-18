using CommonTask;
using ConsoleApp106.DAL;
using ConsoleApp106.Model;
using CsvHelper;
using DPMInterfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TaskDataModels;

namespace Tasks
{
    public class Compressor
    {
        public static class TaskCreator
        {
            public static ITask<CompressorEquipment> Create()
            {
                UploadTask t1 = new UploadTask();
                ValidateTask t2 = new ValidateTask();
                PrcessingMissingValuesTask t3 = new PrcessingMissingValuesTask();
                PredictionTask t4 = new PredictionTask();
                t1.SetNextTask(t2);
                t2.SetNextTask(t3);
                t3.SetNextTask(t4);
                return t1;
            }
        }
        public class UploadTask : BaseTask<CompressorEquipment>
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
                BatchTable batch=_Context.BatchTables.Where(b=>b.Description==fileName && b.IsCompleted==1).FirstOrDefault();
                DataTable csvData = new DataTable();
                List<StagingTableCompressor> StagingTableCompressorrecords = new List<StagingTableCompressor>();
                
                try
                {
                    if (batch != null)
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
                                StagingTableCompressorrecords.Add(new StagingTableCompressor()
                                {
                                    Date = DateTime.Parse(fieldData[0]),
                                    BatchId = batch.Id,
                                    TD1 = fieldData[1],
                                    TS1 = fieldData[2],
                                    TD2 = fieldData[3],
                                    TS2 = fieldData[4],
                                    PD1 = fieldData[5],
                                    PD2 = fieldData[6],
                                    DT1 = fieldData[7],
                                    DT2 = fieldData[8],
                                    PR1 = fieldData[9],
                                    PR2 = fieldData[10]
                                });
                            }
                            _Context.BulkInsert(StagingTableCompressorrecords);
                        }
                        if (this.Next != null)
                        {
                            this.Next.Processess(batch.Description);
                        }
                    }
                    else
                    {
                        Console.WriteLine("File batch already completed ");
                        return;
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public class ValidateTask : BaseTask<CompressorEquipment>
        {
            public override void Processess(object batchDesc)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    BatchTable batch = _Context.BatchTables.Where(r => r.Description == batchDesc).FirstOrDefault();

                    List<StagingTableCompressor> equipment = _Context.StagingTableSingles.Where(r => r.BatchId == batch.Id)
                                                                    .ToList<StagingTableCompressor>();
                    List<CleanTableCompressor> cleanData = new List<CleanTableCompressor>();
                    List<ErrorTableCompressor> errorData = new List<ErrorTableCompressor>();
                    foreach (var item in equipment)
                    {
                        //Get list of workflow rules declared in the json
                        string json = File.ReadAllText(@"G:\DPMBGProcess\ConsoleApp106\Tasks\Rules.json");
                        var rules = JsonConvert.DeserializeObject<WorkflowRules[]>(json);
                        var engine = new RulesEngine.RulesEngine(rules);

                        if (float.TryParse(item.TD1, out _) && float.TryParse(item.TS1, out _) && float.TryParse(item.TD2, out _)
                            && float.TryParse(item.TS2, out _) && float.TryParse(item.PD1, out _) && float.TryParse(item.PD2, out _))
                        {
                            var TD1 = new RuleParameter("fieldData", float.Parse(item.TD1));
                            var TS1 = new RuleParameter("fieldData", float.Parse(item.TS1));
                            var TD2 = new RuleParameter("fieldData", float.Parse(item.TD2));
                            var TS2 = new RuleParameter("fieldData", float.Parse(item.TS2));
                            var PD1 = new RuleParameter("fieldData", float.Parse(item.PD1));
                            var PD2 = new RuleParameter("fieldData", float.Parse(item.PD2));
                            var TD1result = engine.ExecuteAllRulesAsync("ValidationTD1", TD1).Result;
                            var TS1result = engine.ExecuteAllRulesAsync("ValidationTS1", TS1).Result;
                            var TD2result = engine.ExecuteAllRulesAsync("ValidationTD2", TD2).Result;
                            var TS2result = engine.ExecuteAllRulesAsync("ValidationTS2", TS2).Result;
                            var PD1result = engine.ExecuteAllRulesAsync("ValidationPD1", PD1).Result;
                            var PD2result = engine.ExecuteAllRulesAsync("ValidationPD2", PD2).Result;

                            List<int> td1 = new List<int>();
                            List<int> ts1 = new List<int>();
                            List<int> td2 = new List<int>();
                            List<int> ts2 = new List<int>();
                            List<int> pd1 = new List<int>();
                            List<int> pd2 = new List<int>();
                            foreach (var res in TD1result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    td1.Add(n);
                                }
                            }
                            foreach (var res in TS1result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    ts1.Add(n);
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
                            foreach (var res in TS2result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    ts2.Add(n);
                                }
                            }
                            foreach (var res in PD1result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    pd1.Add(n);
                                }
                            }
                            foreach (var res in PD2result)
                            {
                                var output = res.ActionResult.Output;
                                if ((res.Rule.RuleName == "Numeric" && output.ToString() == "1") || (res.Rule.RuleName == "Outlier" && output.ToString() == "1"))
                                {
                                    var n = 1;
                                    pd2.Add(n);
                                }
                            }

                            if (td1.Count == TD1result.Count && ts1.Count == TS1result.Count && td2.Count == TD2result.Count && ts2.Count == TS2result.Count &&
                                  pd1.Count == PD1result.Count && pd2.Count == PD2result.Count)
                            {

                                cleanData.Add(new CleanTableCompressor()
                                {
                                    BatchId = item.BatchId,
                                    Date = item.Date,
                                    TD1 = float.Parse(item.TD1),
                                    TS1 = float.Parse(item.TS1),
                                    TD2 = float.Parse(item.TD2),
                                    TS2 = float.Parse(item.TS2),
                                    PD1 = float.Parse(item.PD1),
                                    PD2 = float.Parse(item.PD2),
                                    DT1 = float.Parse(item.DT1),
                                    DT2 = float.Parse(item.DT2),
                                    PR1 = float.Parse(item.PR1),
                                    PR2 = float.Parse(item.PR2)
                                });
                            }
                            else
                            {
                                errorData.Add(new ErrorTableCompressor()
                                {
                                    BatchId = item.BatchId,
                                    rowAffected = item.Id,
                                    Description = "Data may be zero" + item.TD1 + item.TS1 +
                                                  item.TD2 + item.TS2 + item.PD1 + item.PD2
                                });
                            }
                        }
                        else
                        {
                            errorData.Add(new ErrorTableCompressor()
                            {
                                BatchId = item.BatchId,
                                rowAffected = item.Id,
                                Description = "Data contains letters" + item.TD1 + item.TS1 +
                                                  item.TD2 + item.TS2 + item.PD1 + item.PD2
                            });
                        }
                    }

                    _Context.BulkInsert(cleanData);
                    _Context.BulkInsert(errorData);
                    if (this.Next != null)
                    {
                        this.Next.Processess(batchDesc);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public class PrcessingMissingValuesTask : BaseTask<CompressorEquipment>
        {
            public override void Processess(object path)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    BatchTable batch = _Context.BatchTables.Where(r => r.Description == path).FirstOrDefault();

                    List<CleanTableCompressor> cleanData = _Context.CleanTableSingles.Where(r => r.BatchId == batch.Id).ToList<CleanTableCompressor>();
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python310\python.EXE"; //cmd is full path to python.exe
                    //var script = @"G:\PredictiveMaintenance\ConsoleApp106\Tasks\MissingValuesDB.py {0}";
                    //var batchId = batch.Id;
                    start.Arguments = string.Format(@"G:\PredictiveMaintenance\ConsoleApp106\Tasks\MissingValuesDB.py {0}",batch.Id); //args is path to .py file and any cmd line args
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = true;
                    start.RedirectStandardError = true;
                    using (Process process = Process.Start(start))
                    {
                        using (StreamReader reader = process.StandardOutput)
                        {
                            string result = reader.ReadToEnd();
                            Console.Write(result);
                            //return new string[] { result };
                        }
                    }
                    if (this.Next != null)
                    {
                        this.Next.Processess(path);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public class PredictionTask : BaseTask<CompressorEquipment>
        {
            public override void Processess(object path)
            {
                try
                {
                    var _Context = new PlantDBContext();
                    BatchTable batch = _Context.BatchTables.Where(r => r.Description == path).FirstOrDefault();
                    batch.IsCompleted = 0;
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
