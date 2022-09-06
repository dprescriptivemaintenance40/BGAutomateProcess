using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plant.Models.Plant
{
    public class CentrifugalParameter
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("failureModes")]
        public int FailureModeId { get; set; }
        public List<CentrifugalStagingTable> centrifugalStagingTable { get; set; }
        public List<CentrifugalCleaningTable> centrifugalCleaningTable { get; set; }
        public List<CentrifugalErrorTable> centrifugalErrorTable { get; set; }
        public List<CentrifugalProcessedTable> centrifugalProcessedTable { get; set; }
        public List<CentrifugalPredictedTable> centrifugalPredictedTable { get; set; }
        public FailureMode failureMode;
    }
    public class CentrifugalStagingTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("centrifugalParameter")]
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public string Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter;
    }
    public class CentrifugalCleaningTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("centrifugalParameter")]
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public string Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter;
    }
    public class CentrifugalErrorTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("centrifugalParameter")]
        public int CPId { get; set; }
        public int rowAffected { get; set; }
        public string Description { get; set; }
        public CentrifugalParameter centrifugalParameter;
    }
    public class CentrifugalProcessedTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("centrifugalParameter")]
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public string Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter;
    }
    public class CentrifugalPredictedTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("centrifugalParameter")]
        public int CPId { get; set; }
        public DateTime Date { get; set; }
        public string Vibration3H { get; set; }
        public CentrifugalParameter centrifugalParameter;
    }
}
