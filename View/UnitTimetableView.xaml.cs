using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Human_Resource_Information_System.Model;
using Human_Resource_Information_System.Controller;
using System.Collections.ObjectModel;
namespace Human_Resource_Information_System.View
{
    /// <summary>
    /// Interaction logic for UnitTimetableView.xaml
    /// </summary>
    public partial class UnitTimetableView : UserControl
    {
        UnitController unitCtrl;
        public ObservableCollection<UnitClass> unitClasses;
        public UnitTimetableView()
        {
            unitCtrl = new UnitController();
            InitializeComponent();
            //cmUnitTimeTable.Visibility = Visibility.Collapsed;
        }
        public void ShowUnitClasses(Unit unit)
        {
            unitCtrl.SetSelectedUnitCode(unit.Code);
            unitClasses = unitCtrl.LoadUnitClassesByUnitCode();
            dgUnitTimetable.ItemsSource = unitClasses;
        }
        public void ShowUnitClasses(string unitCode)
        {
            unitCtrl.SetSelectedUnitCode(unitCode);
            unitClasses = unitCtrl.LoadUnitClassesByUnitCode();
            dgUnitTimetable.ItemsSource = unitClasses;
        }

        private void ComboBox_CampusChanged(object sender, SelectionChangedEventArgs e)
        {
            Campus selectedCampus = (Campus)Enum.Parse(typeof(Campus), cbCampus.SelectedItem.ToString());
            unitCtrl.FilterByCampus(selectedCampus);
            unitClasses = unitCtrl.LoadUnitClassesByUnitCode();
            dgUnitTimetable.ItemsSource = unitClasses;
        }

        public event EventHandler SwitchtoStaffEvent;
        private void SwitchtoStaffDetails(object sender, RoutedEventArgs e)
        {
            SwitchtoStaffEvent.Invoke(sender, e);
        }

        private void Button_Click_GenerateCM(object sender, RoutedEventArgs e)
        {
            //code for clashmap
            if (unitClasses != null)
            {
                IEnumerable<UnitClass> unitClassesAsEnumerable = unitClasses.AsEnumerable<UnitClass>();
                List<UnitClass> unitClassesAsList = unitClassesAsEnumerable.ToList();
                cmUnitTimeTable.ItemsSource = unitCtrl.LoadClashMap(unitClassesAsList);
            }
        }
    }
}
