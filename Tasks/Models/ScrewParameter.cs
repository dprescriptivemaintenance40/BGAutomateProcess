using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tasks.Models
{
    public class ScrewParameter
    {
        [Key]
        public int Id { get; set; }
        public int FailureModeId { get; set; }
        public List<ScrewStagingTable> screwStagingTable { get; set; }
        public List<ScrewCleaningTable> screwCleaningTable { get; set; }
        public List<ScrewErrorTable> screwErrorTable { get; set; }
        public List<ScrewProcessedTable> screwProcessedTable { get; set; }
        public List<ScrewPredictedTable> screwPredictedTable { get; set; }
        public Asset_FailureMode asset_failureModes { get; set; }
    }
    public class ScrewStagingTable
    {
        [Key]
        public int Id { get; set; }
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public string TD1 { get; set; }
        public string TD2 { get; set; }
        public string DT1 { get; set; }
        public string DT2 { get; set; }
        public string PR1 { get; set; }
        public string PR2 { get; set; }
        public ScrewParameter screwParameter { get; set; }
    }
    public class ScrewCleaningTable
    {
        [Key]
        public int Id { get; set; }
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public float TD1 { get; set; }
        public float TD2 { get; set; }
        public float DT1 { get; set; }
        public float DT2 { get; set; }
        public float PR1 { get; set; }
        public float PR2 { get; set; }
        public ScrewParameter screwParameter { get; set; }
    }
    public class ScrewErrorTable
    {
        [Key]
        public int Id { get; set; }
        public int SPId { get; set; }
        public int rowAffected { get; set; }
        public string Description { get; set; }
        public ScrewParameter screwParameter { get; set; }
    }
    public class ScrewProcessedTable
    {
        [Key]
        public int Id { get; set; }
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public float TD1 { get; set; }
        public float TD2 { get; set; }
        public float DT1 { get; set; }
        public float DT2 { get; set; }
        public float PR1 { get; set; }
        public float PR2 { get; set; }
        public ScrewParameter screwParameter { get; set; }
    }
    public class ScrewPredictedTable
    {
        [Key]
        public int Id { get; set; }
        public int SPId { get; set; }
        public DateTime Date { get; set; }
        public float TD1 { get; set; }
        public float TD2 { get; set; }
        public float DT1 { get; set; }
        public float DT2 { get; set; }
        public float PR1 { get; set; }
        public float PR2 { get; set; }
        public ScrewParameter screwParameter { get; set; }
    }
}
