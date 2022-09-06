using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plant.Models.Plant
{
    public class ScrewParameter
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("failureModes")]
        public int FailureModeId { get; set; }
        public List<ScrewStagingTable> screwStagingTable { get; set; }
        public List<ScrewCleaningTable> screwCleaningTable { get; set; }
        public List<ScrewErrorTable> screwErrorTable { get; set; }
        public List<ScrewProcessedTable> screwProcessedTable { get; set; }
        public List<ScrewPredictedTable> screwPredictedTable { get; set; }
        public FailureMode failureModes;
    }
    public class ScrewStagingTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("screwParameter")]
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public string TD1 { get; set; }
        public string TD2 { get; set; }
        public string DT1 { get; set; }
        public string DT2 { get; set; }
        public string PR1 { get; set; }
        public string PR2 { get; set; }
        public ScrewParameter screwParameter;
    }
    public class ScrewCleaningTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("screwParameter")]
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public string TD1 { get; set; }
        public string TD2 { get; set; }
        public string DT1 { get; set; }
        public string DT2 { get; set; }
        public string PR1 { get; set; }
        public string PR2 { get; set; }
        public ScrewParameter screwParameter;
    }
    public class ScrewErrorTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("screwParameter")]
        public int SPId { get; set; }
        public int rowAffected { get; set; }
        public string Description { get; set; }
        public ScrewParameter screwParameter;
    }
    public class ScrewProcessedTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("screwParameter")]
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public string TD1 { get; set; }
        public string TD2 { get; set; }
        public string DT1 { get; set; }
        public string DT2 { get; set; }
        public string PR1 { get; set; }
        public string PR2 { get; set; }
        public ScrewParameter screwParameter;
    }
    public class ScrewPredictedTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("screwParameter")]
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public string TD1 { get; set; }
        public string TD2 { get; set; }
        public string DT1 { get; set; }
        public string DT2 { get; set; }
        public string PR1 { get; set; }
        public string PR2 { get; set; }
        public ScrewParameter screwParameter;
    }
}
