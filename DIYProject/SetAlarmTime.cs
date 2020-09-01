using System;
using System.Collections.Generic;
using System.Text;

namespace DIYProject
{
    public class SetAlarmTime
    {
        public enum TimeTypes
        {
            AM,
            PM,
            MORNING,
            AFTERNOON,
            EVENING,
            Null
        }

        public TimeTypes? TimeType { get; set; }

        public string AlarmTime { get; set; }

    }
}
