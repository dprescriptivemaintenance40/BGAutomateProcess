using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tasks.Models
{
    public class ReciprocatingParameter
    {
        [Key]
        public int Id { get; set; }
        public int FailureModeId { get; set; }
        public List<ReciprocatingStagingTable> reciprocatingStagingTable { get; set; }
        public List<ReciprocatingCleaningTable> reciprocatingCleaningTable { get; set; }
        public List<ReciprocatingErrorTable> reciprocatingErrorTable { get; set; }
        public List<ReciprocatingProcessedTable> reciprocatingProcessedTable { get; set; }
        public List<ReciprocatingPredictedTable> reciprocatingPredictedTable { get; set; }
        public Asset_FailureMode asset_failureModes { get; set; }
    }
    public class ReciprocatingStagingTable
    {
        [Key]
        public int Id { get; set; }
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public string TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter { get; set; }
    }
    public class ReciprocatingCleaningTable
    {
        [Key]
        public int Id { get; set; }
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public float TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter { get; set; }

    }
    public class ReciprocatingErrorTable
    {
        [Key]
        public int Id { get; set; }
        public int RPId { get; set; }
        public int rowAffected { get; set; }
        public string Description { get; set; }
        public ReciprocatingParameter reciprocatingParameter { get; set; }
    }
    public class ReciprocatingProcessedTable
    {
        [Key]
        public int Id { get; set; }
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public float TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter { get; set; }
    }
    public class ReciprocatingPredictedTable
    {
        [Key]
        public int Id { get; set; }
        public int RPId { get; set; }
        public DateTime Date { get; set; }
        public float TDValve { get; set; }
        public ReciprocatingParameter reciprocatingParameter { get; set; }
    }
}
