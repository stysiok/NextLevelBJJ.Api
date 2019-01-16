using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Settings
    {
        public int Id { get; set; }
        public string CameraName { get; set; }
        public int CameraFrameRate { get; set; }
        public int RepeatScanCounter { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }
}
