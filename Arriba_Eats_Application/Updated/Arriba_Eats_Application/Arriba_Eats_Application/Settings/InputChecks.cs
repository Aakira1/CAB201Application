using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArribaEats.Models;
using ArribaEats.Utils;

namespace ArribaEats.UI.Settings
{
    class InputChecks
    {
        public InputChecks()
        {
            // Constructor logic here
        }

        public void Output(string input)
        {
            Console.WriteLine(input);
        }

        public string GetInput()
        {
            string input = Console.ReadLine() ?? string.Empty;
            return input;
        }

        public string GetValidInput(string prompt, Func<string, bool> validationFunc, string Error)
        {
            string input;
            do
            {
                Output(prompt);
                input = GetInput();
                if (!validationFunc(input))
                {
                    Output(Error);
                }
            } while (!validationFunc(input));
            return input;
        }
        public int GetValidIntInput(string prompt, Func<int, bool> validationFunc, string Error)
        {
            int input;
            do
            {
                Output(prompt);
                string userInput = GetInput();
                if (!int.TryParse(userInput, out input) || !validationFunc(input))
                {
                    Output(Error);
                }
            } while (!validationFunc(input));
            return input;
        }

        /// <summary>
        /// Gets a valid location from the user
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="errorMessage">The error message to display</param>
        /// <returns>The valid location</returns>
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
                else
                {
                    Console.WriteLine("Invalid location.");
                }
            }
        }

        /// <summary>
        /// Gets a valid price from the user
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="errorMessage">The error message to display</param>
        /// <returns>The valid price</returns>
        public decimal GetValidPrice(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine()?.Trim();

                // Reject if it contains a dollar sign
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


        public string GetValidPassword()
        {
            while (true)
            {
                // Always show password rules before entering
                Console.WriteLine("Your password must: ");
                Console.WriteLine("- be at least 8 characters long ");
                Console.WriteLine("- contain a number ");
                Console.WriteLine("- contain a lowercase letter ");
                Console.WriteLine("- contain an uppercase letter ");
                Console.WriteLine("Please enter a password: ");

                string password = Console.ReadLine()?.Trim();

                if (!InputValidator.ValidatePassword(password))
                {
                    Console.WriteLine("Invalid password.");
                    continue; // Loop again — rules will reprint
                }

                Console.WriteLine("Please confirm your password: ");
                string confirm = Console.ReadLine()?.Trim();

                if (password == confirm)
                {
                    return password;
                }

                Console.WriteLine("Passwords do not match.");
                // Loop again — rules will reprint
            }
        }

    }
}
