using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tasks.Models
{
    public class CentrifugalParameter
    {
        [Key]
        public int Id { get; set; }
        public int FailureModeId { get; set; }
        public List<CentrifugalStagingTable> centrifugalStagingTable { get; set; }
        public List<CentrifugalCleaningTable> centrifugalCleaningTable { get; set; }
        public List<CentrifugalErrorTable> centrifugalErrorTable { get; set; }
        public List<CentrifugalProcessedTable> centrifugalProcessedTable { get; set; }
        public List<CentrifugalPredictedTable> centrifugalPredictedTable { get; set; }
        public Asset_FailureMode asset_failureModes { get; set; }
    }
    public class CentrifugalStagingTable
    {
        [Key]
        public int Id { get; set; }
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public string Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter { get; set; }
    }
    public class CentrifugalCleaningTable
    {
        [Key]
        public int Id { get; set; }
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public float Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter { get; set; }
    }
    public class CentrifugalErrorTable
    {
        [Key]
        public int Id { get; set; }
        public int CPId { get; set; }
        public int rowAffected { get; set; }
        public string Description { get; set; }
        public CentrifugalParameter centrifugalParameter { get; set; }
    }
    public class CentrifugalProcessedTable
    {
        [Key]
        public int Id { get; set; }
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public float Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter { get; set; }
    }
    public class CentrifugalPredictedTable
    {
        [Key]
        public int Id { get; set; }
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public float Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter { get; set; }
    }
}
