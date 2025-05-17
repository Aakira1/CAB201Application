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
        /// <summary>
        /// Validates a name
        /// </summary>
        /// <param name="name">The name to validate</param>
        /// <returns>True if the name is valid, false otherwise</returns>
        public static bool ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Must consist of at least one letter and only letters, spaces, apostrophes and hyphens
            return Regex.IsMatch(name, @"^[a-zA-Z]+[a-zA-Z\s'-]*$");
        }

        /// <summary>
        /// Validates an age
        /// </summary>
        /// <param name="age">The age to validate</param>
        /// <returns>True if the age is valid, false otherwise</returns>
        public static bool ValidateAge(int age)
        {
            // An integer value between 18 -100 inclusive
            return age >= 18 && age <= 100;
        }

        /// <summary>
        /// Validates a mobile number
        /// </summary>
        /// <param name="mobile">The mobile number to validate</param>
        /// <returns>True if the mobile number is valid, false otherwise</returns>
        public static bool ValidateMobile(string mobile)
        {
            // Must consist of only numbers, be exactly 10 characters long, and have a leading zero
            return Regex.IsMatch(mobile, @"^0\d{9}$");
        }

        /// <summary>
        /// Validates an email address
        /// </summary>
        /// <param name="email">The email address to validate</param>
        /// <returns>True if the email address is valid, false otherwise</returns>
        public static bool ValidateEmail(string email)
        {
            // Must include exactly one @ character and have at least one other character on either side
            return Regex.IsMatch(email, @"^[^@]+@[^@]+$");
        }

        /// <summary>
        /// Validates a password
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if the password is valid, false otherwise</returns>
        public static bool ValidatePassword(string password)
        {
			// Basic requirements (unchanged from your original)
			bool meetsBasicRequirements = password.Length >= 8 &&
										  password.Any(char.IsDigit) &&
										  password.Any(char.IsLower) &&
										  password.Any(char.IsUpper) &&
										  password.Any(c => "!@#$%^&*()_+-=[]{}|;':\",.<>?/`~".Contains(c));

			return meetsBasicRequirements;
		}

        /// <summary>
        /// Validates a licence plate
        /// </summary>
        /// <param name="plate">The licence plate to validate</param>
        /// <returns>True if the licence plate is valid, false otherwise</returns>
        public static bool ValidateLicencePlate(string plate)
        {
            // Must be between 1 and 8 characters long, may only contain uppercase letters, numbers and spaces
            // and may not consist entirely of spaces
            if (string.IsNullOrWhiteSpace(plate) || plate.Length > 8)
                return false;

            return Regex.IsMatch(plate, @"^[A-Z0-9 ]+$") && plate.Trim().Length > 0;
        }

        /// <summary>
        /// Validates a restaurant name
        /// </summary>
        /// <param name="name">The restaurant name to validate</param>
        /// <returns>True if the restaurant name is valid, false otherwise</returns>
        public static bool ValidateRestaurantName(string name)
        {
            // Must consist of at least one non-whitespace character
            return !string.IsNullOrWhiteSpace(name);
        }

        /// <summary>
        /// Validates a location
        /// </summary>
        /// <param name="location">The location string to validate</param>
        /// <param name="parsedLocation">The parsed location (out parameter)</param>
        /// <returns>True if the location is valid, false otherwise</returns>
        public static bool ValidateLocation(string location, out Location parsedLocation)
        {
            parsedLocation = new Location();

            if (string.IsNullOrWhiteSpace(location))
                return false;

            // Must be of the format X,Y where X and Y are both integer values
            string[] parts = location.Split(',');
            if (parts.Length != 2)
                return false;

            if (!int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
                return false;

            parsedLocation = new Location(x, y);
            return true;
        }

        /// <summary>
        /// Validates an item price
        /// </summary>
        /// <param name="price">The price string to validate</param>
        /// <param name="parsedPrice">The parsed price (out parameter)</param>
        /// <returns>True if the price is valid, false otherwise</returns>
        public static bool ValidateItemPrice(string price, out decimal parsedPrice)
        {
            parsedPrice = 0;

            // Check if price is null or empty
            if (string.IsNullOrEmpty(price))
                return false;

            // Must be between $0.00 and $999.99
            if (!price.StartsWith("$"))
                return false;

            string priceStr = price.Substring(1);
            if (!decimal.TryParse(priceStr, out parsedPrice))
                return false;

            return parsedPrice >= 0 && parsedPrice <= 999.99m;
        }
    }
}