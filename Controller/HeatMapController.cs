using Human_Resource_Information_System.Model;
using Human_Resource_Information_System.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Human_Resource_Information_System.Controller
{
    public class HeatMapController
    {
        private Campus currSelectedCampus = Campus.All;
        private readonly HeatMap heatMap = new HeatMap();
        private Color HeatMapColor{get; set; } = Colors.LightCyan;
        public List<ColourGridRow> LoadConsultationHeatMap(bool firstLoad)
        {
            if (firstLoad == true)
            {
                SchoolDBAdapter schoolDB = new SchoolDBAdapter();
                heatMap.AllConsultations = schoolDB.FetchAllConsultations();
                return GenerateHeatMap(heatMap.AllConsultations);
            }
            else
            {
                return GenerateHeatMap(heatMap.AllConsultations);
            }
        }

        public List<ColourGridRow> LoadClassesHeatMap(bool firstLoad)
        {
            if (firstLoad == true)
            {
                SchoolDBAdapter schoolDB = new SchoolDBAdapter();
                heatMap.AllUnitClasses = schoolDB.FetchAllUnitClasses();
                return GenerateHeatMap(heatMap.AllUnitClasses);
            }
            else
            {
                return GenerateHeatMap(ApplyFilterByCampus(heatMap.AllUnitClasses));
            }
        }
        public void FilterByCampus(Campus campus)
        {
            currSelectedCampus = campus;
        }

        private List<Event> ApplyFilterByCampus(List<Event> events)
        {
            IEnumerable<Event> updatedList;
            List<Event> filteredEvents;
            if (currSelectedCampus == Campus.Hobart || currSelectedCampus == Campus.Launceston)
            {
                updatedList = from e in events
                              where e.Campus.Equals(currSelectedCampus)
                              select e;

                filteredEvents = updatedList.ToList();
            }
            else
            {
                return events;
            }
            return filteredEvents;
        }

        public List<ColourGridRow> GenerateHeatMap(List<Event> events)
        {
            List<ColourGridRow> agList = new List<ColourGridRow>();
            for (int i = 9; i < 17; i++)
            {
                ColourGridRow examplerow = new ColourGridRow
                {
                    TimeSlot = new Time(i, 0, 0)
                };
                for (int j = 1; j < 6; j++)
                {
                    foreach (Event eachClass in events)
                    {
                        if (eachClass.Day == (DayOfWeek)j)
                        {
                            if (Time.IsTimeSlotValid(eachClass.Start, examplerow.TimeSlot, eachClass.End) == true)
                            {
                                examplerow.Colours[j - 1] = new SolidColorBrush(HeatMapColor);
                                if (examplerow.Values[j - 1] == null)
                                {
                                    examplerow.Values[j - 1] = 1;
                                }
                                else
                                {
                                    examplerow.Values[j - 1] = (int)examplerow.Values[j - 1] + 1;
                                    int examplerowValueInt = (int)examplerow.Values[j - 1];
                                    examplerow.Colours[j - 1] = new SolidColorBrush(ChangeLightness(HeatMapColor, examplerowValueInt));
                                }
                            }
                        }
                    }
                }
                agList.Add(examplerow);
            }
            return agList;
        }

        public void ChangePrimaryColor(Color color)
        {
            this.HeatMapColor = color;
        }

        private Color ChangeLightness(Color color, int forCoef)
        {
            double coef;
            switch (forCoef)
            {
                case 2:
                    coef = 0.75;
                    break;
                case 3:
                    coef = 0.55;
                    break;
                case 4:
                    coef = 0.35;
                    break;
                case 5:
                    coef = 0.25;
                    break;
                default:
                    coef = 0.9;
                    break;
            }
            return Color.FromRgb((byte)(color.R * coef), (byte)(color.G * coef), (byte)(color.B * coef));
        }
    }
}

