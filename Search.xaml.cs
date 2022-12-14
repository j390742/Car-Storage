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
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public bool isSearching = false;
        public CarObject basePeramiter = null;
        public CarObject topPeramiter = null;

        public Search()
        {
            InitializeComponent();
            FillOptions(((App)Application.Current).cars.GetArray());
        }


        //goes through the array and adds all unique items to their respective areas
        void FillOptions(CarObject[] _carArray)
        {
            foreach (CarObject car in _carArray)
            {
                if (car != null)
                {
                    if (!MakeCbBx.Items.Contains(car.make))
                    {
                        MakeCbBx.Items.Add(car.make);
                    }

                    if (!ModelCbBx.Items.Contains(car.model))
                    {
                        ModelCbBx.Items.Add(car.model);
                    }

                    if (!CurrentBayMainCbBx.Items.Contains(car.currentBay))
                    {
                        CurrentBayMainCbBx.Items.Add(car.currentBay);
                        CurrentBayRangeCbBx.Items.Add(car.currentBay);
                    }

                    if (!RegoNoCbBx.Items.Contains(car.registrationNumber))
                    {
                        RegoNoCbBx.Items.Add(car.registrationNumber);
                    }

                    if (!PriceMainCbBx.Items.Contains(car.price))
                    {
                        PriceMainCbBx.Items.Add(car.price);
                        PriceRangeCbBx.Items.Add(car.price);
                    }

                    if (!YearMainCbBx.Items.Contains(car.year))
                    {
                        YearMainCbBx.Items.Add(car.year);
                        YearRangeCbBx.Items.Add(car.year);
                    }
                }
            }
        }

        void PackObjects() //test if the feilds have been used then enter the data into the search objects
        {
            bool testTop = TestTop();
            bool testBase = TestBase();

            if (testBase)
            {
                basePeramiter = new CarObject();
                basePeramiter.make = MakeCbBx.Text;
                basePeramiter.model = ModelCbBx.Text;
                basePeramiter.registrationNumber = RegoNoCbBx.Text;

                if (CurrentBayMainCbBx.SelectedIndex > 0)
                { basePeramiter.currentBay = int.Parse(CurrentBayMainCbBx.Text); }
                else { basePeramiter.currentBay = -1; }

                if (PriceMainCbBx.SelectedIndex > 0)
                { basePeramiter.price = int.Parse(PriceMainCbBx.Text); }
                else { basePeramiter.price = -1; }

                if (YearMainCbBx.SelectedIndex > 0)
                { basePeramiter.year = int.Parse(YearMainCbBx.Text); }
                else { basePeramiter.year = -1; }
            }

            if (testTop)
            {
                topPeramiter = new CarObject();
                topPeramiter.make = MakeCbBx.Text;
                topPeramiter.model = ModelCbBx.Text;
                topPeramiter.registrationNumber = RegoNoCbBx.Text;

                if (CurrentBayRangeCbBx.SelectedIndex > 0)
                { topPeramiter.currentBay = int.Parse(CurrentBayRangeCbBx.Text); }
                else { topPeramiter.currentBay = -1; }

                if (PriceRangeCbBx.SelectedIndex > 0)
                { topPeramiter.price = int.Parse(PriceRangeCbBx.Text); }
                else { topPeramiter.price = -1; }

                if (YearRangeCbBx.SelectedIndex > 0)
                { topPeramiter.year = int.Parse(YearRangeCbBx.Text); }
                else { topPeramiter.year = -1; }
            }
            else
            {
                topPeramiter = basePeramiter;
            }

            if(!testTop && !testBase && CurrentBayMainCbBx.SelectedIndex > 0) //if only the current bay is selected send back that item for editing immideatly
            {
                isSearching = false;
                basePeramiter = ((App)Application.Current).cars.GetCar(int.Parse(CurrentBayMainCbBx.Text)-1);
            }
        }

        bool TestBase() // test the base data groups usage
        {
            if (MakeCbBx.SelectedIndex > 0) { return true; }
            if (ModelCbBx.SelectedIndex > 0) { return true; }
            if (RegoNoCbBx.SelectedIndex > 0) { return true; }
            if (PriceMainCbBx.SelectedIndex > 0) { return true; }
            if (YearMainCbBx.SelectedIndex > 0) { return true; }
            return false;
        }

        bool TestTop() //test the top data groups
        {
            if (CurrentBayRangeCbBx.SelectedIndex > 0) { return true; }
            if (PriceRangeCbBx.SelectedIndex > 0) { return true; }
            if (YearRangeCbBx.SelectedIndex > 0) { return true; }
            return false;
        }

        //Below here is the same thing 4 times, when a box is checked/unchecked, use that state to decide the interactablity for it's respective range box and clear it if it's unchecked

        private void CurrentBayRangeSlct_Checked(object sender, RoutedEventArgs e)
        {
            CurrentBayRangeCbBx.IsEnabled = (bool)CurrentBayRangeSlct.IsChecked;
        }

        private void PriceRangeSlct_Checked(object sender, RoutedEventArgs e)
        {
            PriceRangeCbBx.IsEnabled = (bool)PriceRangeSlct.IsChecked;
        }

        private void YearRangeSlct_Checked(object sender, RoutedEventArgs e)
        {
            YearRangeCbBx.IsEnabled = (bool)YearRangeSlct.IsChecked;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            isSearching = false;
            this.Close();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            PackObjects();
            ((App)Application.Current).cars.SetSearch(basePeramiter, topPeramiter);
            isSearching = basePeramiter != null && topPeramiter != null;
            this.Close();
        }
    }
}