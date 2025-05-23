using System;
using System.Collections.Generic;
using ArribaEats.Models;
using ArribaEats.Utils;

namespace ArribaEats.UI.Settings
{
    /// <summary>
    /// Provides reusable input validation and prompts.
    /// </summary>
    class InputChecks
    {
        public InputChecks() { }

        #region Basic Input
        /// <summary>
        /// Outputs a string to the console.
        /// </summary>
        /// <param name="input"></param>
        public void Output(string input)
        {
            Console.WriteLine(input);
        }
        /// <summary>
        /// Prompts the user for input and returns it as a string.
        /// </summary>
        /// <returns></returns>
        public string GetInput()
        {
            return Console.ReadLine() ?? string.Empty;
        }

        #endregion

        #region General Validation
        /// <summary>
        /// Prompts the user for a valid input and validates it using a custom validation function.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="validationFunc"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public string GetValidInput(string prompt, Func<string, bool> validationFunc, string error)
        {
            string input;
            do
            {
                Output(prompt);
                input = GetInput();
                if (!validationFunc(input))
                {
                    Output(error);
                }
            } while (!validationFunc(input));

            return input;
        }
        /// <summary>
        /// Prompts the user for a valid integer input and validates it using a custom validation function.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="validationFunc"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public int GetValidIntInput(string prompt, Func<int, bool> validationFunc, string error)
        {
            int input;
            do
            {
                Output(prompt);
                string userInput = GetInput();
                if (!int.TryParse(userInput, out input) || !validationFunc(input))
                {
                    Output(error);
                }
            } while (!validationFunc(input));

            return input;
        }

        #endregion

        #region Location Input
        /// <summary>
        /// Prompts the user for a valid location input and validates it.
        /// </summary>
        /// <returns></returns>
        public Location GetValidLocation()
        {
            while (true)
            {
                Console.WriteLine("Please enter your location (in the form of X,Y):");
                string input = Console.ReadLine()?.Trim();

                var parts = input.Split(',');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int x) &&
                    int.TryParse(parts[1], out int y))
                {
                    return new Location { X = x, Y = y };
                }

                Console.WriteLine("Invalid location.");
            }
        }
        /// <summary>
        /// Prompts the user for a menu choice between min and max values.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int GetMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.WriteLine($"Please enter a choice between {min} and {max}: ");
                string input = Console.ReadLine()?.Trim();
                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
            }
        }


        #endregion

        #region Price Input
        /// <summary>
        /// Prompts the user for a valid price input and validates it.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public decimal GetValidPrice(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine()?.Trim();

                if (input.Contains('$'))
                {
                    Console.WriteLine(errorMessage);
                    continue;
                }

                if (InputValidator.ValidateItemPrice(input, out decimal price))
                {
                    return price;
                }

                Console.WriteLine(errorMessage);
            }
        }

        #endregion

        #region Password Input
        /// <summary>
        /// Prompts the user for a valid password and confirms it.
        /// </summary>
        /// <returns></returns>
        public string GetValidPassword()
        {
            while (true)
            {
                Console.WriteLine("Your password must: ");
                Console.WriteLine("- be at least 8 characters long");
                Console.WriteLine("- contain a number");
                Console.WriteLine("- contain a lowercase letter");
                Console.WriteLine("- contain an uppercase letter");
                Console.WriteLine("Please enter a password: ");

                string password = Console.ReadLine()?.Trim();

                if (!InputValidator.ValidatePassword(password))
                {
                    Console.WriteLine("Invalid password.");
                    continue;
                }

                Console.WriteLine("Please confirm your password: ");
                string confirm = Console.ReadLine()?.Trim();

                if (password == confirm)
                {
                    return password;
                }

                Console.WriteLine("Passwords do not match.");
            }
        }
        #endregion
    }
}
