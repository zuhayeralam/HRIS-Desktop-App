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
    /// Interaction logic for UnitTab.xaml
    /// </summary>
    public partial class UnitTab : UserControl
    {
        public UnitTab()
        {
            InitializeComponent();

            UnitListViewName.LoadUnitClasses += LoadUnitClasses;
        }

        private void LoadUnitClasses(object sender, EventArgs e)
        {
            if (e is SelectionChangedEventArgs sce)
            {
                if (sce.AddedItems.Count != 0)
                {
                    Unit selected_Unit = (Unit)sce.AddedItems[0];
                    UnitTimetableViewName.ShowUnitClasses(selected_Unit);
                }
            }
        }
    }
}
