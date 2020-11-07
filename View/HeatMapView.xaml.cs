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

namespace Human_Resource_Information_System.View
{
    /// <summary>
    /// Interaction logic for HeatMapView.xaml
    /// </summary>
    public partial class HeatMapView : UserControl
    {
        public List<Color> HeatMapColorChoices { get; set; }
        public HeatMapController heatMapCtrl;
        public bool consultHeatMapLoaded = false;
        public bool classHeatMapLoaded = false;
        public HeatMapView()
        {
            heatMapCtrl = new HeatMapController();
            InitializeComponent();
            tblMapConsultationHeat.Visibility = Visibility.Collapsed;
            tblMapClassHeat.Visibility = Visibility.Collapsed;
            cbHMCampus.Visibility = Visibility.Hidden;
            tbHMCampus.Visibility = Visibility.Hidden;
        }

        private void cbColorChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e is SelectionChangedEventArgs sce)
            {
                if (sce.AddedItems.Count != 0)
                {
                    SolidColorBrush selected_ColorBrush = (SolidColorBrush)sce.AddedItems[0];
                    Color selected_Color = selected_ColorBrush.Color;
                    heatMapCtrl.ChangePrimaryColor(selected_Color);
                    if (consultHeatMapLoaded == true)
                    {
                        tblMapConsultationHeat.ItemsSource = heatMapCtrl.LoadConsultationHeatMap(false);
                    }
                    if (classHeatMapLoaded == true)
                    {
                        tblMapClassHeat.ItemsSource = heatMapCtrl.LoadClassesHeatMap(false);
                    }
                }
            }
        }

        private void HMComboBox_CampusChanged(object sender, SelectionChangedEventArgs e)
        {
            Campus selectedCampus = (Campus)Enum.Parse(typeof(Campus), cbHMCampus.SelectedItem.ToString());
            heatMapCtrl.FilterByCampus(selectedCampus);
            if (classHeatMapLoaded == true)
            {
                tblMapClassHeat.ItemsSource = heatMapCtrl.LoadClassesHeatMap(false);
            }
        }

        private void Button_Click_Consultation_HeatMap(object sender, RoutedEventArgs e)
        {
            tblMapConsultationHeat.Visibility = Visibility.Visible;
            tblMapConsultationHeat.ItemsSource = heatMapCtrl.LoadConsultationHeatMap(true);
            consultHeatMapLoaded = true;
        }

        private void Button_Click_Class_HeatMap(object sender, RoutedEventArgs e)
        {
            tblMapClassHeat.Visibility = Visibility.Visible;
            cbHMCampus.Visibility = Visibility.Visible;
            tbHMCampus.Visibility = Visibility.Visible;
            tblMapClassHeat.ItemsSource = heatMapCtrl.LoadClassesHeatMap(true);
            classHeatMapLoaded = true;
        }
    }
}
