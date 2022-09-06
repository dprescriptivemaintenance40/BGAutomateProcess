using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.Models.Plant
{
    public class Equipment
    {
        [Key]
        public int Id { get; set; }
        public string TagNumber { get; set; }
        public string AssetName { get; set; }  //eg. ScrewCompressor 
        public List<FailureMode> failureMode { get; set; }
    }
}
