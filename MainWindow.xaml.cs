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

namespace Car_Storage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public JSON_Interaction JSON = new JSON_Interaction(); // adds the main window settings and sets the array size
        int carsLength = ((App)Application.Current).cars.GetLength(); //the size of the car array
        CarObject selectedCar; // the selected item in DataGrid
        public MainWindow()
        {
            InitializeComponent();
        }


        private void NewCatalogBtn_Click(object sender, RoutedEventArgs e) // opens a new file window with a filter for JSONS
        {
            AddItemBtn.IsEnabled = JSON.NewJSON();
            if (AddItemBtn.IsEnabled) { JSON.UpdateJsonFile(); }
            UpdateData();
        }


        private void OpnCatalogBtn_Click(object sender, RoutedEventArgs e) // opens a select window with a filter for JSONS
        {
            AddItemBtn.IsEnabled = JSON.OpenJSON();
            JSON.DeSerialiseJsonData();

            UpdateData();
        }


        private void ItemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) // if the selected object, update the selected index item and Edit button IsEnabled
        {
            if (carsLength >= ItemDataGrid.SelectedIndex && ItemDataGrid.SelectedIndex >= 0)
            {
                selectedCar = (CarObject)ItemDataGrid.SelectedItem;
                EditSelectedBtn.IsEnabled = true;
            }
        }


        private void EditSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            EditObject(selectedCar);
        }


        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            Data dataWindow = new Data(new CarObject());
            dataWindow.ShowDialog();
            JSON.UpdateJsonFile();
            UpdateData();
        }


        private void ClearSortBtn_Click(object sender, RoutedEventArgs e) // re-runs the parse script
        {
            //re-runs the parse view script
            ClearSortBtn.IsEnabled = false;
            ((App)Application.Current).cars.SetSearch(null, null);
            UpdateData();
        }

        
        private void SortListBtn_Click(object sender, RoutedEventArgs e) //opens the sort list then sets the output
        {
            Search dataWindow = new Search();
            dataWindow.ShowDialog();
            if (dataWindow.mode == "sort")
            {
                ClearSortBtn.IsEnabled = true;
            }
            else if(dataWindow.mode == "item")
            {
                EditObject(dataWindow.basePeramiter);
            }
        }

        void EditObject(CarObject _selectedCar)
        {
            Data dataWindow = new Data(_selectedCar);
            dataWindow.ShowDialog();
            JSON.UpdateJsonFile();
            UpdateData();
        }

        void UpdateData()
        {
            ItemDataGrid.ItemsSource = "";
            ItemDataGrid.ItemsSource = ((App)Application.Current).cars.ParseArray();
            int _count = ((App)Application.Current).cars.GetUsedLength();
            AddItemBtn.Content = $"New Item ({_count}/{carsLength})";
            SortListBtn.IsEnabled = _count > 0;
            AddItemBtn.IsEnabled = _count <= carsLength;
        }
    }
}
