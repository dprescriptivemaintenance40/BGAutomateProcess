using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Plant.Models.Plant
{
    public class FailureMode
    {
        [Key]
        public int Id { get; set; }
        public int TagNumberId { get; set; }
        public string FailureModeName { get; set; }
        public string Description { get; set; }
        public string DateTimeBatchUploaded { get; set; }
        public string DateTimeBatchCompleted { get; set; }
        public int IsProcessCompleted { get; set; }
        public List<ScrewParameter> screwParameter { get; set; }
        public List<CentrifugalParameter> centrifugalParameter { get; set; }
        public List<ReciprocatingParameter> reciprocatingParameter { get; set; }
        public Equipment equipment;
    }
}
