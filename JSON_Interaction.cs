using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
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
    public class JSON_Interaction
    {
        int carsLength = ((App)Application.Current).cars.GetLength();
        public string fileLocation; //location of JSON

        /// <summary>
        /// Opens an Open File Dialog window and sets fileLocation to the output
        /// </summary>
        /// <returns>cancel bool</returns>
        public bool OpenJSON()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON file (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                ((App)Application.Current).cars.SetArray(new CarObject[carsLength]);
                fileLocation = openFileDialog.FileName;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Opens an Save File Dialog window and sets fileLocation to the output
        /// </summary>
        /// <returns>cancel bool</returns>
        public bool NewJSON() // opens a new file window with a filter for JSONS
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileLocation = saveFileDialog.FileName;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Converts the array back into a JSON file and writes it to the file solation in fileLocation
        /// </summary>
        public void UpdateJsonFile(string _fileLocation = "", CarObject[] _array = null) // compresses the object list into a JSON file, should be fired any time an object changes
        {
            if(_fileLocation == "") { _fileLocation = fileLocation; }
            if(_array == null) { _array = ((App)Application.Current).cars.GetArray(); }
            using (StreamWriter file = File.CreateText(_fileLocation))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _array);
            }
        }


        /// <summary>
        /// fills the car array with the JSON referenced in fileLocation
        /// </summary>
        /// <returns>pass bool</returns>
        public bool DeSerialiseJsonData(string _fileLocation = "") 
        {
            if (_fileLocation == "") { _fileLocation = fileLocation; }
            try
            {
                string jsonStorage = "";

                using (StreamReader reader = new StreamReader(new FileStream(_fileLocation, FileMode.Open)))
                {
                    string line;
                    // Read line by line  
                    while ((line = reader.ReadLine()) != null)
                    {
                        jsonStorage += line;
                    }
                }

                CarObject[] tempUnpackCar = JsonConvert.DeserializeObject<List<CarObject>>(jsonStorage).ToArray();

                if (tempUnpackCar.Length > carsLength)
                {
                    MessageBox.Show($"The selected JSON has more than {carsLength} entries. only the first {carsLength} will be shown", "Too many entries in JSON");
                }

                ((App)Application.Current).cars.SetArray(tempUnpackCar);
            }
            catch (Exception)
            {
                MessageBox.Show($"The selected JSON is either incorrectly formatted or corrupted", "JSON error");
                return false;
            }

            return true;
        }
    }
}
