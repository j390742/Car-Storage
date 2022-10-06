using Microsoft.Win32;
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

namespace Car_Storage
{
    /// <summary>
    /// Interaction logic for Data.xaml
    /// </summary>
    public partial class Data : Window
    {
        int originalBay; //where the car came from
        int carsLength = ((App)Application.Current).cars.GetLength(); // the size of the array
        JSON_Interaction json = new JSON_Interaction();

        public Data(CarObject _selected)
        {
            originalBay = _selected.currentBay;
            InitializeComponent();
            fillComboBox();
            UpdateData(_selected);
        }


        void fillComboBox() // usiing the max length, sets the options in the combo box ///there must be a better way to do this, some sort of range system maybe?
        {
            for (int i = 1; i <= carsLength; i++)
            {
                CurrentBayCbBx.Items.Add($"{i}"); /// The reason for the formatted string is futher development
            }
            CurrentBayCbBx.SelectedIndex = ((App)Application.Current).cars.GetUsedLength();
        }

        void UpdateData(CarObject _selected) //adds the already present data from the imported object to the apropreate data boxes
        {
            try
            {
                Imageimg.Source = new BitmapImage(new Uri(_selected.photoFileName));
                PhotoTxBx.Text = _selected.photoFileName;
            }
            catch
            {  }

            if(_selected.currentBay >= 0)
            {
                CurrentBayCbBx.SelectedIndex = _selected.currentBay - 1;
                originalBay = _selected.currentBay - 1;
            }

            if (_selected != null) //if the selected item as a whole is not null
            {
                //enter each item in (as long as they aren't null too
                RegNoTxBx.Text = _selected.registrationNumber ?? "";
                MakeTxBx.Text = _selected.make ?? "";
                ModelTxBx.Text = _selected.model ?? "";
                YearTxBx.Text = _selected.year.ToString();
                PriceTxBx.Text = _selected.price.ToString();
            }
            else
            {
                //if the object is null, make it all blank.
                RegNoTxBx.Text = "";
                MakeTxBx.Text = "";
                ModelTxBx.Text = "";
                YearTxBx.Text = "";
                PriceTxBx.Text = "";
            }
        }


        private void BrowsePhotoBtn_Click(object sender, RoutedEventArgs e) // asks for a Open File Dialog then sets the output to the aproopreate fields
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == true)
            {
                Imageimg.Source = new BitmapImage(new Uri(@"" + openFileDialog.FileName));
                PhotoTxBx.Text = openFileDialog.FileName;
            }
        }


        private void ContinueBtn_Click(object sender, RoutedEventArgs e) //checks against the source array and then tests the data
        {
            CarObject carObject = TestData();
            
            if(carObject != null)
            {
                if (((App)Application.Current).cars.GetCar(carObject.currentBay) != null )
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("There is a car in that bay currently.\n Are you sure you want to overide?", "Change Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                if (originalBay >= 0) { ((App)Application.Current).cars.SetCar(carObject, originalBay, carObject.currentBay); }
                else { ((App)Application.Current).cars.SetCar(carObject, carObject.currentBay); }
                this.Close();
            }
        }

        CarObject TestData() // for each of the data fields, test and if an error occurs return the name
        {
            CarObject _packingObject = new CarObject();

            try { _packingObject.currentBay = CurrentBayCbBx.SelectedIndex; } 
            catch{ ErrorMessage("Current bay"); }

            try { _packingObject.make = MakeTxBx.Text; } 
            catch { ErrorMessage("Make"); }

            try { _packingObject.model = ModelTxBx.Text; } 
            catch { ErrorMessage("Model"); }

            try { _packingObject.photoFileName = PhotoTxBx.Text; } 
            catch { ErrorMessage("Photo directory"); }

            try { _packingObject.registrationNumber = RegNoTxBx.Text; } 
            catch { return ErrorMessage("Registry Number"); }

            try { _packingObject.year = int.Parse(YearTxBx.Text); } 
            catch { return ErrorMessage("Year"); }

            try { _packingObject.price = int.Parse(PriceTxBx.Text); } 
            catch { return ErrorMessage("Price"); }

            return _packingObject;
        }

        CarObject ErrorMessage(string _where) //if one of the above meantioned feilds fails return an error
        {
            MessageBox.Show($"The Entered {_where} is incorrectly formatted. Please re-enter the data", "Input Error");
            return null;
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e) //if a car is being removed, propmts to confirm sale, if so, copy to the sold car json file in MyDocuments of current user
        {
            MessageBoxResult soldResult = MessageBox.Show($"Would you like to log the sale of the {YearTxBx.Text} {MakeTxBx.Text} {ModelTxBx.Text}?", "Sale logging", MessageBoxButton.YesNoCancel);
            if (soldResult == MessageBoxResult.Yes)
            {
                try
                {
                    string location = json.OpenJSON();

                    if (location != null)
                    {
                        List<CarObject> tempList = new List<CarObject>();
                        tempList.AddRange(json.DeSerialiseJsonData(location).ToList());

                        tempList.Add(((App)Application.Current).cars.GetCar(originalBay));
                        json.UpdateJsonFile(location, tempList.ToArray());

                        if (originalBay >= 0) { ((App)Application.Current).cars.SetCar(null, originalBay); }

                        this.Close();
                    }
                    
                }
                catch
                {
                    MessageBox.Show($"There was an error editing the sold JSON", "Output Error");
                    return;
                }
            }
            else if (soldResult == MessageBoxResult.No)
            {
                if (originalBay >= 0) { ((App)Application.Current).cars.SetCar(null, originalBay); }
                this.Close();
            }
        }

        private void CancleBtn_Click(object sender, RoutedEventArgs e) //closes the window and logs that for use in the other window
        {
            this.Close();
        }

        private void PhotoTxBx_TextChanged(object sender, TextChangedEventArgs e) //for internet images or manually entered links
        {
            try{Imageimg.Source = new BitmapImage(new Uri(PhotoTxBx.Text));}
            catch { }
        }
    }
}