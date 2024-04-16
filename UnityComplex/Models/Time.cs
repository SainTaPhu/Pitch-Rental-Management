using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Time
    {
        public int IdTime { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
