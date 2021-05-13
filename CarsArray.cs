using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Storage
{
    public class CarsArray
    {
        private int usedBays = 0; // The amount of bays being used
        private CarObject[] cars; // JSON collection array for use in displaying and modifying the JSON data
        private CarObject basePeramiter = null; // the base of the search range
        private CarObject topPeramiter = null; // the top of the search range

        /// <summary>
        /// Data controls for JSON files imported to arrays and their usage with DataGrids
        /// </summary>
        /// <param name="_carLength">THe length of the array</param>
        public CarsArray(int _carLength)
        {
            cars = new CarObject[_carLength];
        }


        /// <summary>
        /// Update the array with new array data of the same length
        /// </summary>
        /// <param name="_array">New array to set to cars</param>
        public void SetArray(CarObject[] _array)
        {
            Array.Copy(_array, cars, cars.Length);
        }


        /// <returns>the cars Array</returns>
        public CarObject[] GetArray()
        {
            return cars;
        }

        public void SetSearch(CarObject _basePeramiter, CarObject _topPeramiter)
        {
            basePeramiter = _basePeramiter;
            topPeramiter = _topPeramiter;
        }

        public bool TestPerams(CarObject _car)
        {
            if (basePeramiter != null)
            {
                //check to see if the ___ range start entered is not...
                //the same as the car's Make variable or is equal to "Any"
                if (!(_car.make == basePeramiter.make || basePeramiter.make == "Any"))
                {
                    return false;
                }


                //the same as the car's Model variable or is equal to "Any"
                if (!(_car.model == basePeramiter.model || basePeramiter.model == "Any"))
                {
                    return false;
                }


                //the same as the car's parking bay range start variable or is equal to "Any"
                if (!(basePeramiter.currentBay < 0 || (_car.currentBay >= basePeramiter.currentBay)))
                {
                    return false;
                }


                //the same as the car's regestry number range end variable or is equal to "Any"
                if (!(basePeramiter.registrationNumber == "Any" || (_car.registrationNumber == basePeramiter.registrationNumber)))
                {
                    return false;
                }


                //the same as the car's price range start variable or is equal to "Any"
                if (!(basePeramiter.price < 0 || (_car.price >= basePeramiter.price)))
                {
                    return false;
                }


                //the same as the car's year range start variable or is equal to "Any"
                if (!(basePeramiter.year < 0 || (_car.year >= basePeramiter.year)))
                {
                    return false;
                }
            }

            if (topPeramiter != null)
            {
                //the same as the car's parking bay range start variable or is equal to "Any"
                if (!(topPeramiter.currentBay < 0 || (_car.currentBay >= topPeramiter.currentBay)))
                {
                    return false;
                }


                //the same as the car's price range end variable or is equal to "Any"
                if (!(topPeramiter.price < 0 || (_car.price >= topPeramiter.price)))
                {
                    return false;
                }


                //the same as the car's year range end variable or is equal to "Any"
                if (!(topPeramiter.year < 0 || (_car.year >= topPeramiter.year)))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Converts the array into a list
        /// </summary>
        /// <returns>the amount of items moved from the array into the list</returns>
        public List<CarObject> ParseArray(CarObject[] _input = null)
        {
            List<CarObject> carsView = new List<CarObject>();
            usedBays = 0;
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i] != null && TestPerams(cars[i]))
                {
                    usedBays += 1;
                    cars[i].ParkSet(i + 1);
                    carsView.Add(cars[i]);
                }
            }
            return carsView;
        }


        /// <summary>
        /// Sets car data object to a posision in the array
        /// </summary>
        /// <param name="_car">New car data</param>
        /// <param name="_carPosition">Where in the array the car is going to go</param>
        public void SetCar(CarObject _car, int _carPosition)
        {
            cars[_carPosition] = _car;
        }


        /// <summary>
        /// Sets car data object to the target position and carPosition to null
        /// </summary>
        /// <param name="_car">New car data</param>
        /// <param name="_carPosition">Where in the array the car was</param>
        /// <param name="_targetPosition">Where in the array the car is going to go</param>
        public void SetCar(CarObject _car, int _carPosition, int _targetPosition)
        {
            cars[_carPosition] = null;
            cars[_targetPosition] = _car;
        }


        /// <summary>
        /// Retreives a car at a place in the array
        /// </summary>
        /// <param name="_carPosition">The position of the requested car</param>
        /// <returns></returns>
        public CarObject GetCar(int _carPosition)
        {
            return cars[_carPosition];
        }


        /// <returns>the current size of the array</returns>
        public int GetLength()
        {
            return cars.Length;
        }


        /// <returns>the bays in use</returns>
        public int GetUsedLength()
        {
            ParseArray();
            return usedBays;
        }
    }
}
