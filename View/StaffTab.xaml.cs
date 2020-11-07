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

namespace Human_Resource_Information_System.View
{
    /// <summary>
    /// Interaction logic for StaffTab.xaml
    /// </summary>
    public partial class StaffTab : UserControl
    {
        public StaffTab()
        {
            InitializeComponent();
            StaffDetailsViewName.Visibility = Visibility.Collapsed;
            StaffListViewName.LoadStaffDetails += LoadStaffDetails;
        }
        private void LoadStaffDetails(object sender, EventArgs e)
        {
            if (e is SelectionChangedEventArgs sce)
            {
                if (sce.AddedItems.Count != 0)
                {
                    Staff selected_Staff = (Staff)sce.AddedItems[0];
                    StaffDetailsViewName.ShowStaffDetails(selected_Staff);
                    StaffDetailsViewName.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
