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
        /// Update the array with new array data of the same length after testing its validity
        /// </summary>
        /// <param name="_array">New array to set to cars</param>
        public bool SetArray(CarObject[] _array)
        {
            CarObject[] tempArray = new CarObject[cars.Length];

            try { Array.Copy(_array, tempArray, _array.Length); }
            catch { return false; }

            Array.Clear(cars, 0, cars.Length);
            Array.Copy(tempArray, cars, tempArray.Length);
            return true;
        }


        /// <returns>the cars Array</returns>
        public CarObject[] GetArray()
        {
            return cars;
        }

        /// <summary>
        /// provide the base and/or top of rearching the car data
        /// </summary>
        /// <param name="_basePeramiter">base data</param>
        /// <param name="_topPeramiter">top data</param>
        /// <returns> bool if successful</returns>
        public bool SetSearch(CarObject _basePeramiter, CarObject _topPeramiter)
        {
            try
            {
                basePeramiter = _basePeramiter;
                topPeramiter = _topPeramiter;
                return true;
            }
            catch
            {
                return false;
            }
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
                if (!(topPeramiter.currentBay < 0 || (_car.currentBay <= topPeramiter.currentBay)))
                {
                    return false;
                }


                //the same as the car's price range end variable or is equal to "Any"
                if (!(topPeramiter.price < 0 || (_car.price <= topPeramiter.price)))
                {
                    return false;
                }


                //the same as the car's year range end variable or is equal to "Any"
                if (!(topPeramiter.year < 0 || (_car.year <= topPeramiter.year)))
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
        public List<CarObject> ParseArray()
        {
            List<CarObject> carsView = new List<CarObject>();
            usedBays = 0;
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i] != null)
                {
                    usedBays += 1;
                    cars[i].ParkSet(i + 1);

                    if (TestPerams(cars[i]))
                    {
                        carsView.Add(cars[i]);
                    }
                }
            }
            return carsView;
        }


        /// <summary>
        /// Sets car data object to a posision in the array if it falls in the bounds
        /// </summary>
        /// <param name="_car">New car data</param>
        /// <param name="_carPosition">Where in the array the car is going to go</param>
        /// <returns> bool if successful</returns>
        public bool SetCar(CarObject _car, int _carPosition)
        {
            if(0 <= _carPosition && _carPosition <= cars.Length)
            {
                cars[_carPosition] = _car;
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Sets car data object to the target position and carPosition to null
        /// </summary>
        /// <param name="_car">New car data</param>
        /// <param name="_carPosition">Where in the array the car was</param>
        /// <param name="_targetPosition">Where in the array the car is going to go</param>
        public bool SetCar(CarObject _car, int _carPosition, int _targetPosition)
        {
            if ((0 <= _carPosition && _carPosition < cars.Length) && (0 <= _targetPosition && _targetPosition < cars.Length))
            {
                cars[_carPosition] = null;
                cars[_targetPosition] = _car;
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Retreives a car at a place in the array
        /// </summary>
        /// <param name="_carPosition">The position of the requested car</param>
        /// <returns>the car or nbull if the retrieval wasn't successful</returns>
        public CarObject GetCar(int _carPosition)
        {
            if (0 <= _carPosition && _carPosition < cars.Length)
            {
                return cars[_carPosition];
            }
            else
            {
                return null;
            }
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

        /// <summary>
        /// Searches through a sorted array with a binary search
        /// </summary>
        /// <param name="array">The array to be searched through</param>
        /// <param name="bottomOfTheArray">Where the array search starts</param>
        /// <param name="topOfTheArray">Where the array search ends</param>
        /// <param name="targetValue">The end goal of the binary search</param>
        /// <returns>the index of targetValue if found or -1 if its not</returns>
        public int binarySearch(int[] array, int bottomOfTheArray, int topOfTheArray, int targetValue)
        {
            if (topOfTheArray >= bottomOfTheArray) // if the top and bottom of the array don't cross
            {
                int middleOfTheArray = bottomOfTheArray + (topOfTheArray - bottomOfTheArray) / 2;

                //If the target is at the middle
                if (array[middleOfTheArray] == targetValue)
                {
                    return middleOfTheArray;
                }

                // if the target is smaller than the middle, recurse the top half of the array through
                if (array[middleOfTheArray] > targetValue)
                {
                    return binarySearch(array, bottomOfTheArray, middleOfTheArray - 1, targetValue);
                }

                // if the target is larger than the middle, recurse the top bottom of the array through
                if (array[middleOfTheArray] < targetValue)
                {
                    return binarySearch(array, middleOfTheArray + 1, topOfTheArray, targetValue);
                }
            }

            //else (the object isn't found)
            return -1;
        }
    }
}
