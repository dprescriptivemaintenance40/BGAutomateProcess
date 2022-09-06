using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plant.Models.Plant
{
    public class ReciprocatingParameter
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("failureModes")]
        public int FailureModeId { get; set; }
        public List<ReciprocatingStagingTable> reciprocatingStagingTable { get; set; }
        public List<ReciprocatingCleaningTable> reciprocatingCleaningTable { get; set; }
        public List<ReciprocatingErrorTable> reciprocatingErrorTable { get; set; }
        public List<ReciprocatingProcessedTable> reciprocatingProcessedTable { get; set; }
        public List<ReciprocatingPredictedTable> reciprocatingPredictedTable { get; set; }
        public FailureMode failureModes;
    }
    public class ReciprocatingStagingTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("reciprocatingParameter")]
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public string TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter;
    }
    public class ReciprocatingCleaningTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("reciprocatingParameter")]
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public string TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter;

    }
    public class ReciprocatingErrorTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("reciprocatingParameter")]
        public int RPId { get; set; }
        public int rowAffected { get; set; }
        public string Description { get; set; }
        public ReciprocatingParameter reciprocatingParameter;
    }
    public class ReciprocatingProcessedTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("reciprocatingParameter")]
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public string TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter;
    }
    public class ReciprocatingPredictedTable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("reciprocatingParameter")]
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public string TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter;
    }
}
