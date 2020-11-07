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
using Human_Resource_Information_System.Controller;
using Human_Resource_Information_System.Model;

namespace Human_Resource_Information_System.View
{
    /// <summary>
    /// Interaction logic for StaffDetailsView.xaml
    /// </summary>
    public partial class StaffDetailsView : UserControl
    {
        readonly StaffController staffDetailCtrl;
        public StaffDetailsView()
        {
            staffDetailCtrl = new StaffController();
            InitializeComponent();
            tblMapActivityGrid.Visibility = Visibility.Collapsed;
        }

        public void ShowStaffDetails(Staff staff)
        {
            int staffID = staff.ID;
            this.DataContext = staffDetailCtrl.LoadStaffDetail(staffID);
        }

        public void ShowStaffDetails(int staffID)
        {
            this.DataContext = staffDetailCtrl.LoadStaffDetail(staffID);
        }

        public event EventHandler SwitchtoUnitTimetableEvent;
        private void SwitchtoUnitTimetable(object sender, RoutedEventArgs e)
        {
            SwitchtoUnitTimetableEvent.Invoke(sender, e);
        }

        private void ShowActivityGrid(object sender, RoutedEventArgs e)
        {
            tblMapActivityGrid.Visibility = Visibility.Visible;
        }

        private void HideActivityGrid(object sender, RoutedEventArgs e)
        {
            tblMapActivityGrid.Visibility = Visibility.Collapsed;
        }
    }
}
