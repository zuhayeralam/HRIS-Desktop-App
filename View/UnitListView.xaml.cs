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

namespace Human_Resource_Information_System.View
{
    /// <summary>
    /// Interaction logic for UnitListView.xaml
    /// </summary>
    public partial class UnitListView : UserControl
    {
        readonly UnitController unitCtrl;
        public UnitListView()
        {
            unitCtrl = new UnitController();
            InitializeComponent();
            lbUnitList.ItemsSource = unitCtrl.LoadUnits(true);
        }

        private void TextBox_FilterUnit(object sender, KeyEventArgs e)
        {

            TextBox tBox = (TextBox)sender;
            String userEnteredNameCode = tBox.Text;
            unitCtrl.FilterByNameCode(userEnteredNameCode);
            lbUnitList.ItemsSource = unitCtrl.LoadUnits(false);

        }

        public event EventHandler LoadUnitClasses;
        private void unitNameCode_Selected(object sender, SelectionChangedEventArgs e)
        {
            //if ((Unit)lbUnitList.SelectedItem != null)
            //{
            //    Unit temp = (Unit)lbUnitList.SelectedItem;
            //    string id = temp.Code;
            //    MessageBox.Show(id.ToString());
            //}

            LoadUnitClasses.Invoke(sender, e);
        }
    }
}
