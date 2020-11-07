using System;

namespace Human_Resource_Information_System.Model
{
    public class Event
    {
       public DayOfWeek Day { get; set; }
        public Time Start { get; set; }
        public Time End { get; set; }
        public Campus Campus { get; set; }
        public string FromTimetoTime { get { return FromTtoTString(); } }
        private string FromTtoTString()
        {
            return ($"{Start}-{End}");
        }
    }
}
