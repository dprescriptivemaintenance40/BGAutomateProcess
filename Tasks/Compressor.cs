using CommonTask;
using ConsoleApp106.DAL;
using ConsoleApp106.Model;
using CsvHelper;
using DPMInterfaces;
using EFCore.BulkExtensions;
using Microsoft.IdentityModel.Protocols;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
                ValidateThreshold v = new ValidateThreshold();
                t1.SetNextTask(t2);
                t2.SetNextTask(v);
                return t1;
            }
        }
        public class UploadTask : BaseTask<CompressorEquipment>
        {
            public override void Process()
            {
                DataTable csvData = new DataTable();
                var _Context = new EquipmentDbContext();
                List<StagingTableCompressor> StagingTableCompressorrecords = new List<StagingTableCompressor>();
                string DataCSVPath = @"G:\DPMBGProcess\ConsoleApp106\Tasks\CompressorData.csv";
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
                            //Making empty value as null
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }
                            //Adding fields
                            //StagingTableCompressorrecords.Add(new StagingTableCompressor()
                            //{
                            //    Date = DateTime.Parse(fieldData[0]),
                            //    TD1 = float.Parse(fieldData[1]),
                            //    TS1 = float.Parse(fieldData[2]),
                            //    TD2 = float.Parse(fieldData[3]),
                            //    TS2 = float.Parse(fieldData[4]),
                            //    PD1 = float.Parse(fieldData[5]),
                            //    PD2 = float.Parse(fieldData[6]),
                            //    DT1 = float.Parse(fieldData[7]),
                            //    DT2 = float.Parse(fieldData[8]),
                            //    PR1 = float.Parse(fieldData[9]),
                            //    PR2 = float.Parse(fieldData[10])
                            //});
                        }
                     //   _Context.BulkInsert(StagingTableCompressorrecords);
                    }
                    if (this.Next != null)
                    {
                        this.Next.Process();
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
            public override void Process()
            {
                var _Context = new EquipmentDbContext();
                List<StagingTableCompressor> equipment = _Context.StagingTableSingles.ToList<StagingTableCompressor>();
                List<CleanTableCompressor> cleanData = _Context.CleanTableSingles.ToList<CleanTableCompressor>();
                List<ErrorTableCompressor> errorData = _Context.ErrorTableSingles.ToList<ErrorTableCompressor>();
                foreach (var item in equipment)
                {
                    string json = File.ReadAllText(@"G:\DPMBGProcess\ConsoleApp106\Tasks\Rules.json");
                    var rules = JsonConvert.DeserializeObject<WorkflowRules[]>(json);
                    var engine = new RulesEngine.RulesEngine(rules);
                    var TD1 = new RuleParameter("fieldData", 160);
                    var TS1 = new RuleParameter("fieldData", 10);
                    var TD2 = new RuleParameter("fieldData", 200);
                    var TS2 = new RuleParameter("fieldData", 70);
                    var PD1 = new RuleParameter("fieldData", 1.8);
                    var PD2 = new RuleParameter("fieldData", 7);
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
                                                TD1 = item.TD1,
                                                TS1 = item.TS1,
                                                TD2 = item.TD2,
                                                TS2 = item.TS2,
                                                PD1 = item.PD1,
                                                PD2 = item.PD2,
                                                DT1 = item.DT1,
                                                DT2 = item.DT2,
                                                PR1 = item.PR1,
                                                PR2 = item.PR2
                                            });
                    }
                    else
                    {
                        errorData.Add(new ErrorTableCompressor()
                        {
                            BatchId = item.BatchId,
                            rowAffected = item.Id,
                            Description = "Error in data"
                    });
                    }
                }
                //Get list of workflow rules declared in the json
                
                if (this.Next != null)
                {
                    this.Next.Process();
                }
            }
        }
        public class ValidateThreshold : BaseTask<CompressorEquipment>
        {
            public override void Process()
            {
                if (this.Next != null)
                {
                    this.Next.Process();
                }
            }
        }
    }
}
