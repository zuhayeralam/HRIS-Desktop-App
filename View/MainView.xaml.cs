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
using System.Windows.Shapes;

namespace Human_Resource_Information_System.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            UnitTabName.UnitTimetableViewName.SwitchtoStaffEvent += SwitchtoStaffTab;
            StaffTabName.StaffDetailsViewName.SwitchtoUnitTimetableEvent += SwitchtoUnitTab;
        }

        private void SwitchtoStaffTab(object sender, EventArgs e)
        {
            TextBlock textblock = (TextBlock)sender;
            int id = (int)textblock.Tag;
            myTabControl.SelectedIndex = 0;
            StaffTabName.StaffDetailsViewName.ShowStaffDetails(id);
            StaffTabName.StaffDetailsViewName.Visibility = Visibility.Visible;
            StaffTabName.StaffListViewName.lbStaffList.UnselectAll();
        }

        private void SwitchtoUnitTab(object sender, EventArgs e)
        {
            TextBlock textblock = (TextBlock)sender;
            string unitCode = (string)textblock.Tag;
            myTabControl.SelectedIndex = 1;
            UnitTabName.UnitTimetableViewName.ShowUnitClasses(unitCode);
            UnitTabName.UnitTimetableViewName.Visibility = Visibility.Visible;
            UnitTabName.UnitListViewName.lbUnitList.UnselectAll();
        }
    }
}
