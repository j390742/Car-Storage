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
        public string locationOfJSON; //location of JSON
        int carsLength = ((App)Application.Current).cars.GetLength();

        /// <summary>
        /// Opens an Open File Dialog window and sets fileLocation to the output
        /// </summary>
        /// <returns>cancel bool</returns>
        public string OpenJSON()
        {
            string fileLocation = null; //location of JSON

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON file (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                fileLocation = openFileDialog.FileName;
            }
            return fileLocation;
        }


        /// <summary>
        /// Opens an Save File Dialog window and sets fileLocation to the output
        /// </summary>
        /// <returns>cancel bool</returns>
        public string NewJSON() // opens a new file window with a filter for JSONS
        {
            string fileLocation = null; //location of JSON

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileLocation = saveFileDialog.FileName;
            }
            return fileLocation;
        }


        /// <summary>
        /// Converts the array back into a JSON file and writes it to the file solation in fileLocation
        /// </summary>
        public void UpdateJsonFile(string _fileLocation = "", CarObject[] _array = null) // compresses the object list into a JSON file, should be fired any time an object changes
        {
            if(_fileLocation == "") { _fileLocation = locationOfJSON; }
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
        public CarObject[] DeSerialiseJsonData()
        {
            CarObject[] tempUnpackCar = null;

            try
            {
                string jsonStorage = "";

                using (StreamReader reader = new StreamReader(new FileStream(locationOfJSON, FileMode.Open)))
                {
                    string line;
                    // Read line by line  
                    while ((line = reader.ReadLine()) != null)
                    {
                        jsonStorage += line;
                    }
                }

                if (jsonStorage != "")
                {
                    tempUnpackCar = JsonConvert.DeserializeObject<List<CarObject>>(jsonStorage).ToArray();
                }

                if (tempUnpackCar.Length > carsLength)
                {
                    MessageBox.Show($"The selected JSON has more than {carsLength} entries. only the first {carsLength} will be shown", "Too many entries in JSON");
                }
            }
            catch
            {
                MessageBox.Show($"The selected JSON is either incorrectly formatted or corrupted", "JSON error");
                return null;
            }

            return tempUnpackCar;
        }


        /// <summary>
        /// fills the car array with the JSON referenced in fileLocation (for use with SOLD json primarily)
        /// </summary>
        /// <returns>pass bool</returns>
        public CarObject[] DeSerialiseJsonData(string _fileLocation = "") 
        {
            CarObject[] tempUnpackCar = null;

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

                if(jsonStorage != "")
                {
                    tempUnpackCar = JsonConvert.DeserializeObject<List<CarObject>>(jsonStorage).ToArray();
                }
            }
            catch 
            {
                MessageBox.Show($"The selected JSON is either incorrectly formatted or corrupted", "JSON error");
                return null;
            }

            return tempUnpackCar;
        }


        /// <summary>
        /// Checks to see if a file and it's directory exists
        /// </summary>
        /// <param name="_folder">The folder location</param>
        /// <param name="_file">the file in the folder location (include the \ before the file)</param>
        /// <returns>if the file exists: Bool</returns>
        public bool CheckFile(string _location)
        {
            return File.Exists(_location);
        }
    }
}