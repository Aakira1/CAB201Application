using System;
using System.Linq;
using System.Text.RegularExpressions;
using ArribaEats.Models;

namespace ArribaEats.Utils
{
    /// <summary>
    /// Utility class for validating user input
    /// </summary>
    public static class InputValidator
    {
        #region Name Validation
        /// <summary>
        /// Validates the name input.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return Regex.IsMatch(name, @"^[a-zA-Z]+[a-zA-Z\s'-]*$");
        }

        #endregion

        #region Age Validation

        public static bool ValidateAge(int age)
        {
            return age >= 18 && age <= 100;
        }

        #endregion

        #region Mobile Validation
        /// <summary>
        /// Validates the mobile number input.
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool ValidateMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^0\d{9}$");
        }

        #endregion

        #region Email Validation
        /// <summary>
        /// Validates the email input.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@]+@[^@]+$");
        }

        #endregion

        #region Password Validation
        /// <summary>
        /// Validates the password input.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password)
        {
            return password != null &&
                   password.Length >= 8 &&
                   password.Any(char.IsDigit) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsUpper) &&
                   !password.Contains(" ");
        }

        #endregion

        #region Licence Plate Validation
        /// <summary>
        /// Validates the licence plate input.
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public static bool ValidateLicencePlate(string plate)
        {
            if (string.IsNullOrWhiteSpace(plate) || plate.Length > 8)
                return false;

            return Regex.IsMatch(plate, @"^[A-Z0-9 ]+$") && plate.Trim().Length > 0;
        }

        #endregion

        #region Restaurant Name Validation
        /// <summary>
        /// Validates the restaurant name input.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateRestaurantName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
        /// <summary>
        /// Selects the food style for the restaurant.
        /// </summary>
        /// <returns></returns>
        public static FoodStyle SelectFoodStyle()
        {
            Console.WriteLine("Please select your restaurant's style:");
            Console.WriteLine("1: Italian");
            Console.WriteLine("2: French");
            Console.WriteLine("3: Chinese");
            Console.WriteLine("4: Japanese");
            Console.WriteLine("5: American");
            Console.WriteLine("6: Australian");
            Console.WriteLine("Please enter a choice between 1 and 6:\n");

            while (true)
            {
                string input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 6)
                {
                    return (FoodStyle)choice;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                }
            }
        }
        #endregion

        #region Location Validation
        /// <summary>
        /// Validates the location input in the form of "X,Y" where X and Y are integers.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="parsedLocation"></param>
        /// <returns></returns>
        public static bool ValidateLocation(string location, out Location parsedLocation)
        {
            parsedLocation = new Location();

            if (string.IsNullOrWhiteSpace(location))
                return false;

            location = location.Trim();
            string[] parts = location.Split(',');
            if (parts.Length != 2)
                return false;

            string xPart = parts[0].Trim();
            string yPart = parts[1].Trim();

            if (!int.TryParse(xPart, out int x) || !int.TryParse(yPart, out int y))
                return false;

            parsedLocation = new Location(x, y);
            return true;
        }

        #endregion

        #region Price Validation

        public static bool ValidateItemPrice(string input, out decimal price)
        {
            return decimal.TryParse(input, out price) && price > 0 && price <= 999.99m;
        }

        #endregion
    }
}
