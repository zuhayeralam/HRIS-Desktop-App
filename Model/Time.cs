using System;

namespace Human_Resource_Information_System.Model
{
    public class Time
    {
        private int hour;
        private int minute;
        private int second;

        public Time()
        {
            SetTime(0, 0, 0);
        }

        public Time(int h, int m, int s)
        {
            hour = h;
            minute = m;
            second = s;
        }

        public void SetTime(int h, int m, int s)
        {
            hour = h;
            minute = m;
            second = s;
        }

        public Time(String dbTimeData)
        {
            string hour = dbTimeData.Substring(0, 2);
            string minute = dbTimeData.Substring(3, 2);
            string second = dbTimeData.Substring(6, 2);

            int h = Convert.ToInt32(hour);
            int m = Convert.ToInt32(minute);
            int s = Convert.ToInt32(second);

            this.hour = h;
            this.minute = m;
            this.second = s;
        }

        public string ToAMPMString()
        {
            string r;

            if (hour >= 12)
            {
                if (hour == 12)
                {
                    r = "12";
                }
                else
                {
                    r = string.Format("{0:00}", hour - 12);
                }
                r = r + ":" + string.Format("{0:00}:{1:00}", minute, second) + " PM";
            }
            else
            {
                if (hour == 0)
                {
                    r = "12";
                }
                else
                {
                    r = string.Format("{0:00}", hour);
                }
                r = r + ":" + string.Format("{0:00}:{1:00}", minute, second) + " AM";
            }
            return r;
        }

        public override string ToString()
        {
            return string.Format("{0:00}:{1:00}", hour, minute);
        }

        public static bool IsWithinTime(Time start, Time now, Time end)
        {
            if (now.hour >= start.hour && now.hour <= end.hour)
            {
                if (now.hour < end.hour && now.hour > start.hour)
                {
                    return true;
                }
                else if (now.hour == start.hour && now.minute >= start.minute)
                {
                    return true;
                }
                else if (now.hour == end.hour && now.minute <= end.minute)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsTimeSlotValid(Time start, Time timeSlot, Time end)
        {
            if (timeSlot.hour >= start.hour && end.hour > timeSlot.hour)
            {
                return true;
            }
            return false;
        }

        public string ToTimeSlotString()
        {
            return string.Format("{0:00}:{1:00}-{2:00}:{3:00}", hour, minute, hour + 1, 0);
        }
    }
}