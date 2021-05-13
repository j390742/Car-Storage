using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Storage
{
    public class CarObject
    {
        public CarObject(int _currentBay, string _registrationNumber, string _make, string _model, int _year, int _price, string _photoFileName)
        {
            this.currentBay = _currentBay;
            this.registrationNumber = _registrationNumber;
            this.make = _make;
            this.model = _model;
            this.year = _year;
            this.price = _price;
            this.photoFileName = _photoFileName;
        }

        public CarObject() // for use in new objects
        {
            this.currentBay = -1;
            this.registrationNumber = "";
            this.make = "";
            this.model = "";
            this.year = 0;
            this.price = 0;
            this.photoFileName = "";
        }

        public void ParkSet(int _currentBay) // for use in the visual tool, shows what bay it is in
        {
            this.currentBay = _currentBay;
        }

        public int currentBay { get; set; }

        public string registrationNumber { get; set; }

        public string make { get; set; }

        public string model { get; set; }

        public int year { get; set; }

        public int price { get; set; }

        public string photoFileName { get; set; }

        public override string ToString()
        {
            return ($"{currentBay}, {registrationNumber}, {make}, {model}, {year}, {price}, {photoFileName}");
        }
    }
}
