using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

// Personal note
/*
	
 */


namespace Arriba_Eats_App.Arriba_Eats_App.UI
{
	static class MainMenuUI
	{
		public static void MainMenu()
		{
			MainMenuOptions();
		}

		#region Handles Main Menu Options and UI functionality

		/// <summary>
		///  Runs the main menu options for the user to select from.
		/// </summary>
		public static void MainMenuOptions()
		{
			// set the menu to be active until the user selects to exit.
			bool IsActive = true;
			
			// print message to screen so a selection is visible for the user
			Console.Clear();
			Console.WriteLine(WelcomeMessage()+$"\n");
			Console.WriteLine("From the following selection, using your input keys you can navigate the menu:");
			Console.WriteLine("1. Customer Login");
			Console.WriteLine("2. Restaruant Staff Login");
			Console.WriteLine("3. Delivery Staff Login");
			Console.WriteLine("4. Register New Account");
			Console.WriteLine("5. Exit");

			// set variable input to be picked up by the switch case.
			string input = GetUserInput();
			HandleEmptyInputs(input);

			switch (input)
			{
				case "1":
					Console.WriteLine("Customer Successfully Logged In");
					return;
				case "2":
					Console.WriteLine("Restaurant Staff Successfully Logged In");
					return;
				case "3":
					Console.WriteLine("Delivery Staff Successfully Logged In");
					return;
				case "4":
					Console.WriteLine("Register New Account");
					return;
				case "5":
					Console.WriteLine("Exiting the program...");
					IsActive = false;
					break;
			}

		}

		/// <summary>
		/// Displays the welcome message to the user.
		/// </summary>
		/// <returns>
		/// returns welcome message to the user.
		/// </returns>
		public static string WelcomeMessage()
		{
			return "Welcome To Arriba Eats!";
		}

		/// <summary>
		/// Displays the welcome message to the user.
		/// </summary>
		/// <returns>
		/// A string containing the formatted welcome message. 
		/// </returns>
		static string GetUserInput()
		{
			Console.WriteLine("Please select an option from the menu above:");
			string userInput = Console.ReadLine()!;
			return userInput;
		}
		#endregion

		#region Debugging Functions & Error Handling Functions

		/// <summary>
		/// Displays error messages to the user.
		/// </summary>
		/// <param name="message">
		///	An input return
		/// </param>
		static void DisplayErrorMessage(string message)
		{
			Console.WriteLine(message);
		}


		/// <summary>
		/// Handles empty inputs by displaying an error message and returning to the main menu.
		/// </summary>
		/// <param name="Input">
		/// a string that handles user input to determine if it is empty.
		/// </param>
		static void HandleEmptyInputs(string Input)
		{
			string ErrorMessage = "";
			if (Input is null || Input == string.Empty)
			{
				ErrorMessage = "Error: Input cannot be empty. Please try again.";
				DisplayErrorMessage(ErrorMessage);
				Console.WriteLine("Press any key to continue...");
				Console.ReadKey();
				Console.Clear();
				MainMenuOptions();
			}
		}
		#endregion
	}
}
