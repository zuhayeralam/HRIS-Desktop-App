using System;
using System.Collections.Generic;

namespace Human_Resource_Information_System.Model
{
    public class Staff
    {
        public int ID { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Title { get; set; }
        public Campus Campus { get; set; }
        public string Room { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public Category Category { get; set; }
        public string ListFormatString { get { return ToListFormatString(); } }
        public string FullNameString { get { return ToFullNameString(); } }
        public List<Event> Consultations { get; set; }
        public List<UnitClass> TeachingUnits { get; set; }
        public List<ColourGridRow> ActivityGridRows { get; set; }
        public List<Availability> StaffAvailability { get { return AvailabilityofStaff(); } }

        private string ToFullNameString()
        {
            return $"{Title} {GivenName} {FamilyName}";
        }

        private string ToListFormatString()
        {
            return $"{FamilyName}, {GivenName} ({Title})";
        }
        private List<Availability> AvailabilityofStaff()
        {
            Availability staffAvailability = new Availability
            {
                AvailabilityProp = AvailabilityType.Free
            };
            var dateTimeOfNow = DateTime.Now;
            Time currTime = new Time(dateTimeOfNow.ToString("HH:mm:ss"));
            //Time currTime = new Time("17:01:00");
            DayOfWeek currDay = dateTimeOfNow.DayOfWeek;
            //DayOfWeek currDay = DayOfWeek.Monday;
            //Above commented out code is to test John Bechett's availability during consultation hours
            foreach (Event eachConsultation in this.Consultations)
            {
                if (eachConsultation.Day == currDay)
                {
                    if (Time.IsWithinTime(eachConsultation.Start, currTime, eachConsultation.End) == true)
                    {
                        staffAvailability.AvailabilityProp = AvailabilityType.Consulting;
                    }
                }
            }

            foreach (UnitClass eachClass in this.TeachingUnits)
            {
                if (eachClass.Day == currDay)
                {

                    if (Time.IsWithinTime(eachClass.Start, currTime, eachClass.End) == true)
                    {
                        staffAvailability.AvailabilityProp = AvailabilityType.Teaching;
                        staffAvailability.Room = eachClass.Room;
                        staffAvailability.UnitCode = eachClass.ClassUnitCode;
                    }
                }
            }
            List<Availability> avList = new List<Availability>
            {
                staffAvailability
            };
            return avList;
        }
    }
}