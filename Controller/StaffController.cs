using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Human_Resource_Information_System.Database;
using Human_Resource_Information_System.Model;

namespace Human_Resource_Information_System.Controller
{
    class StaffController
    {

        private string currNameFilter = "";
        private Category currCategoryFilter = Category.All;
        private readonly List<Staff> staffList = new List<Staff>();

        public ObservableCollection<Staff> LoadStaffs(bool firstLoad)
        {
            if (firstLoad == true)
            {
                SchoolDBAdapter schoolDB = new SchoolDBAdapter();
                staffList.AddRange(schoolDB.FetchBasicStaffDetails());
                IEnumerable<Staff> tempStaffList = from staff in staffList orderby staff.FamilyName select staff;
                ObservableCollection<Staff> observableStaffList = new ObservableCollection<Staff>(tempStaffList);
                return observableStaffList;
            }
            else
            {
                return ApplyFilter(staffList);
            }
        }

        public void FilterBy(Category category)
        {
            currCategoryFilter = category;
        }

        public void FilterByName(String name)
        {
            currNameFilter = name;
        }

        private ObservableCollection<Staff> ApplyFilter(List<Staff> staffList)
        {
            IEnumerable<Staff> updatedList;
            if (currCategoryFilter == Category.All)
            {
                updatedList = from staff in staffList
                              orderby staff.FamilyName
                              where staff.GivenName.IndexOf(currNameFilter, StringComparison.OrdinalIgnoreCase) >= 0 || staff.FamilyName.IndexOf(currNameFilter, StringComparison.OrdinalIgnoreCase) >= 0
                              select staff;
            }
            else
            {
                updatedList = from staff in staffList
                              orderby staff.FamilyName
                              where staff.Category.Equals(currCategoryFilter) && ((staff.GivenName.IndexOf(currNameFilter, StringComparison.OrdinalIgnoreCase) >= 0) || (staff.FamilyName.IndexOf(currNameFilter, StringComparison.OrdinalIgnoreCase) >= 0))
                              select staff;

            }

            ObservableCollection<Staff> observableUpdatedList = new ObservableCollection<Staff>(updatedList);
            return observableUpdatedList;
        }

        //Staff detail activity from here
        public ObservableCollection<Staff> LoadStaffDetail(int staffID)
        {
            SchoolDBAdapter schoolDB = new SchoolDBAdapter();
            staffList.Add(schoolDB.FetchStaffDetails(staffID));
            IEnumerable<Staff> tempStaffList = from staff in staffList where staff.ID.Equals(staffID) select staff;
            List<Staff> tempstaffListTwo = tempStaffList.ToList();
            tempstaffListTwo[0].ActivityGridRows = GenerateActivityGridRows(tempstaffListTwo[0].Consultations, tempstaffListTwo[0].TeachingUnits);
            tempStaffList = tempstaffListTwo.AsEnumerable();
            ObservableCollection<Staff> observableStaffList = new ObservableCollection<Staff>(tempStaffList);
            return observableStaffList;
        }

        public List<ColourGridRow> GenerateActivityGridRows(List<Event> consultations, List<UnitClass> modelUnits)
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
                    foreach (Event eachConsultation in consultations)
                    {
                        if (eachConsultation.Day == (DayOfWeek)j)
                        {
                            if (Time.IsTimeSlotValid(eachConsultation.Start, examplerow.TimeSlot, eachConsultation.End) == true)
                            {
                                examplerow.Colours[j - 1] = new SolidColorBrush(Colors.DarkBlue);
                            }
                        }
                    }

                    foreach (UnitClass eachModel in modelUnits)
                    {
                        if (eachModel.Day == (DayOfWeek)j)
                        {
                            if (Time.IsTimeSlotValid(eachModel.Start, examplerow.TimeSlot, eachModel.End) == true)
                            {
                                examplerow.Colours[j - 1] = new SolidColorBrush(Colors.Red);
                            }
                        }
                    }
                }
                agList.Add(examplerow);
            }
            return agList;
        }

        //Code for unit controller to use
        public static List<Event> LoadStaffConsultationsForClashMap(List<int> staffIDs)
        {
            List<Event> consultationListforClashMap = new List<Event>();
            SchoolDBAdapter schoolDB = new SchoolDBAdapter();
            foreach (int eachID in staffIDs)
            {
                consultationListforClashMap.AddRange(schoolDB.FetchConsultationTimes(eachID));
            }
            return consultationListforClashMap;
        }
    }
}

