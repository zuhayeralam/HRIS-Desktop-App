using System;
using System.Collections.Generic;
using System.Linq;
using Human_Resource_Information_System.Model;
using Human_Resource_Information_System.Database;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Human_Resource_Information_System.Controller
{
    class UnitController
    {
        private Campus currSelectedCampus = Campus.All;
        private string currSearchNameCode;
        private String currSelectedUnitCode = "";
        readonly List<Unit> unitList = new List<Unit>();
        readonly List<UnitClass> unitClassList = new List<UnitClass>();
        //This section of code for unitlistview
        public ObservableCollection<Unit> LoadUnits(bool firstLoad)
        {
            if (firstLoad == true)
            {
                SchoolDBAdapter schoolDB = new SchoolDBAdapter();
                unitList.AddRange(schoolDB.FetchUnits());
                IEnumerable<Unit> tempUnitList = from unit in unitList orderby unit.UnitNameCode select unit;
                ObservableCollection<Unit> observableUnitList = new ObservableCollection<Unit>(tempUnitList);
                return observableUnitList;
            }
            else
            {
                return ApplyFilter(unitList);
            }
        }

        public void FilterByNameCode(String nameCode)
        {
            currSearchNameCode = nameCode;
        }
        public void SetSelectedUnitCode(String unitCode)
        {
            currSelectedUnitCode = unitCode;
        }

        private ObservableCollection<Unit> ApplyFilter(List<Unit> unitList)
        {
            IEnumerable<Unit> updatedList;
            updatedList = from unit in unitList orderby unit.UnitNameCode where unit.UnitNameCode.IndexOf(currSearchNameCode, StringComparison.OrdinalIgnoreCase) >= 0 select unit;
            ObservableCollection<Unit> observableUpdatedList = new ObservableCollection<Unit>(updatedList);
            return observableUpdatedList;
        }

        //This section of code is for unittimetableview
        public void FilterByCampus(Campus campus)
        {
            currSelectedCampus = campus;
        }

        //Dealing with unit timetable from here
        public ObservableCollection<UnitClass> LoadUnitClassesByUnitCode()
        {
            string unitCode = currSelectedUnitCode;
            int result_count = (from c in unitClassList where c.ClassUnitCode.Equals(unitCode) select c).Count();
            if (result_count > 0)
            {
                return ApplyFilterbyUnitCode(unitClassList);
            }
            else
            {
                SchoolDBAdapter schoolDB = new SchoolDBAdapter();
                unitClassList.AddRange(schoolDB.FetchUnitClasses(unitCode));
                return ApplyFilterbyUnitCode(unitClassList);
            }
        }

        private ObservableCollection<UnitClass> ApplyFilterbyUnitCode(List<UnitClass> unitClassList)
        {
            string unitCode = currSelectedUnitCode;
            IEnumerable<UnitClass> updatedList;
            ObservableCollection<UnitClass> observableUpdatedList;
            if (currSelectedCampus == Campus.Hobart || currSelectedCampus == Campus.Launceston)
            {
                updatedList = from c in unitClassList
                              orderby c.Day,
                              DateTime.Parse(c.Start.ToString())
                              where c.Campus.Equals(currSelectedCampus) && c.ClassUnitCode.Equals(unitCode)
                              select c;

                observableUpdatedList = new ObservableCollection<UnitClass>(updatedList);
            }
            else
            {
                updatedList = from c in unitClassList
                              orderby c.Day,
                              DateTime.Parse(c.Start.ToString())
                              where c.ClassUnitCode.Equals(unitCode)
                              select c;

                observableUpdatedList = new ObservableCollection<UnitClass>(updatedList);
            }

            return observableUpdatedList;
        }

        public List<ColourGridRow> LoadClashMap(List<UnitClass> unitClasses)
        {
            List<int> relevantStaffIDs = unitClasses.Select(c => c.StaffID).ToList();
            List<Event> staffConsultationTimesForUnit = new List<Event>();
            staffConsultationTimesForUnit.AddRange(StaffController.LoadStaffConsultationsForClashMap(relevantStaffIDs));

            List<ColourGridRow> agList = new List<ColourGridRow>();
            for (int i = 9; i < 17; i++)
            {
                ColourGridRow examplerow = new ColourGridRow();
                examplerow.TimeSlot = new Time(i, 0, 0);
                for (int j = 1; j < 6; j++)
                {
                    foreach (Event eachConsultation in staffConsultationTimesForUnit)
                    {
                        if (eachConsultation.Day == (DayOfWeek)j)
                        {
                            if (Time.IsTimeSlotValid(eachConsultation.Start, examplerow.TimeSlot, eachConsultation.End) == true)
                            {
                                examplerow.Colours[j - 1] = new SolidColorBrush(Colors.LawnGreen);
                            }
                        }
                    }

                    foreach (Event eachClass in unitClasses)
                    {
                        if (eachClass.Day == (DayOfWeek)j)
                        {
                            if (Time.IsTimeSlotValid(eachClass.Start, examplerow.TimeSlot, eachClass.End) == true)
                            {
                                if (examplerow.Colours[j - 1] == null)
                                {
                                    examplerow.Colours[j - 1] = new SolidColorBrush(Colors.LawnGreen);
                                }
                                else
                                {
                                    string clashString = "clash";
                                    examplerow.Values[j - 1] = (string)clashString;
                                    examplerow.Colours[j - 1] = new SolidColorBrush(Colors.Red);
                                }
                            }
                        }
                    }
                }
                agList.Add(examplerow);
            }
            return agList;
        }
    }
}

