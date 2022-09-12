using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Models
{
    public class Asset_Equipment
    {
        public int Id { get; set; }
        public string TagNumber { get; set; }
        public string AssetName { get; set; }  //eg. ScrewCompressor 
        public List<Asset_FailureMode> asset_failureModes { get; set; }
    }
}
