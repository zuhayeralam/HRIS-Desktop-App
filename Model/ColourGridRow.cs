using System.Windows.Media;

namespace Human_Resource_Information_System.Model
{
    public class ColourGridRow
    {
        public Time TimeSlot { get; set; }
        public string TimeSlotString { get { return TimeSlotStringGenerator(this.TimeSlot); } }
        public object[] Values { get; set; } = new object[5];
        public Brush[] Colours { get; set; } = new Brush[5];
        private string TimeSlotStringGenerator(Time timeSlot)
        {
            return timeSlot.ToTimeSlotString();
        }
    }
}
