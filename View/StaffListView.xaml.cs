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
    /// Interaction logic for StaffListView.xaml
    /// </summary>
    public partial class StaffListView : UserControl
    {
        readonly StaffController staffCtrl;
        public StaffListView()
        {
            staffCtrl = new StaffController();
            InitializeComponent();
            lbStaffList.ItemsSource = staffCtrl.LoadStaffs(true);
            //cbCategory.SelectedItem = Category.All;
        }

        private void ComboBox_CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            Category selectedCategory = (Category)Enum.Parse(typeof(Category), cbCategory.SelectedItem.ToString());
            staffCtrl.FilterBy(selectedCategory);
            lbStaffList.ItemsSource = staffCtrl.LoadStaffs(false);
        }

        private void TextBox_FilterName(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                TextBox tBox = (TextBox)sender;
                String userEnteredName = tBox.Text;
                staffCtrl.FilterByName(userEnteredName);
                lbStaffList.ItemsSource = staffCtrl.LoadStaffs(false);
            }
        }

        //Activity to link stafflist to staffdetails
        public event EventHandler LoadStaffDetails;
        private void staffName_Selected(object sender, SelectionChangedEventArgs e)
        {
            LoadStaffDetails.Invoke(sender, e);
        }
    }
}
